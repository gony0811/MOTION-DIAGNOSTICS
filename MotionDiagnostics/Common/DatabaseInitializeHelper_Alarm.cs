using EPLE.Data;
using EPLE.Data.Entity;

namespace MotionDiagnostics.Common
{
    internal partial class DatabaseInitializeHelper
    {
        public static AlarmConfigEntity[] AlarmConfig = new[]
        {
            new AlarmConfigEntity { Name = "E1001", Level = ALCD.HEAVY, Text = "MOTION_TIMEOUT", Status = ALST.RESET, Enable = ALED.ENABLE, Description = "MOTION MOVING TIMEOUT" },
        };
    }
}
