namespace MDS.UI.Model
{
    public class HorizontalSetting
    {
        public double AngleChange { get; set; }
        public double Angle { get; set; }
        public double LeftX { get; set; }
        public double LeftY { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double RightX { get; set; }
        public double RightY { get; set; }

        private static HorizontalSetting _instance;
        private static readonly object _lock = new object();

        private HorizontalSetting()
        {
            AngleChange = 0; Angle = 0;
            LeftX = 0; LeftY = 0;
            CenterX = 0; CenterY = 0;
            RightX = 0; RightY = 0;
        }

        public static HorizontalSetting GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new HorizontalSetting();
                    }
                }
            }
            return _instance;
        }

        public void Reset()
        {
            AngleChange = 0; Angle = 0;
            LeftX = 0; LeftY = 0;
            CenterX = 0; CenterY = 0;
            RightX = 0; RightY = 0;
        }
    }
}
