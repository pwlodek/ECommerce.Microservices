using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Sales.Api.Model
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
