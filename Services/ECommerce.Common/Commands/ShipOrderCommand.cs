using MassTransit;
using System;
namespace ECommerce.Common.Commands
{
    public class ShipOrderCommand : CorrelatedBy<Guid>
    {
        public ShipOrderCommand()
        {
        }

        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }
    }
}
