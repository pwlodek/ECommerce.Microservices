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

    public enum OrderStatus
    {
        Submitted,
        Packing,
        Packed,
        Shipped
    }
}
