using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using MassTransit;

namespace ECommerce.Payment.Host.Consumers
{
    public class InitiatePaymentCommandConsumer : IConsumer<InitiatePaymentCommand>
    {
        public InitiatePaymentCommandConsumer()
        {
        }

        public async Task Consume(ConsumeContext<InitiatePaymentCommand> context)
        {
            await Task.Delay(2000); // simulate payment

            Console.WriteLine($"Processing payment for order {context.Message.OrderId} by customer {context.Message.CustomerId} in the amount of {context.Message.Total}");

            // Payment was accepted
            await context.Publish(new PaymentAcceptedEvent() { 
                OrderId = context.Message.OrderId,
                CustomerId = context.Message.CustomerId,
                Total = context.Message.Total
            });
        }
    }
}
