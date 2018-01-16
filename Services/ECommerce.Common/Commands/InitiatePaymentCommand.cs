using System;
namespace ECommerce.Common.Commands
{
    public class InitiatePaymentCommand
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public double Total { get; set; }
    }
}
