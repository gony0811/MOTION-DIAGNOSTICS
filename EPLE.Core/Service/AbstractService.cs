using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EPLE.Core.Service
{
    public abstract class AbstractService : IHostedService
    {
        protected readonly ILogger<AbstractService> logger;

        public AbstractService(ILogger<AbstractService> logger)
        {
            this.logger = logger;
        }

        public abstract Task StartAsync(CancellationToken cancellationToken);
        public abstract Task StopAsync(CancellationToken cancellationToken);
    }
}
