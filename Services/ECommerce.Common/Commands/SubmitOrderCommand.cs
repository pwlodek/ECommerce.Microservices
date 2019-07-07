using MassTransit;
using System;
namespace ECommerce.Common.Commands
{
    public class SubmitOrderCommand : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public int CustomerId { get; set; }

        public Item[] Items { get; set; }
    }

    public class Item
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
