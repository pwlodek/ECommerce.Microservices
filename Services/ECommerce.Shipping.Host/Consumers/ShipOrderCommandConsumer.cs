using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using log4net;
using MassTransit;

namespace ECommerce.Shipping.Host.Consumers
{
    public class ShipOrderCommandConsumer : IConsumer<ShipOrderCommand>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ShipOrderCommandConsumer));

        public ShipOrderCommandConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ShipOrderCommand> context)
        {
            Logger.Debug($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} is being shipped");
                   
            await Task.Delay(5000); // shipping takes some time!

            await context.Publish(new OrderCompletedEvent() { 
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId
            });
        }
    }
}
