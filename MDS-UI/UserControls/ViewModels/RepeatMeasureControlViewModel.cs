using System.Collections.ObjectModel;
using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class RepeatMeasureControlViewModel
    {
        public int SelectedColumn { get; set; }
        public int SelectedRow { get; set; }
        public int RepeatCount { get; set; } = 1;
        public bool IsRunning { get; set; } = false;
        public string Status { get; set; } = "";
        public ObservableCollection<object> MeasureResults { get; set; } = new ObservableCollection<object>();

        public DelegateCommand StartRepeatMeasureCommand { get; }
        public DelegateCommand StopRepeatMeasureCommand { get; }

        public RepeatMeasureControlViewModel()
        {
            StartRepeatMeasureCommand = new DelegateCommand(StartRepeatMeasure);
            StopRepeatMeasureCommand = new DelegateCommand(StopRepeatMeasure);
        }

        public void StartRepeatMeasure() { }

        public void StopRepeatMeasure() { }
    }
}
