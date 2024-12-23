using EPLE.Core.Device.Interface;
using Microsoft.Extensions.Logging;

namespace Device
{
    public class Dummy : IDeviceHandler
    {
        private ILogger _logger=null!;
        private DevMode _devMode;

        public bool DeviceAttach(string arguments)
        {
            _logger.LogInformation("DeviceAttach() called");
            _devMode = DevMode.CONNECT;
            return true;
        }

        public bool DeviceDettach()
        {
            _logger.LogInformation("DeviceDettach() called");
            _devMode = DevMode.DISCONNECT;
            return true;
        }

        public void DeviceInit(ILogger logger)
        {
            _logger = logger;
            _devMode = DevMode.DISCONNECT;
            _logger.LogInformation("DeviceInit() called");
        }

        public bool DeviceReset()
        {
            _logger.LogInformation("DeviceReset() called");
            return true;
        }

        public object GET_DATA_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public double GET_DOUBLE_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public int GET_INT_IN(string command, ref bool result)
        {
            result = true;
            return 123;
        }

        public string GET_STRING_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public DevMode IsDevMode()
        {
            return _devMode;
        }

        public void SET_DATA_OUT(string command, object value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public void SET_DOUBLE_OUT(string command, double value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public void SET_INT_OUT(string command, int value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public void SET_STRING_OUT(string command, string value, ref bool result)
        {
            throw new NotImplementedException();
        }
    }
}
