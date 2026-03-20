using EPLE.Core.Device.Interface;
using System;
using Serilog;

namespace Device
{
    public class Dummy : IDeviceHandler
    {
        private ILogger _logger;
        private DevMode _devMode;

        public bool DeviceAttach(string arguments)
        {
            _logger.Information("DeviceAttach() called");
            _devMode = DevMode.CONNECT;
            return true;
        }

        public bool DeviceDettach()
        {
            _logger.Information("DeviceDettach() called");
            _devMode = DevMode.DISCONNECT;
            return true;
        }

        public void DeviceInit(ILogger logger)
        {
            _logger = logger;
            _devMode = DevMode.DISCONNECT;
            _logger.Information("DeviceInit() called");
        }

        public bool DeviceReset()
        {
            _logger.Information("DeviceReset() called");
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
            switch (command)
            {
                case "TEST_COMMAND_OUT":
                    _logger.Information("SET_INT_OUT() called with value {0}", value);
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
        }

        public void SET_STRING_OUT(string command, string value, ref bool result)
        {
            throw new NotImplementedException();
        }
    }
}
