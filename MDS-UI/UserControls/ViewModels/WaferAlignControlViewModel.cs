using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class WaferAlignControlViewModel
    {
        public double AlignAngle { get; set; }
        public double AlignX { get; set; }
        public double AlignY { get; set; }
        public bool IsAligning { get; set; } = false;
        public string AlignStatus { get; set; } = "";

        public DelegateCommand StartAlignCommand { get; }
        public DelegateCommand StopAlignCommand { get; }

        public WaferAlignControlViewModel()
        {
            StartAlignCommand = new DelegateCommand(StartAlign);
            StopAlignCommand = new DelegateCommand(StopAlign);
        }

        public void StartAlign() { }

        public void StopAlign() { }
    }
}
