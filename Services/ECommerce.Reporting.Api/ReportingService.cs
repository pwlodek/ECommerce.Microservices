using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Reporting.Api
{
    public class ReportingService : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<ReportingService> _logger;

        public ReportingService(IBusControl busControl, ILogger<ReportingService> logger)
        {
            _busControl = busControl;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting service bus");

            try
            {
                await _busControl.StartAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while startin service bus.");
                throw;
            }

            _logger.LogInformation("Running Reporting microservice.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync();

            _logger.LogInformation("Reporting microservice stopped.");
        }
    }
}
