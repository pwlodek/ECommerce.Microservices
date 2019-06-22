using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Sales.Api
{
    public class SalesService : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<SalesService> _logger;

        public SalesService(IBusControl busControl, ILogger<SalesService> logger)
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

            _logger.LogInformation("Running Sales microservice.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync();

            _logger.LogInformation("Sales microservice stopped.");
        }
    }
}
