using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using ECommerce.Common.Infrastructure.Messaging;
using ECommerce.Sales.Api.Model;
using ECommerce.Sales.Api.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ECommerce.Sales.Api.Consumers
{
    public class SubmitOrderCommandConsumer : IConsumer<SubmitOrderCommand>
    {
        private readonly IDataService _dataService;
        private readonly SalesContext _salesContext;
        private readonly ILogger<SubmitOrderCommandConsumer> _logger;

        public SubmitOrderCommandConsumer(IDataService dataService, SalesContext salesContext, ILogger<SubmitOrderCommandConsumer> logger)
        {
            _dataService = dataService;
            _salesContext = salesContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubmitOrderCommand> context)
        {
            var customer = await _dataService.GetCustomerAsync(context.Message.CustomerId);
            if (customer == null)
            {
                // probably we want to log this
                _logger.LogWarning($"Submitted invalid order for customer {context.Message.CustomerId}. No such customer");

                return;
            }

            var products = await _dataService.GetProductsAsync();
            var order = new Order() { CustomerId = context.Message.CustomerId, Status = OrderStatus.Submitted };

            double total = 0.0;
            foreach (var item in context.Message.Items)
            {
                var product = products.FirstOrDefault(t => t.ProductId == item.ProductId);
                if (product != null)
                {
                    total += item.Quantity * product.Price;
                    order.Items.Add(new OrderItem() { ProductId = item.ProductId, Quantity = item.Quantity, Name = product.Name, Price = product.Price });
                }
            }

            // Business rule
            if (total > 100)
            {
                total = total * .9; // 10% off
                _logger.LogInformation($"Applying bonus for customer {customer.CustomerId} for the total amount of {total}");
            }
            order.Total = total;

            _salesContext.Orders.Add(order);
            _salesContext.SaveChanges();

            _logger.LogInformation($"Created order {order.OrderId} for customer {customer.CustomerId} for the total amount of {order.Total}");

            await context.Publish(new OrderSubmittedEvent() {
                CorrelationId = context.Message.CorrelationId,
                CustomerId = customer.CustomerId,
                OrderId = order.OrderId,
                Total = order.Total,
                Products = order.Items.Select(t => new SubmittedOrderItem() { ProductId = t.ProductId, Quantity = t.Quantity }).ToArray()
            });
        }
    }
}
