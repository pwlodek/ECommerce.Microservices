using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
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

            using (SalesContext ctx = new SalesContext())
            {
                var order = new Order() { CustomerId = context.Message.CustomerId };

                foreach (var item in context.Message.Items)
                {
                    var product = products.FirstOrDefault(t => t.ProductId == item.ProductId);
                    if (product != null)
                    {
                        order.Items.Add(new OrderItem() { ProductId = item.ProductId, Quantity = item.Quantity, Name = product.Name, Price = product.Price });
                    }
                }

                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }
        }
    }
}
