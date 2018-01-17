using System;
using Automatonymous;

namespace ECommerce.Shipping.Host.Sagas
{
    public class Shipment : SagaStateMachineInstance
    {
        public Shipment()
        {
        }

        public string CurrentState { get; set; }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public bool IsPacked { get; set; }

        public bool IsPayed { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
