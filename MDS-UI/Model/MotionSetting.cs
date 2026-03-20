using PropertyChanged;

namespace MDS.UI.Model
{
    [AddINotifyPropertyChangedInterface]
    public partial class MotionSetting
    {
        public string MotionDevice { get; set; }
        public double MotionX { get; set; }
        public double MotionY { get; set; }
        public double MotionZ1 { get; set; }
        public double MotionZ2 { get; set; }
        public double MotionZ3 { get; set; }
        public double StepPitchXY { get; set; }
        public double StepPitchZ { get; set; }
        public int ZIndex { get; set; }

        public MotionSetting()
        {
            MotionX = 0; MotionY = 0;
            MotionZ1 = 0; MotionZ2 = 0; MotionZ3 = 0;
            StepPitchXY = 0; StepPitchZ = 0; ZIndex = 0;
        }

        public void Reset()
        {
            MotionX = 0; MotionY = 0;
            MotionZ1 = 0; MotionZ2 = 0; MotionZ3 = 0;
            StepPitchXY = 0; StepPitchZ = 0; ZIndex = 0;
        }
    }
}
