using EPLE.Core.Service.Interface;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace EPLE.Core.Service
{
    public enum Result
    {
        PROCESSING,
        SUCCESS,
        FAILED,
        CANCELED,
        TIMEOUT
    }

    public abstract class AbstractService : IMainService
    {
        protected readonly ILogger logger;

        public AbstractService(ILogger logger)
        {
            this.logger = logger;
        }

        public abstract Task StartAsync(CancellationToken cancellationToken);
        public abstract Task StopAsync(CancellationToken cancellationToken);
    }
}
