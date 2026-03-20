namespace MotionDiagnostics.Model
{
    public class HorizontalSetting
    {
        public double AngleChange { get; set; }  // 각도 변경
        public double Angle { get; set; }  // 각도

        public double LeftX { get; set; }
        public double LeftY { get; set; }

        public double CenterX { get; set; }
        public double CenterY { get; set; }

        public double RightX { get; set; }
        public double RightY { get; set; }
        private static HorizontalSetting _instance;

        // Thread-safety를 위한 Lock 객체
        private static readonly object _lock = new object();

        // Private 생성자: 외부에서 직접 인스턴스를 생성하지 못하도록 방지
        private HorizontalSetting()
        {
            AngleChange = 0; Angle = 0;
            LeftX = 0; LeftY = 0;
            CenterX = 0; CenterY = 0;
            RightX = 0; RightY = 0;
        }

        // Public 메서드: 인스턴스를 가져옴 (Lazy Initialization)
        public static HorizontalSetting GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock) // Thread-safe
                {
                    if (_instance == null)
                    {
                        _instance = new HorizontalSetting();
                    }
                }
            }
            return _instance;
        }

        // Optional: 인스턴스를 초기화하는 메서드
        public void Reset()
        {
            AngleChange = 0; Angle = 0;
            LeftX = 0; LeftY = 0;
            CenterX = 0; CenterY = 0;
            RightX = 0; RightY = 0;
        }
    }
}
