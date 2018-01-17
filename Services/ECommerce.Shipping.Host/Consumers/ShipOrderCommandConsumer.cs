using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using MassTransit;

namespace ECommerce.Shipping.Host.Consumers
{
    public class ShipOrderCommandConsumer : IConsumer<ShipOrderCommand>
    {
        public ShipOrderCommandConsumer()
        {
        }

        public async Task Consume(ConsumeContext<ShipOrderCommand> context)
        {
            await context.Publish(new OrderCompletedEvent() { 
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId
            });
        }
    }
}
