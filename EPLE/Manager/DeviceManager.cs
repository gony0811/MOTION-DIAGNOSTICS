using EPLE.Data;
using EPLE.Core.Device.Interface;
using System.Reflection;
using DataType = EPLE.Data.DataType;

namespace EPLE.Manager
{
    public delegate void DeviceLoadEvent(string deviceName);

    public class DeviceManager
    {
        private readonly ILogger<DeviceManager> logger;
        private readonly DataRepository dataRepository;
        private readonly Dictionary<string, IDeviceHandler> deviceHandlerDict = new();

        public DeviceManager(ILogger<DeviceManager> logger, DataRepository dataRepository)
        {
            this.logger = logger;
            this.dataRepository = dataRepository;

            foreach (var device in dataRepository.DeviceConfig)
            {
                if (device.Use == false) continue;
                string pullPath = Path.GetFullPath(device.FileName);
                Assembly assembly = Assembly.LoadFile(pullPath);
                if (device.DeviceName == null)
                    throw new Exception($"{device.DeviceName} DeviceName이 null 입니다.");

                if (assembly.CreateInstance(device.InstanceName) is not IDeviceHandler deviceInstance)
                    throw new Exception($"Device file has some problems (Device filename={device.FileName} | Device Name={device.DeviceName})");

                deviceInstance.DeviceInit(AppLogger.Factory.CreateLogger(AppLogger.GetDeviceLoggerCategory(device.DeviceType, device.DeviceName)));

                deviceHandlerDict.TryAdd(device.DeviceName, deviceInstance);
            }
        }

        public async Task AttachDevices(CancellationToken cancellationToken)
        {
            await Task.WhenAll(dataRepository.DeviceConfig.Select(device => Task.Run(() =>
            {
                try
                {
                    if (device.Use == false)
                    {
                        logger.LogInformation("Device [{DeviceName}] {DeviceType} is not used", device.DeviceName, device.DeviceType);
                        return;
                    }

                    var deviceInstance = deviceHandlerDict.GetValueOrDefault(device.DeviceName);
                    var deviceAttachSuccess = deviceInstance?.DeviceAttach(device.Args) ?? false;

                    if (!deviceAttachSuccess)
                        logger.LogError("Device({DeviceName}) Attach Failed : DLL file name is {FileName}", device.DeviceName, device.FileName);
                    else
                        logger.LogInformation("Device [{DeviceName}] {DeviceType} Attached", device.DeviceName, device.DeviceType);
                }
                catch (NotImplementedException ex)
                {
                    logger.LogError("DeviceAttach() {ex}", ex.Message);
                }
                catch (Exception ex)
                {
                    logger.LogError("DeviceAttach() {ex}", ex.Message);
                }
            }, cancellationToken)).ToArray());
        }

        public async Task DetachDevices(CancellationToken cancellationToken)
        {
            await Task.WhenAll(dataRepository.DeviceConfig.Select(device => Task.Run(() =>
            {
                try
                {
                    var deviceInstance = deviceHandlerDict.GetValueOrDefault(device.DeviceName);
                    var deviceDetachSuccess = deviceInstance?.DeviceDettach() ?? false;
                    if (!deviceDetachSuccess)
                        logger.LogError("Device({DeviceName}) Detach Failed : DLL file name is {FileName}", device.DeviceName, device.FileName);
                    else
                        logger.LogInformation("Device [{DeviceName}] {DeviceType} Detached", device.DeviceName, device.DeviceType);
                }
                catch (NotImplementedException ex)
                {
                    logger.LogError("DeviceDettach() {ex}", ex.Message);
                }
                catch (Exception ex)
                {
                    logger.LogError("DeviceDettach() {ex}", ex.Message);
                }
            }, cancellationToken)).ToArray());
        }

        public DevMode IsDeviceMode(string driverName)
        {
            if (deviceHandlerDict.ContainsKey(driverName) && deviceHandlerDict[driverName] != null)
            {
                return deviceHandlerDict[driverName].IsDevMode();
            }
            else
            {
                return DevMode.UNKNOWN;
            }
        }

        public bool GetDataFromDevice(string name, out object? value)
        {
            bool result = false;

            try
            {
                var data = dataRepository.DataConfig.Where(x => x.Name == name).Single();

                value = null;

                if (data == null) return false;

                if (!data.DeviceName.ToUpper().Equals("VIRTUAL") && deviceHandlerDict.ContainsKey(data.DeviceName) == false)
                {
                    throw new KeyNotFoundException(string.Format("Execption : DeviceName is wrong [Data.DeviceName = {0}]", data.DeviceName));

                }

                if (deviceHandlerDict[data.DeviceName] == null || !data.Use) return false;


                var devMode = deviceHandlerDict[data.DeviceName].IsDevMode();

                if (devMode == DevMode.DISCONNECT || devMode == DevMode.UNKNOWN)
                {
                    return false;
                }
                else
                {
                    switch (data.Type)
                    {
                        case DataType.INT:
                            {
                                value = deviceHandlerDict[data.DeviceName].GET_INT_IN(data.Command, ref result);
                            }
                            break;
                        case DataType.DOUBLE:
                            {
                                value = deviceHandlerDict[data.DeviceName].GET_DOUBLE_IN(data.Command, ref result);
                            }
                            break;
                        case DataType.STRING:
                            {
                                value = deviceHandlerDict[data.DeviceName].GET_STRING_IN(data.Command, ref result);
                            }
                            break;
                        case DataType.OBJECT:
                            {
                                value = deviceHandlerDict[data.DeviceName].GET_DATA_IN(data.Command, ref result);
                            }
                            break;
                        default:
                            {
                                value = null;
                                logger.LogDebug("[ERROR] DataType is unknown!!! : {Name} / {Type}", data.Name, data.Type.ToString());
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError("[ERROR] GetDataFromDevice() : {ex}", ex.Message);
                value = null;
                return false;
            }

            if (!result) return false;

            return true;
        }

        public bool SetDataToDevice(string name, object value)
        {
            bool result = false;

            try
            {

                var data = dataRepository.DataConfig.Where(x => x.Name == name).Single();
                if (data == null) return false;

                if (deviceHandlerDict[data.DeviceName] == null || !data.Use) return false;

                var devMode = deviceHandlerDict[data.DeviceName].IsDevMode();

                if (devMode == DevMode.DISCONNECT || devMode == DevMode.UNKNOWN)
                {
                    return false;
                }
                else
                {
                    switch (data.Type)
                    {
                        case DataType.INT:
                            {
                                deviceHandlerDict[data.DeviceName].SET_INT_OUT(data.Command, (int)value, ref result);
                            }
                            break;
                        case DataType.DOUBLE:
                            {
                                deviceHandlerDict[data.DeviceName].SET_DOUBLE_OUT(data.Command, (double)value, ref result);
                            }
                            break;
                        case DataType.STRING:
                            {
                                deviceHandlerDict[data.DeviceName].SET_STRING_OUT(data.Command, (string)value, ref result);
                            }
                            break;
                        case DataType.OBJECT:
                            {
                                deviceHandlerDict[data.DeviceName].SET_DATA_OUT(data.Command, value, ref result);
                            }
                            break;
                        default:
                            {
                                logger.LogDebug("[ERROR] DataType is unknown!!! : {0} / {1}", data.Name, data.Type.ToString());
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError("[ERROR] SetDataToDevice() : {0}", ex.Message);
                return false;
            }

            if (!result) return false;

            return true;
        }
    }
}
