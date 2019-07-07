using MassTransit;
using System;
namespace ECommerce.Common.Commands
{
    public class InitiatePaymentCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public double Total { get; set; }
    }
}
