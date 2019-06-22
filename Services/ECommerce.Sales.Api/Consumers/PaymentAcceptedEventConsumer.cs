using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Events;
using ECommerce.Sales.Api.Model;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ECommerce.Sales.Api.Consumers
{
    public class PaymentAcceptedEventConsumer : IConsumer<PaymentAcceptedEvent>
    {
        private readonly SalesContext _salesContext;
        private readonly ILogger<PaymentAcceptedEventConsumer> _logger;

        public PaymentAcceptedEventConsumer(SalesContext salesContext, ILogger<PaymentAcceptedEventConsumer> logger)
        {
            _salesContext = salesContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentAcceptedEvent> context)
        {
            var order = _salesContext.Orders.FirstOrDefault(t => t.OrderId == context.Message.OrderId && t.CustomerId == context.Message.CustomerId);
            if (order != null)
            {
                order.Status |= OrderStatus.Payed;
                _salesContext.SaveChanges();
            }

            _logger.LogInformation($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} has been marked as payed");
        }
    }
}
