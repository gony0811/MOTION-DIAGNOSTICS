using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MDS.UI.Model;
using MDS.UI.UserControls.ViewModels;
using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class AccuracyViewModel : IDataErrorInfo
    {
        public Brush MotionStatusBackground { get; set; }
        public Brush VisionStatusBackground { get; set; }
        public HorizontalSetting HorizontalSetting { get; set; }
        public string MotionType { get; set; }
        public MotionSetting MotionSetting { get; set; }
        public ModelManagement ModelManagement { get; set; }

        public List<Tuple<int, int>> Selections { get; set; } = new List<Tuple<int, int>>();
        public int SelectedRow { get; set; }
        public int SelectedCol { get; set; }

        public BitmapImage ImageSource { get; set; }

        public int RepeatPointCol { get; set; } = 0;
        public int RepeatPointRow { get; set; } = 0;
        public int RepeatCount { get; set; } = 1;

        public bool IsPneumaticOptionChecked { get; set; } = false;
        public bool IsXGuideSolOptionChecked { get; set; } = false;
        public bool IsXSupportSolOptionChecked { get; set; } = false;
        public bool IsY1GuideSolOptionChecked { get; set; } = false;
        public bool IsY1SupportSolOptionChecked { get; set; } = false;
        public bool IsY2GuideSolOptionChecked { get; set; } = false;
        public bool IsY2SupportSolOptionChecked { get; set; } = false;

        public string Error => string.Empty;

        public int Diameter { get; set; }
        public int GridWidth { get; set; }
        public int GridSize { get; set; }

        public WaferControlViewModel WaferControlViewModel { get; set; }
        public RepeatMeasureControlViewModel RepeatMeasureControlViewModel { get; set; }
        public MappingAndMeasureControlViewModel MappingAndMeasureControlViewModel { get; set; }

        public DelegateCommand LoadedCommand { get; }
        public DelegateCommand UnloadedCommand { get; }
        public DelegateCommand InitializeCommand { get; }
        public DelegateCommand CancelRepeatMeasureCommand { get; }
        public DelegateCommand CancelMovePositionCommand { get; }
        public DelegateCommand MovePositionAsyncCommand { get; }
        public DelegateCommand RepeatPointInputCommand { get; }
        public DelegateCommand<object> SelectionChangedCommand { get; }

        public AccuracyViewModel()
        {
            LoadedCommand = new DelegateCommand(Loaded);
            UnloadedCommand = new DelegateCommand(Unloaded);
            InitializeCommand = new DelegateCommand(Initialize);
            CancelRepeatMeasureCommand = new DelegateCommand(CancelRepeatMeasure);
            CancelMovePositionCommand = new DelegateCommand(CancelMovePosition);
            MovePositionAsyncCommand = new DelegateCommand(MovePositionAsync);
            RepeatPointInputCommand = new DelegateCommand(RepeatPointInput);
            SelectionChangedCommand = new DelegateCommand<object>(SelectionChanged);
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "RepeatCount")
                {
                    if (RepeatCount <= 0)
                    {
                        return "Repeat must be greater than 0";
                    }
                }
                return "";
            }
        }

        public void Loaded() { }

        public void Unloaded() { }

        public void Initialize() { }

        public void CancelRepeatMeasure() { }

        public void CancelMovePosition() { }

        public void MovePositionAsync() { }

        public void RepeatPointInput() { }

        public void SelectionChanged(object position) { }
    }
}
