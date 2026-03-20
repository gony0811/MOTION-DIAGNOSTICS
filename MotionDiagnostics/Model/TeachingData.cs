namespace MotionDiagnostics.Model
{
    public class TeachingData
    {
        public double CommandPosition { get; set; }
        public double FeedbackPosition { get; set; }
        public double Velocity { get; set; }
        public double Pitch { get; set; }
        public bool AlarmStatus { get; set; }
        public bool EnableStatus { get; set; }
        public bool BusyStatus { get; set; }
    }
}
