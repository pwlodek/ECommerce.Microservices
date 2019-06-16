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
    public class OrderPackedEventConsumer : IConsumer<OrderPackedEvent>
    {
        private readonly SalesContext _salesContext;
        private readonly ILogger<OrderPackedEventConsumer> _logger;

        public OrderPackedEventConsumer(SalesContext salesContext, ILogger<OrderPackedEventConsumer> logger)
        {
            _salesContext = salesContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPackedEvent> context)
        {
            var order = _salesContext.Orders.FirstOrDefault(t => t.OrderId == context.Message.OrderId && t.CustomerId == context.Message.CustomerId);
            if (order != null)
            {
                order.Status |= OrderStatus.Packed;

                _salesContext.SaveChanges();
            }

            _logger.LogInformation($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} has been marked as packed");
        }
    }
}
