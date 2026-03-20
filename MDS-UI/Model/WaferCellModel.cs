using System.Windows.Controls;

namespace MDS.UI.Model
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
        NotValidated,
        Valid,
        Invalid
    }
}
