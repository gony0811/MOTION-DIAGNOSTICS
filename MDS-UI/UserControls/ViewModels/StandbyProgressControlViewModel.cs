using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class StandbyProgressControlViewModel
    {
        public bool IsRunning { get; set; } = false;
        public double Progress { get; set; } = 0.0;
        public string StatusMessage { get; set; } = "Standby";
        public string CurrentStep { get; set; } = "";
        public int TotalSteps { get; set; } = 0;
        public int CurrentStepIndex { get; set; } = 0;

        public DelegateCommand StartCommand { get; }
        public DelegateCommand StopCommand { get; }
        public DelegateCommand ResetCommand { get; }

        public StandbyProgressControlViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            ResetCommand = new DelegateCommand(Reset);
        }

        public void Start() { }

        public void Stop() { }

        public void Reset() { }
    }
}
