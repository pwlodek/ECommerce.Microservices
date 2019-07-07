using MassTransit;
using System;
namespace ECommerce.Common.Events
{
    public class PaymentAcceptedEvent : CorrelatedBy<Guid>
    {
        public PaymentAcceptedEvent()
        {
        }

        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public double Total { get; set; }
    }
}
