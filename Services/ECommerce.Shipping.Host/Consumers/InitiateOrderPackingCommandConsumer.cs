using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ECommerce.Shipping.Host.Consumers
{
    public class InitiateOrderPackingCommandConsumer : IConsumer<InitiateOrderPackingCommand>
    {
        private readonly ILogger<InitiateOrderPackingCommandConsumer> _logger;

        public InitiateOrderPackingCommandConsumer(ILogger<InitiateOrderPackingCommandConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<InitiateOrderPackingCommand> context)
        {
            _logger.LogDebug($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} is being packed");

            await Task.Delay(10000);

            await context.Publish(new OrderPackedEvent()
            {
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId
            });
        }
    }
}
