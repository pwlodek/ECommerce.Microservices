using MassTransit;
using System;

namespace ECommerce.Common.Events
{
    public class OrderSubmittedEvent : CorrelatedBy<Guid>
    {
        public OrderSubmittedEvent()
        {
        }

        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public double Total { get; set; }

        public SubmittedOrderItem[] Products { get; set; }
    }

    public class SubmittedOrderItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
