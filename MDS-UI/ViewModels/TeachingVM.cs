using MDS.UI.Model;
using PropertyChanged;
using Prism.Commands;

namespace MDS.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class TeachingVM
    {
        public string LabelContent { get; set; }
        public string Name { get; set; }
        public double CommandPosition { get; set; }
        public double FeedbackPosition { get; set; }
        public double Velocity { get; set; }
        public double Pitch { get; set; }
        public bool AlarmStatus { get; set; }
        public bool EnableStatus { get; set; }
        public bool BusyStatus { get; set; }

        public DelegateCommand<object> TeachCommand { get; private set; }
        public DelegateCommand<object> SetCommand { get; private set; }
        public DelegateCommand<object> MoveCommand { get; private set; }

        public TeachingVM(string name)
        {
            this.Name = name;
            this.LabelContent = this.Name.Split(new char[] { ' ' })[1] + " POS";
            TeachCommand = new DelegateCommand<object>((p) => { });
            SetCommand = new DelegateCommand<object>((p) => { });
            MoveCommand = new DelegateCommand<object>((p) => { });
        }
    }
}
