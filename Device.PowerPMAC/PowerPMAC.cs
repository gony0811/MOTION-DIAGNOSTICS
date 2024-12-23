using EPLE.Core.Device.Interface;
using Microsoft.Extensions.Logging;

namespace Device
{
    public class PowerPMAC : IDeviceHandler
    {
        public bool DeviceAttach(string arguments)
        {
            throw new NotImplementedException();
        }

        public bool DeviceDettach()
        {
            throw new NotImplementedException();
        }

        public void DeviceInit(ILogger logger)
        {
            throw new NotImplementedException();
        }

        public bool DeviceReset()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string GET_STRING_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public DevMode IsDevMode()
        {
            throw new NotImplementedException();
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
