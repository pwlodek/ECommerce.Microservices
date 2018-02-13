using System;
using System.Threading.Tasks;
using ECommerce.Common.Events;
using MassTransit;

namespace ECommerce.Reporting.Api.Consumers
{
    public class OrderSubmittedEventConsumer : IConsumer<OrderSubmittedEvent>
    {
        public OrderSubmittedEventConsumer()
        {
        }

        public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
        {
        }
    }
}
