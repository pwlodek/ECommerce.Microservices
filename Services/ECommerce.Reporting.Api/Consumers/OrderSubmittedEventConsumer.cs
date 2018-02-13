using System;
using System.Threading.Tasks;
using ECommerce.Common.Events;
using ECommerce.Reporting.Api.Models;
using ECommerce.Reporting.Api.Services;
using MassTransit;

namespace ECommerce.Reporting.Api.Consumers
{
    public class OrderSubmittedEventConsumer : IConsumer<OrderSubmittedEvent>
    {
        readonly IDataService _dataService;
        readonly IOrderRepository _orderRepository;

        public OrderSubmittedEventConsumer(IDataService dataService, IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
            this._dataService = dataService;
        }

        public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
        {
            var customer = await _dataService.GetCustomerAsync(context.Message.CustomerId);
            var order = new Order() { 
                OrderId = context.Message.OrderId, 
                CustomerId = context.Message.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Status = 0,
                Total = context.Message.Total
            };

            _orderRepository.Add(order);
        }
    }
}
