using PropertyChanged;

namespace MDS.UI.Model
{
    [AddINotifyPropertyChangedInterface]
    public partial class PositionStatus
    {
        public string PositionName { get; set; }
        public double CommandPosition { get; set; }
        public double FeedbackPosition { get; set; }
        public double UnitPulse { get; set; }
        public bool PlusLimit { get; set; }
        public bool HomeStat { get; set; }
        public bool MinusLimit { get; set; }
        public bool Enable { get; set; }
        public bool InPosition { get; set; }
        public bool Alarm { get; set; }
        public bool Busy { get; set; }
        public bool HomeDone { get; set; }

        public PositionStatus()
        {
            PositionName = string.Empty;
            CommandPosition = 0.0;
            FeedbackPosition = 0.0;
            UnitPulse = 0.0;
            PlusLimit = false;
            HomeStat = false;
            MinusLimit = false;
            Enable = false;
            InPosition = false;
            Alarm = false;
            Busy = false;
            HomeDone = false;
        }
    }
}
