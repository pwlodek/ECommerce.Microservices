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

            try
            {
                // Try processing the payment
                await ProcessPayment(context.Message);

                // Payment was accepted
                await context.Publish(new PaymentAcceptedEvent()
                {
                    CorrelationId = context.Message.CorrelationId,
                    OrderId = context.Message.OrderId,
                    CustomerId = context.Message.CustomerId,
                    Total = context.Message.Total
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while processing payment operation for order {context.Message.OrderId}");
                throw;
            }
        }

        private async Task ProcessPayment(InitiatePaymentCommand message)
        {
            await Task.Delay(5000); // simulate payment

            _count++;

            if (message.Total > 500)
            {
                // Sometimes payments over 500 fail :)
                if (_count % 2 == 0)
                {
                    throw new InvalidOperationException("Payment gateway rejected pyment");
                }
            }
        }

        private static int _count;
    }
}
