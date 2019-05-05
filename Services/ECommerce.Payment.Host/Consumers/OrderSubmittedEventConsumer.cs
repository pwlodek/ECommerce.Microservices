using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ECommerce.Payment.Host.Consumers
{
    public class OrderSubmittedEventConsumer : IConsumer<OrderSubmittedEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderSubmittedEventConsumer> _logger;

        public OrderSubmittedEventConsumer(IConfiguration configuration, ILogger<OrderSubmittedEventConsumer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
        {
            var endpoint = await context.GetSendEndpoint(new Uri($"rabbitmq://{_configuration["RabbitHost"]}/payment_initiate_payment"));

            _logger.LogInformation($"Initiating payment for customer {context.Message.CustomerId}, order {context.Message.OrderId} in total of {context.Message.Total}");

            await endpoint.Send(new InitiatePaymentCommand() {
                CustomerId = context.Message.CustomerId,
                OrderId = context.Message.OrderId,
                Total = context.Message.Total
            });
        }
    }
}
