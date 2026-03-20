using Prism.Commands;
using PropertyChanged;

namespace MDS.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class TableViewModel
    {
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string FileName { get; set; }
        public string InstanceName { get; set; }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        public TableViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand(Delete);
        }

        public void Save() { }

        public void Delete() { }
    }
}
