using GreenPipes;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce.Common.Infrastructure.Messaging
{
    public class CorrelationIdSpecification<T> : IPipeSpecification<T> where T : class, PipeContext
    {
        private readonly IMessageCorrelationContextAccessor _messageCorrelationContextAccessor;

        public CorrelationIdSpecification(IMessageCorrelationContextAccessor messageCorrelationContextAccessor)
        {
            _messageCorrelationContextAccessor = messageCorrelationContextAccessor;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<T> builder)
        {
            builder.AddFilter(new CorrelationIdFilter<T>(_messageCorrelationContextAccessor));
        }
    }
}
