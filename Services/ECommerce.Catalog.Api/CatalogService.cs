using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Catalog.Api
{
    public class CatalogService : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(IBusControl busControl, ILogger<CatalogService> logger)
        {
            _busControl = busControl;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync();
            _logger.LogInformation("Running Catalog microservice");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync();
            _logger.LogInformation("Stopped Catalog microservice");
        }
    }
}
