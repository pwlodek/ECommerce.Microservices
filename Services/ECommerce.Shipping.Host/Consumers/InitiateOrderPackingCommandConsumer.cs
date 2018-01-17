using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using MassTransit;

namespace ECommerce.Shipping.Host.Consumers
{
    public class InitiateOrderPackingCommandConsumer : IConsumer<InitiateOrderPackingCommand>
    {
        public InitiateOrderPackingCommandConsumer()
        {
        }

        public async Task Consume(ConsumeContext<InitiateOrderPackingCommand> context)
        {
            await context.Publish(new OrderPackedEvent()
            {
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId
            });
        }
    }
}
