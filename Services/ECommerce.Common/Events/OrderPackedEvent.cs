using System;
namespace ECommerce.Common.Events
{
    public class OrderPackedEvent
    {
        public OrderPackedEvent()
        {
        }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }
    }
}
