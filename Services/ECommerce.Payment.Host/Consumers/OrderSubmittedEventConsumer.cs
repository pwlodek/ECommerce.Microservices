using System;
using System.Threading.Tasks;
using ECommerce.Common;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using MassTransit;

namespace ECommerce.Payment.Host.Consumers
{
    public class OrderSubmittedEventConsumer : IConsumer<OrderSubmittedEvent>
    {
        public OrderSubmittedEventConsumer()
        {
        }

        public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
        {
            var endpoint = await context.GetSendEndpoint(new Uri($"rabbitmq://{Configuration.RabbitMqHost}/payment_initiate_payment"));
            await endpoint.Send(new InitiatePaymentCommand() {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
                Total = context.Message.Total
            });
        }
    }
}
