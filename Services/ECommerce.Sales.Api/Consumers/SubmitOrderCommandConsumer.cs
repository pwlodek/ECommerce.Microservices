using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using ECommerce.Sales.Api.Model;
using MassTransit;

namespace ECommerce.Sales.Api.Consumers
{
    public class SubmitOrderCommandConsumer : IConsumer<SubmitOrderCommand>
    {
        public SubmitOrderCommandConsumer()
        {
        }

        public async Task Consume(ConsumeContext<SubmitOrderCommand> context)
        {
            using (SalesContext ctx = new SalesContext())
            {
                var order = new Order() { CustomerId = context.Message.CustomerId };

                foreach (var item in context.Message.Items)
                {
                    order.Items.Add(new OrderItem() {  ProductId = item.ProductId, Quantity = item.Quantity, Name = "sdf", Price = 7878});
                }

                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }
        }
    }
}
