using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Payment.Host
{
    public class PaymentService : Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IBusControl busControl, ILogger<PaymentService> logger)
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

            _logger.LogInformation("Running Payment microservice.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync();

            _logger.LogInformation("Payment microservice stopped.");
        }
    }
}
