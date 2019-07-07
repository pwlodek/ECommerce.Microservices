using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Common.Infrastructure.Messaging
{
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        private const string DefaultHeader = "X-Correlation-ID";

        private readonly IMessageCorrelationContextAccessor _correlationContextAccessor;

        public CorrelationIdDelegatingHandler(IMessageCorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationContextAccessor.CorrelationId;
            if (correlationId != Guid.Empty)
            {
                request.Headers.Add(DefaultHeader, correlationId.ToString());
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
