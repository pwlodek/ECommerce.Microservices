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
                var order = new Order() { CustomerId = 1, Status = OrderStatus.Submitted, Total = 10 };
                order.Items.Add(new OrderItem() { Name = "sdf", Price = 123, Quantity = 111 });

                ctx.Orders.Add(order);
                ctx.SaveChanges();
            }
        }
    }
}
