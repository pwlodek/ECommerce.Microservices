using MassTransit;
using System;
namespace ECommerce.Common.Commands
{
    public class InitiateOrderPackingCommand : CorrelatedBy<Guid>
    {
        public InitiateOrderPackingCommand()
        {
        }

        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }
    }
}
