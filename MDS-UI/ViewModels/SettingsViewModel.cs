using System.Collections.Generic;
using System.Collections.ObjectModel;
using MDS.UI.Model;
using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class SettingsViewModel
    {
        public PositionStatus PositionStatus { get; set; }
        public double Speed { get; set; } = 1;
        public ObservableCollection<TableViewModel> TableViewModels { get; set; } = new ObservableCollection<TableViewModel>();
        public Dictionary<string, TeachingVM> TeachingVMMap { get; set; }

        public DelegateCommand CreateDeviceCommand { get; }

        public SettingsViewModel()
        {
            PositionStatus = new PositionStatus();

            var positions = new List<string>
            {
                "LOADING_T_POS", "LOADING_X_POS", "LOADING_Y_POS",
                "INSPECTION_T_POS",
                "INSPECTION_X_POS_TOP", "INSPECTION_Y_POS_TOP",
                "INSPECTION_X_POS_LEFT", "INSPECTION_Y_POS_LEFT",
                "INSPECTION_X_POS_CENTER", "INSPECTION_Y_POS_CENTER",
                "INSPECTION_X_POS_RIGHT", "INSPECTION_Y_POS_RIGHT",
                "INSPECTION_X_POS_BOTTOM", "INSPECTION_Y_POS_BOTTOM"
            };

            TeachingVMMap = new Dictionary<string, TeachingVM>();
            foreach (var position in positions)
            {
                TeachingVMMap[position] = new TeachingVM(position.Replace("_POS", "").Replace("_", " "));
            }

            CreateDeviceCommand = new DelegateCommand(CreateDevice);
        }

        public void CreateDevice() { }
    }
}
