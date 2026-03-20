using System.Windows.Controls;

namespace MotionDiagnostics.Model
{
    public class WaferCellModel
    {
        public Button ButtonInfo { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsWaferSurface { get; set; }
        public int ErrorX { get; set; }
        public int ErrorY { get; set; }
        public ValidationState validationState { get; set; } = ValidationState.NotValidated;
    }
    public enum ValidationState
    {
        NotValidated,   // 검증 안 함
        Valid,          // 검증됨
        Invalid         // 검증 실패
    }
}
