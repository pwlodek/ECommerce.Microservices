using MassTransit;
using System;
namespace ECommerce.Common.Events
{
    public class OrderCompletedEvent : CorrelatedBy<Guid>
    {
        public OrderCompletedEvent()
        {
        }

        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }
    }
}
