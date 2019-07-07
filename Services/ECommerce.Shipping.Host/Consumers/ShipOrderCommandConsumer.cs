using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ECommerce.Shipping.Host.Consumers
{
    public class ShipOrderCommandConsumer : IConsumer<ShipOrderCommand>
    {
        private readonly ILogger<ShipOrderCommandConsumer> _logger;

        public ShipOrderCommandConsumer(ILogger<ShipOrderCommandConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ShipOrderCommand> context)
        {
            _logger.LogDebug($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} is being shipped");
                   
            await Task.Delay(5000); // shipping takes some time!

            await context.Publish(new OrderCompletedEvent() { 
                CorrelationId = context.Message.CorrelationId,
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId
            });
        }
    }
}
