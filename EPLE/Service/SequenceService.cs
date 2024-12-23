
using EPLE.Manager;
using EPLE.Core.Service;

namespace EPLE.Service
{
    internal class SequenceService : AbstractService
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;

        public SequenceService(ILogger<SequenceService> logger, DataManager dataManager, DeviceManager deviceManager) : base(logger)
        {
            this.deviceManager = deviceManager;
            this.dataManager = dataManager;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await deviceManager.AttachDevices(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await deviceManager.DetachDevices(cancellationToken);
        }
    }
}
