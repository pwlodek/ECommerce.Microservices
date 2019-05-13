using ECommerce.Common;
using ECommerce.Services.Common.Configuration;
using MassTransit;
using Microsoft.Extensions.Logging;
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
            var waiter = new DependencyAwaiter();
            waiter.WaitForRabbit(Configuration.RabbitMqHost);

            await _busControl.StartAsync(cancellationToken);

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
