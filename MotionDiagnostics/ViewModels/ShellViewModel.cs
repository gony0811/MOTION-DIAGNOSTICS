using System;
using System.Threading.Tasks;
using System.Threading;
using PropertyChanged;
using Prism.Commands;
using EPLE.Service;
using EPLE.ImageProcessing;
using PrismCommands;
using Telerik.Windows.Controls;

namespace MotionDiagnostics.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class ShellViewModel
    {
        private readonly HalconImageProcessing imageProcessing;
        private SequenceService sequenceService;
        private CancellationTokenSource deviceStartCts = new CancellationTokenSource();
        private CancellationTokenSource deviceStopCts = new CancellationTokenSource();

        public string DeviceStatus { get; set; } = "Device Stop";
        public int SelectedTabIndex { get; set; } = 0;
        public bool IsBusy { get; set; } = false;
        public string BusyMessage { get; set; } = "";

        public ShellViewModel(SequenceService sequenceService, HalconImageProcessing imageProcessing)
        {
            this.sequenceService = sequenceService;
            this.imageProcessing = imageProcessing;
        }

        [DelegateCommand]
        public async void DeviceConnect()
        {
            try
            {
                await Task.Delay(1000);

                if (DeviceStatus == "Device Running")
                {
                    IsBusy = true;
                    BusyMessage = "Device disconnect process...";

                    await Task.Delay(1000);

                    deviceStopCts = new CancellationTokenSource();
                    deviceStartCts.Cancel();
                    DeviceStatus = "Device Stop";

                    await this.sequenceService.StopAsync(deviceStopCts.Token);

                    IsBusy = false;
                }
                else
                {
                    IsBusy = true;
                    BusyMessage = "Device connect process...";

                    await Task.Delay(1000);

                    deviceStartCts = new CancellationTokenSource();
                    deviceStopCts.Cancel();
                    DeviceStatus = "Device Running";
                    await this.sequenceService.StartAsync(deviceStartCts.Token);

                    IsBusy = false;
                }
            }
            catch (TaskCanceledException)
            {
                IsBusy = false;
            }
        }
    }
}
