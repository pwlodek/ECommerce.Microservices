using CorrelationId;
using ECommerce.Common.Infrastructure.Messaging;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Sales.Api.Configuration
{
    public class CustomTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ICorrelationContextAccessor _accessor;
        private readonly IMessageCorrelationContextAccessor _messageAccessor;

        public CustomTelemetryInitializer(
            ICorrelationContextAccessor accessor,
            IMessageCorrelationContextAccessor messageAccessor)
        {
            _accessor = accessor;
            _messageAccessor = messageAccessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                //set custom role name here
                telemetry.Context.Cloud.RoleName = "Sales Api";
                //telemetry.Context.Cloud..RoleInstance = "Custom RoleInstance";
            }

            var messageCorrelation = _messageAccessor.CorrelationId;
            if (messageCorrelation != Guid.Empty)
            {
                ISupportProperties properties = (ISupportProperties)telemetry;
                properties.Properties["CorrelationId"] = messageCorrelation.ToString();
            }
            else
            {
                var correlationContext = _accessor.CorrelationContext;
                if (correlationContext != null)
                {
                    ISupportProperties properties = (ISupportProperties)telemetry;
                    properties.Properties["CorrelationId"] = correlationContext.CorrelationId;
                }
            }
        }
    }
}
