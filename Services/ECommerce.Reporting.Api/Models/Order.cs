using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Reporting.Api.Models
{
    public class Order
    {
        public Order()
        {
        }

        [Key]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Status { get; set; }

        public double Total { get; set; }
    }
}
