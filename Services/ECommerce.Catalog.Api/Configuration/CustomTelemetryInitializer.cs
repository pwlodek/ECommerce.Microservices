using CorrelationId;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Catalog.Api.Configuration
{
    public class CustomTelemetryInitializer : ITelemetryInitializer
    {
        private ICorrelationContextAccessor _accessor;

        public CustomTelemetryInitializer(ICorrelationContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                //set custom role name here
                telemetry.Context.Cloud.RoleName = "Catalog Api";
                //telemetry.Context.Cloud..RoleInstance = "Custom RoleInstance";
            }

            var correlationContext = _accessor.CorrelationContext;
            if (correlationContext != null)
            {
                ISupportProperties properties = (ISupportProperties)telemetry;
                properties.Properties["CorrelationId"] = correlationContext.CorrelationId;
            }
        }
    }
}
