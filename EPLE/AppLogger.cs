using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE
{
    public static class AppLogger
    {
        public static ILoggerFactory Factory = new LoggerFactory().AddSerilog(Log.Logger);

        public static string GetDeviceLoggerCategory(string deviceType, string deviceName) => $"{deviceType}_{deviceName}";
    }
}
