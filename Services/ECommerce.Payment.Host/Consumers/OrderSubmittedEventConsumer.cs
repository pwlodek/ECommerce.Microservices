using System;
using System.Threading.Tasks;
using ECommerce.Common;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using log4net;
using MassTransit;

namespace ECommerce.Payment.Host.Consumers
{
    public class OrderSubmittedEventConsumer : IConsumer<OrderSubmittedEvent>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(OrderSubmittedEventConsumer));

        public OrderSubmittedEventConsumer()
        {
        }

        public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
        {
            var endpoint = await context.GetSendEndpoint(new Uri($"rabbitmq://{Configuration.RabbitMqHost}/payment_initiate_payment"));

            Logger.Info($"Initiating payment for customer {context.Message.CustomerId}, order {context.Message.OrderId} in total of {context.Message.Total}");

            await endpoint.Send(new InitiatePaymentCommand() {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
                Total = context.Message.Total
            });
        }
    }
}
