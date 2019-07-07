using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Catalog.Api
{
    public class CatalogService : IHostedService
    {
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(ILogger<CatalogService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running Catalog microservice");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopped Catalog microservice");
        }
    }
}
