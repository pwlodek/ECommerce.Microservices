using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public double Total { get; set; }
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
