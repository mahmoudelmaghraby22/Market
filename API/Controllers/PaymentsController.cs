using System.IO;
using System.Threading.Tasks;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Order = Core.Entities.OrderAggregate.Order;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private const string whSecret = "whsec_wajx0kyx6iaxsDoBfdw1UrlyWt1LU1NS";
        private readonly ILogger<IPaymentService> _Logger;
        public PaymentsController(IPaymentService paymentService, ILogger<IPaymentService> Logger)
        {
            _Logger = Logger;
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null) return BadRequest(new ApiResponse(400, "Problem with your basket"));

            return basket;
        }

        [HttpPost("weebhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], whSecret);

            PaymentIntent intent;
            Order order;

            switch (stripEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripEvent.Data.Object;
                    _Logger.LogInformation("Payment succeeded: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _Logger.LogInformation("Order updated to payment recived", order.Id);
                    break;

                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripEvent.Data.Object;
                    _Logger.LogInformation("Payment Failed: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _Logger.LogInformation("Payment failed", order.Id);
                    break;
            }

            return new EmptyResult();
        }
    }
}