using System;
using System.Collections.Generic;

namespace ECommerce.Common.Events
{
    public class OrderSubmittedEvent
    {
        public OrderSubmittedEvent()
        {
        }

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
