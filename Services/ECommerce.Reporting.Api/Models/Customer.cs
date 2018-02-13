using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Reporting.Api.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
