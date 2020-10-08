using Core.Entities.OrderAggregate;

namespace Core.Specification
{
    public class OrderByPaymentIntentIdSpecifications : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecifications(string paymentIntendId) 
            : base(o => o.PaymentIntendId == paymentIntendId)
        {
        }
    }
}