using System;
using System.Threading;

namespace ECommerce.Common.Infrastructure.Messaging
{
    public interface IMessageCorrelationContextAccessor
    {
        Guid CorrelationId { get; set; }
    }

    public class MessageCorrelationContextAccessor : IMessageCorrelationContextAccessor
    {
        private static AsyncLocal<Guid> _correlationContext = new AsyncLocal<Guid>();

        public Guid CorrelationId
        {
            get => _correlationContext.Value;
            set => _correlationContext.Value = value;
        }
    }
}
