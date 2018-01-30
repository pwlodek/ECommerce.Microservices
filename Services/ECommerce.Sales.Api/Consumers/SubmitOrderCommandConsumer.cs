using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using ECommerce.Sales.Api.Model;
using ECommerce.Sales.Api.Services;
using log4net;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Sales.Api.Consumers
{
    public class SubmitOrderCommandConsumer : IConsumer<SubmitOrderCommand>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SubmitOrderCommandConsumer));

        private readonly IDataService _dataService;

        private readonly IConfiguration _cfg;

        public SubmitOrderCommandConsumer(IDataService dataService, IConfiguration cfg)
        {
            _dataService = dataService;
            _cfg = cfg;
        }

        public async Task Consume(ConsumeContext<SubmitOrderCommand> context)
        {
            var customer = await _dataService.GetCustomerAsync(context.Message.CustomerId);
            if (customer == null)
            {
                // probably we want to log this
                Logger.Warn($"Submitted invalid order for customer {context.Message.CustomerId}. No such customer");

                return;
            }

            var products = await _dataService.GetProductsAsync();
            var order = new Order() { CustomerId = context.Message.CustomerId };

            using (SalesContext ctx = new SalesContext(_cfg["ConnectionString"]))
            {
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
                    Logger.Info($"Applying bonus for customer {customer.CustomerId} for the total amount of {total}");
                }
                order.Total = total;

                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }

            Logger.Info($"Created order {order.OrderId} for customer {customer.CustomerId} for the total amount of {order.Total}");

            await context.Publish(new OrderSubmittedEvent() {
                CustomerId = customer.CustomerId,
                OrderId = order.OrderId,
                Total = order.Total,
                Products = order.Items.Select(t => new SubmittedOrderItem() { ProductId = t.ProductId, Quantity = t.Quantity }).ToArray()
            });
        }
    }
}
