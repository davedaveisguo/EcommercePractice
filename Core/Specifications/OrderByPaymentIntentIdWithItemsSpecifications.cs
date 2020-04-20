using System;
using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderByPaymentIntentIdWithItemsSpecifications : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdWithItemsSpecifications(string paymentIntentId) : base(o => o.PaymentIntentId ==paymentIntentId)
        {
        }
    }
}