using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Sales.Api.Model
{
    public class Order
    {
        public Order()
        {
            Items = new List<OrderItem>();
        }

        [Key]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public double Total { get; set; }

        public IList<OrderItem> Items { get; set; }
    }

    [Flags]
    public enum OrderStatus
    {
        None = 0,
        Submitted = 1,
        Packed = 2,
        Payed = 4,
        Shipped = 8
    }
}
