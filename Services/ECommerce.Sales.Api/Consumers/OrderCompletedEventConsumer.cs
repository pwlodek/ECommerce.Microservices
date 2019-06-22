using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Events;
using ECommerce.Sales.Api.Model;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ECommerce.Sales.Api.Consumers
{
    public class OrderCompletedEventConsumer : IConsumer<OrderCompletedEvent>
    {
        private readonly SalesContext _salesContext;
        private readonly ILogger<OrderCompletedEventConsumer> _logger;

        public OrderCompletedEventConsumer(SalesContext salesContext, ILogger<OrderCompletedEventConsumer> logger)
        {
            _salesContext = salesContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            var order = _salesContext.Orders.FirstOrDefault(t => t.OrderId == context.Message.OrderId && t.CustomerId == context.Message.CustomerId);
            if (order != null)
            {
                order.Status &= ~OrderStatus.Submitted; // unsettings Submitted state
                order.Status |= OrderStatus.Shipped; // setting Shipped state

                _salesContext.SaveChanges();
            }

            _logger.LogInformation($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} has been marked as shipped");
        }
    }
}
