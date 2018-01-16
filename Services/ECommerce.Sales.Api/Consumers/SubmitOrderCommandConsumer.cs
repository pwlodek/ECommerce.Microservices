using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using ECommerce.Sales.Api.Model;
using ECommerce.Sales.Api.Services;
using MassTransit;

namespace ECommerce.Sales.Api.Consumers
{
    public class SubmitOrderCommandConsumer : IConsumer<SubmitOrderCommand>
    {
        readonly IDataService _dataService;

        public SubmitOrderCommandConsumer(IDataService dataService)
        {
            this._dataService = dataService;
        }

        public async Task Consume(ConsumeContext<SubmitOrderCommand> context)
        {
            var customer = await _dataService.GetCustomerAsync(context.Message.CustomerId);
            if (customer == null)
            {
                // probably we want to log this
                return;
            }

            var products = await _dataService.GetProductsAsync();
            var order = new Order() { CustomerId = context.Message.CustomerId };

            using (SalesContext ctx = new SalesContext())
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
                }
                order.Total = total;

                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }

            await context.Publish(new OrderSubmittedEvent() {
                CustomerId = customer.CustomerId,
                OrderId = order.OrderId,
                Total = order.Total,
                Products = order.Items.Select(t => new SubmittedOrderItem() { ProductId = t.ProductId, Quantity = t.Quantity }).ToArray()
            });
        }
    }
}
