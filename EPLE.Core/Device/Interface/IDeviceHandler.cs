using Microsoft.Extensions.Logging;

namespace EPLE.Core.Device.Interface
{
    public enum DevMode
    {
        UNKNOWN,
        CONNECT,
        DISCONNECT,
        SIMULATE,
        ERROR
    }
    public interface IDeviceHandler
    {
        bool DeviceAttach(string arguments);
        bool DeviceDettach();
        void DeviceInit(ILogger logger);
        bool DeviceReset();

        DevMode IsDevMode();

        void SET_INT_OUT(string command, int value, ref bool result);

        int GET_INT_IN(string command, ref bool result);

        void SET_DOUBLE_OUT(string command, double value, ref bool result);


        double GET_DOUBLE_IN(string command, ref bool result);

        void SET_STRING_OUT(string command, string value, ref bool result);

        string GET_STRING_IN(string command, ref bool result);

        object GET_DATA_IN(string command, ref bool result);

        void SET_DATA_OUT(string command, object value, ref bool result);
    }
}
