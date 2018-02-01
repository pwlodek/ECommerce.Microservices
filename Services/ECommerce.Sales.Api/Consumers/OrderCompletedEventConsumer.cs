using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Events;
using ECommerce.Sales.Api.Model;
using log4net;
using MassTransit;

namespace ECommerce.Sales.Api.Consumers
{
    public class OrderCompletedEventConsumer : IConsumer<OrderCompletedEvent>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(OrderCompletedEventConsumer));

        private readonly SalesContext _salesContext;

        public OrderCompletedEventConsumer(SalesContext salesContext)
        {
            _salesContext = salesContext;
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

            Logger.Info($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} has been marked as shipped");
        }
    }
}
