using GreenPipes;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace ECommerce.Common.Infrastructure.Messaging
{
    public class CorrelationIdFilter<T> : IFilter<T> where T : class, PipeContext
    {
        private readonly IMessageCorrelationContextAccessor _messageCorrelationContextAccessor;

        public CorrelationIdFilter(IMessageCorrelationContextAccessor messageCorrelationContextAccessor)
        {
            _messageCorrelationContextAccessor = messageCorrelationContextAccessor;
        }

        public void Probe(ProbeContext context)
        {
        }

        public async Task Send(T context, IPipe<T> next)
        {
            try
            {
                var ctx = context as MessageContext;
                if (ctx != null && ctx.CorrelationId.HasValue)
                {
                    _messageCorrelationContextAccessor.CorrelationId = ctx.CorrelationId.Value;
                }
                await next.Send(context);
            }
            finally
            {
                _messageCorrelationContextAccessor.CorrelationId = Guid.Empty;
            }
        }
    }
}
