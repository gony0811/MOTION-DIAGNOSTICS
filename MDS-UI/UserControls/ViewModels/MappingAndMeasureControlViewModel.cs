using System;
using System.Collections.Generic;
using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class MappingAndMeasureControlViewModel
    {
        public List<Tuple<int, int>> Selections { get; set; } = new List<Tuple<int, int>>();
        public bool IsMeasuring { get; set; } = false;
        public string MeasureStatus { get; set; } = "";

        public DelegateCommand StartMeasureCommand { get; }
        public DelegateCommand StopMeasureCommand { get; }

        public MappingAndMeasureControlViewModel()
        {
            StartMeasureCommand = new DelegateCommand(StartMeasure);
            StopMeasureCommand = new DelegateCommand(StopMeasure);
        }

        public void StartMeasure() { }

        public void StopMeasure() { }
    }
}
