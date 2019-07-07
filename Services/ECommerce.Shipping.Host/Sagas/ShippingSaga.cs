using System;
using Automatonymous;
using ECommerce.Common.Commands;
using ECommerce.Common.Events;
using Microsoft.Extensions.Logging;

namespace ECommerce.Shipping.Host.Sagas
{
    public class ShippingSaga : MassTransitStateMachine<Shipment>
    {
        private readonly ILogger<ShippingSaga> _logger;

        public ShippingSaga(ILogger<ShippingSaga> logger)
        {
            InstanceState(t => t.CurrentState);

            Event(() => OrderSubmitted, x => x.CorrelateById(s => s.OrderId, context => context.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Event(() => PaymentReceived, x => x.CorrelateById(s => s.OrderId, context => context.Message.OrderId));

            Event(() => OrderPacked, x => x.CorrelateById(s => s.OrderId, context => context.Message.OrderId));

            Event(() => OrderCompleted, x => x.CorrelateById(s => s.OrderId, context => context.Message.OrderId));

            Initially(
                When(OrderSubmitted)
                .Then(OnOrderSubmitted)
                .TransitionTo(Submitted));

            During(Submitted,
                   When(PaymentReceived).Then(OnPaymentReceived),
                   When(OrderPacked).Then(OnOrderPacked),
                   When(OrderCompleted).Then(OnOrderComplete).TransitionTo(Shipped).Finalize()
                  );

            SetCompletedWhenFinalized();

            _logger = logger;
        }

        private async void OnOrderSubmitted(BehaviorContext<Shipment, OrderSubmittedEvent> context)
        {
            context.Instance.CustomerId = context.Data.CustomerId;
            context.Instance.OrderId = context.Data.OrderId;
            context.Instance.CorrelationId = context.Data.CorrelationId;

            _logger.LogInformation($"Saga: Order {context.Instance.OrderId} submitted by customer {context.Instance.CustomerId}");

            await context.RespondAsync(new InitiateOrderPackingCommand() {
                CorrelationId = context.Instance.CorrelationId,
                CustomerId = context.Instance.CustomerId,
                OrderId = context.Instance.OrderId
            });
        }

        private async void OnOrderPacked(BehaviorContext<Shipment, OrderPackedEvent> context)
        {
            context.Instance.IsPacked = true;

            _logger.LogInformation($"Saga: Order {context.Instance.OrderId} submitted by customer {context.Instance.CustomerId} is packed.");

            if (context.Instance.IsPacked && context.Instance.IsPayed)
            {
                await context.RespondAsync(new ShipOrderCommand() {
                    CorrelationId = context.Instance.CorrelationId,
                    CustomerId = context.Instance.CustomerId,
                    OrderId = context.Instance.OrderId
                });
            }
        }

        private async void OnPaymentReceived(BehaviorContext<Shipment, PaymentAcceptedEvent> context)
        {
            context.Instance.IsPayed = true;

            _logger.LogInformation($"Saga: Order {context.Instance.OrderId} submitted by customer {context.Instance.CustomerId} is payed.");

            if (context.Instance.IsPacked && context.Instance.IsPayed)
            {
                await context.RespondAsync(new ShipOrderCommand() {
                    CorrelationId = context.Instance.CorrelationId,
                    CustomerId = context.Instance.CustomerId,
                    OrderId = context.Instance.OrderId
                });
            }
        }

        private async void OnOrderComplete(BehaviorContext<Shipment, OrderCompletedEvent> context)
        {
            _logger.LogInformation($"Saga: Order {context.Instance.OrderId} submitted by customer {context.Instance.CustomerId} has shipped.");
        }

        public State Submitted { get; set; }

        public State Shipped { get; set; }

        public Event<OrderSubmittedEvent> OrderSubmitted { get; private set; }

        public Event<PaymentAcceptedEvent> PaymentReceived { get; private set; }

        public Event<OrderPackedEvent> OrderPacked { get; private set; }

        public Event<OrderCompletedEvent> OrderCompleted { get; private set; }
    }
}
