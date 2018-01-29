using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using log4net;
using MassTransit;

namespace ECommerce.Shipping.Host.Consumers
{
    public class InitiateOrderPackingCommandConsumer : IConsumer<InitiateOrderPackingCommand>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InitiateOrderPackingCommandConsumer));

        public InitiateOrderPackingCommandConsumer()
        {
        }

        public async Task Consume(ConsumeContext<InitiateOrderPackingCommand> context)
        {
            Logger.Debug($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} is being packed");

            await Task.Delay(5000);

            await context.Publish(new OrderPackedEvent()
            {
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId
            });
        }
    }
}
