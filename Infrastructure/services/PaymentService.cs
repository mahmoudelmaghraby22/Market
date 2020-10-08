using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.Extensions.Configuration;
using Stripe;
using Order = Core.Entities.OrderAggregate.Order;
using Product = Core.Entities.Product;

namespace Infrastructure.services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _congig;
        public PaymentService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IConfiguration congig)
        {
            _congig = congig;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _congig["StripeSettings:Secretkey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket == null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync((int)basket.DeliveryMethodId);

                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var services = new PaymentIntentService();

            PaymentIntent intent;

            // for create a new paymentIntent for user
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"}
                };

                intent = await services.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            
            // if the intent already created and user go to buy or remove something
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100,
                };

                await services.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if(order == null) return null;

            order.Status = OrderStatus.PaymentFailed;
            
            await _unitOfWork.Complete();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecifications(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if(order == null) return null;

            order.Status = OrderStatus.PaymentRecevied;
            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();

            return order;
        }
    }
}