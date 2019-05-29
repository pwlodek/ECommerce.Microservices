using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Shipping.Host
{
    public class ShippingService : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<ShippingService> _logger;

        public ShippingService(IBusControl busControl, ILogger<ShippingService> logger)
        {
            this._busControl = busControl;
            this._logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting service bus");

            try
            {
                await _busControl.StartAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while starting service bus.");
                throw;
            }

            _logger.LogInformation("Running Shipping microservice.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Shipping microservice.");
            await _busControl.StopAsync(cancellationToken);
            _logger.LogInformation("Shipping microservice stopped.");
        }
    }
}
