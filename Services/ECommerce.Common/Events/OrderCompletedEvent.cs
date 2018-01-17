using System;
namespace ECommerce.Common.Events
{
    public class OrderCompletedEvent
    {
        public OrderCompletedEvent()
        {
        }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }
    }
}
