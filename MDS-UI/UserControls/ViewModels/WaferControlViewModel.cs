using System.Collections.ObjectModel;
using MDS.UI.Model;
using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class WaferControlViewModel
    {
        public int Diameter { get; set; } = 300;
        public int GridWidth { get; set; } = 10;
        public int GridSize { get; set; }
        public bool IsMultiSelection { get; set; } = false;
        public bool IsDragSelection { get; set; } = false;
        public object HeatmapDataSeries { get; set; }

        public int WaferGridRowCount { get; set; }
        public int WaferGridColumnCount { get; set; }
        public int SelectedRow { get; set; }
        public int SelectedCol { get; set; }
        public int buttonSize { get; } = 10;

        public ObservableCollection<ObservableCollection<WaferCellModel>> WaferGrids { get; set; }

        public DelegateCommand HeatMapRefreshCommand { get; }
        public DelegateCommand LoadedCommand { get; }

        public WaferControlViewModel()
        {
            HeatMapRefreshCommand = new DelegateCommand(HeatMapRefresh);
            LoadedCommand = new DelegateCommand(Loaded);
        }

        public void WaferGridRefresh() { }

        public void HeatMapRefresh() { }

        public void Loaded() { }
    }
}
