using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ECommerce.Payment.Host.Consumers
{
    public class InitiatePaymentCommandConsumer : IConsumer<InitiatePaymentCommand>
    {
        private readonly ILogger<InitiatePaymentCommandConsumer> _logger;

        public InitiatePaymentCommandConsumer(ILogger<InitiatePaymentCommandConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<InitiatePaymentCommand> context)
        {
            _logger.LogInformation($"Processing payment for order {context.Message.OrderId} by customer {context.Message.CustomerId} in the amount of {context.Message.Total}");

            await Task.Delay(5000); // simulate payment

            // Payment was accepted
            await context.Publish(new PaymentAcceptedEvent() { 
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId,
                Total = context.Message.Total
            });
        }
    }
}
