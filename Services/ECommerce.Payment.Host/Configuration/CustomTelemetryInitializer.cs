using ECommerce.Common.Infrastructure.Messaging;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace ECommerce.Payment.Host.Configuration
{
    class CustomTelemetryInitializer : ITelemetryInitializer
    {
        private IMessageCorrelationContextAccessor _accessor;

        public CustomTelemetryInitializer(IMessageCorrelationContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                //set custom role name here
                telemetry.Context.Cloud.RoleName = "Payment Host";
                //telemetry.Context.Cloud..RoleInstance = "Custom RoleInstance";
            }

            var correlationContext = _accessor.CorrelationId;
            if (correlationContext != Guid.Empty)
            {
                ISupportProperties properties = (ISupportProperties)telemetry;
                properties.Properties["CorrelationId"] = correlationContext.ToString();
            }
        }
    }
}
