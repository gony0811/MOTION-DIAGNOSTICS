using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class WaferLoadingControlViewModel
    {
        public bool IsLoading { get; set; } = false;
        public string LoadingStatus { get; set; } = "";
        public double Progress { get; set; } = 0.0;

        public DelegateCommand StartLoadingCommand { get; }
        public DelegateCommand StopLoadingCommand { get; }

        public WaferLoadingControlViewModel()
        {
            StartLoadingCommand = new DelegateCommand(StartLoading);
            StopLoadingCommand = new DelegateCommand(StopLoading);
        }

        public void StartLoading() { }

        public void StopLoading() { }
    }
}
