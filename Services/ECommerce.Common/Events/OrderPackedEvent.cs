using MassTransit;
using System;
namespace ECommerce.Common.Events
{
    public class OrderPackedEvent : CorrelatedBy<Guid>
    {
        public OrderPackedEvent()
        {
        }

        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }
    }
}
