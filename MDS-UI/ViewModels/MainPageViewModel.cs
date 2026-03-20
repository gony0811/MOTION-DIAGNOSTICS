using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Commands;
using PropertyChanged;
using MDS.UI.UserControls.ViewModels;

namespace MDS.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class MainPageViewModel
    {
        public StandbyProgressControlViewModel StandbyProgressControlViewModel { get; set; }
        public WaferLoadingControlViewModel WaferLoadingControlViewModel { get; set; }
        public WaferAlignControlViewModel WaferAlignControlViewModel { get; set; }

        public Brush MotionStatusBackground { get; set; }
        public Brush VisionStatusBackground { get; set; }
        public string MotionType { get; set; }
        public string VisionType { get; set; }
        private BitmapImage ImageSource { get; set; }

        // MotionControl properties
        public DelegateCommand ServoEnableCommand { get; private set; }
        public DelegateCommand ServoDisableCommand { get; private set; }
        public DelegateCommand ServoHomeCommand { get; private set; }
        public DelegateCommand ServoStopCommand { get; private set; }
        public DelegateCommand JogPlusCommand { get; private set; }
        public DelegateCommand JogMinusCommand { get; private set; }
        public DelegateCommand MoveAbsCommand { get; private set; }
        public DelegateCommand MoveRelCommand { get; private set; }
        public DelegateCommand UserInputVelocityCommand { get; private set; }
        public DelegateCommand VelocityCheckedCommand { get; private set; }
        public DelegateCommand ServoEStopCommand { get; private set; }
        public DelegateCommand MotionControl_LoadedCommand { get; private set; }
        public DelegateCommand MotionControl_UnloadedCommand { get; private set; }

        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand UnloadedCommand { get; }

        public string SelectedMotionAxis { get; set; }
        public bool IsPlusLimitChecked { get; set; }
        public bool IsMinusLimitChecked { get; set; }
        public bool IsEnableChecked { get; set; }
        public bool IsDisableChecked { get; set; }
        public bool IsErrorChecked { get; set; }
        public bool IsHommingChecked { get; set; }
        public bool IsCalibrationChecked { get; set; }
        public bool IsInPositionChecked { get; set; }
        public bool IsBusyChecked { get; set; }
        public bool IsInputVelocityChecked { get; set; }
        public bool IsLowVelocityChecked { get; set; }
        public bool IsMidVelocityChecked { get; set; }
        public bool IsHighVelocityChecked { get; set; }

        public double InputVelocity { get; set; } = 0.0;
        public double LowVelocity { get; set; } = 10.0;
        public double MidVelocity { get; set; } = 30.0;
        public double HighVelocity { get; set; } = 50.0;
        public double MoveAbsPosition { get; set; } = 0.0;
        public double MoveRelDistance { get; set; } = 0.0;
        public double ActualPosition { get; set; } = 0.0;
        public double CommandPosition { get; set; } = 0.0;
        public string ServoErrorText { get; set; } = "";
        public double ActualVelocity { get; set; } = 0.0;
        public double CommandVelocity { get; set; } = 0.0;
        public List<string> MotionAxisList { get; set; }

        public MainPageViewModel()
        {
            MotionAxisList = new List<string> { "X", "Y", "T", "Z1" };
            SelectedMotionAxis = "X";

            ServoEnableCommand = new DelegateCommand(() => { });
            ServoDisableCommand = new DelegateCommand(() => { });
            ServoHomeCommand = new DelegateCommand(() => { });
            ServoStopCommand = new DelegateCommand(() => { });
            JogPlusCommand = new DelegateCommand(() => { });
            JogMinusCommand = new DelegateCommand(() => { });
            MoveAbsCommand = new DelegateCommand(() => { });
            MoveRelCommand = new DelegateCommand(() => { });
            UserInputVelocityCommand = new DelegateCommand(() => { });
            VelocityCheckedCommand = new DelegateCommand(() => { });
            ServoEStopCommand = new DelegateCommand(() => { });
            MotionControl_LoadedCommand = new DelegateCommand(() => { });
            MotionControl_UnloadedCommand = new DelegateCommand(() => { });

            LoadedCommand = new DelegateCommand(Loaded);
            UnloadedCommand = new DelegateCommand(Unloaded);
        }

        public void Loaded() { }

        public void Unloaded() { }
    }
}
