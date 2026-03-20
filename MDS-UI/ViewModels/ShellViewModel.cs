using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class ShellViewModel
    {
        public string DeviceStatus { get; set; } = "Device Stop";
        public int SelectedTabIndex { get; set; } = 0;
        public bool IsBusy { get; set; } = false;
        public string BusyMessage { get; set; } = "";

        public DelegateCommand DeviceConnectCommand { get; }

        public ShellViewModel()
        {
            DeviceConnectCommand = new DelegateCommand(DeviceConnect);
        }

        public void DeviceConnect() { }
    }
}
