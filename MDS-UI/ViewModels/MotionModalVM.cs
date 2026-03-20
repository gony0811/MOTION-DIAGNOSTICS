using PropertyChanged;

namespace MDS.UI.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class MotionModalVM
    {
        public string Name { get; set; }
        public double CommandPosition { get; set; }
        public double FeedbackPosition { get; set; }
        public double Velocity { get; set; }

        public void SetData(string name) { }
    }
}
