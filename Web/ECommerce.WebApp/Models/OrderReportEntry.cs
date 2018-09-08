using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models
{
    public class OrderReportEntry
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public OrderStatus Status { get; set; }

        public double Total { get; set; }
    }
}
