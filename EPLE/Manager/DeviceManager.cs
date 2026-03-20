using EPLE.Data;
using EPLE.Core.Device.Interface;
using System.Reflection;
using DataType = EPLE.Data.DataType;
using System.Collections.ObjectModel;
using EPLE.Data.Entity;
using EPLE.ViewModel;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using EPLE.Data.Repository;

namespace EPLE.Manager
{
    public delegate void DeviceLoadEvent(string deviceName);
    public class DeviceManager
    {
        private readonly ILogger logger;
        private readonly DataRepository dataRepository;
        private readonly Dictionary<string, IDeviceHandler> deviceHandlerDict = new Dictionary<string, IDeviceHandler>();
        private readonly DeviceVMList deviceVMList;

        private bool isDeviceAttached = false;

        public bool IsDeviceAttached()
        {
            return isDeviceAttached;
        }

        public EventHandler DeviceAttachEvent;

        public DeviceManager(ILogger logger, DataRepository dataRepository, DeviceVMList deviceVMList)
        {
            this.logger = logger;
            this.dataRepository = dataRepository;
            this.deviceVMList = deviceVMList;
        }

        private void MakeDeviceList()
        {
            this.deviceVMList.Devices.Clear();
            this.deviceHandlerDict.Clear();

            foreach (var device in dataRepository.DeviceConfig)
            {
                if (device.IsUse == false) continue;
                string pullPath = Path.GetFullPath(device.FileName);
                Assembly assembly = Assembly.LoadFile(pullPath);
                if (device.DeviceName == null)
                    throw new Exception($"{device.DeviceName} DeviceName이 null 입니다.");
                var instance = assembly.CreateInstance(device.InstanceName);
                if (!(assembly.CreateInstance(device.InstanceName) is IDeviceHandler deviceInstance))
                    throw new Exception($"Device file has some problems (Device filename={device.FileName} | Device Name={device.DeviceName})");
                deviceInstance.DeviceInit(logger);
                if (device.IsUse)
                {
                    this.deviceVMList.Devices.Add(new DeviceVMList.DeviceVM(device, dataRepository));
                    deviceHandlerDict.Add(device.DeviceName, deviceInstance);
                }
            }
        }

        public async Task AttachDevices(CancellationToken cancellationToken)
        {
            MakeDeviceList();

            await Task.WhenAll(dataRepository.DeviceConfig.Select(device => Task.Run(() =>
            {
                try
                {
                    if (device.IsUse == false)
                    {
                        logger.Information("Device [{DeviceName}] {DeviceType} is not used", device.DeviceName, device.DeviceType);
                        return;
                    }

                    if (!deviceHandlerDict.TryGetValue(device.DeviceName, out IDeviceHandler deviceInstance))
                    {
                        throw new Exception($"Device({device.DeviceName}) is not detached");
                    }

                    var deviceAttachSuccess = deviceInstance?.DeviceAttach(device.Args) ?? false;

                    if (!deviceAttachSuccess)
                    {
                        deviceHandlerDict.Remove(device.DeviceName);
                        logger.Error("Device({DeviceName}) Attach Failed : DLL file name is {FileName}", device.DeviceName, device.FileName);
                    }
                    else
                    {
                        logger.Information("Device [{DeviceName}] {DeviceType} Attached", device.DeviceName, device.DeviceType);
                    }

                }
                catch (NotImplementedException ex)
                {
                    logger.Error("DeviceAttach() {ex}", ex.Message);
                }
                catch (Exception ex)
                {
                    logger.Error("DeviceAttach() {ex}", ex.Message);
                }
            }, cancellationToken)).ToArray());

            if (deviceHandlerDict.Count == 0)
            {
                isDeviceAttached = false;
            }
            else
            {
                isDeviceAttached = true;
                DeviceAttachEvent?.Invoke(this, EventArgs.Empty); // Event 발생
            }
        }

        public async Task DetachDevices(CancellationToken cancellationToken)
        {
            await Task.WhenAll(this.dataRepository.DeviceConfig.Select(device => Task.Run(() =>
            {
                try
                {
                    if (!deviceHandlerDict.TryGetValue(device.DeviceName, out IDeviceHandler deviceInstance))
                    {
                        throw new Exception($"Device({device.DeviceName}) is not detached");
                    }

                    var deviceDetachSuccess = deviceInstance?.DeviceDettach() ?? false;
                    if (!deviceDetachSuccess)
                        logger.Error("Device({DeviceName}) Detach Failed : DLL file name is {FileName}", device.DeviceName, device.FileName);
                    else
                    {
                        logger.Information("Device [{DeviceName}] {DeviceType} Detached", device.DeviceName, device.DeviceType);
                        var deviceVM = deviceVMList.Devices.FirstOrDefault(x => x.DeviceName == device.DeviceName) ?? null;
                        if (deviceVM != null)
                            this.deviceVMList.Devices.Remove(deviceVM);
                    }
                }
                catch (NotImplementedException ex)
                {
                    logger.Error("DeviceDettach() {ex}", ex.Message);
                }
                catch (Exception ex)
                {
                    logger.Error("DeviceDettach() {ex}", ex.Message);
                }
            }, cancellationToken)).ToArray());

            this.deviceVMList.Devices.Clear();
            this.deviceHandlerDict.Clear();
            isDeviceAttached = false;
        }

        public string GetDeviceName(string deviceType)
        {
            return dataRepository.DeviceConfig.Where(x => x.DeviceType == deviceType && x.IsUse == true).Select(x => x.DeviceName).FirstOrDefault();
        }

        public DevMode IsDeviceMode(string driverName)
        {
            if (deviceHandlerDict.ContainsKey(driverName) && deviceHandlerDict[driverName] != null)
            {
                return deviceHandlerDict[driverName].IsDevMode();
            }
            else
            {
                return DevMode.DETTACHED;
            }
        }

        public bool GetDataFromDevice(string name, out object value)
        {
            bool result = false;

            try
            {
                var data = dataRepository.DataConfig.Where(x => x.Name == name).Single();

                value = null;

                if (data == null)
                {
                    throw new Exception("Data Name is not exist(Name : {name})");
                }
                else if (data.DeviceName.ToUpper().Equals("VIRTUAL"))
                {
                    return false;
                }

                var devMode = this.IsDeviceMode(data.DeviceName);

                if (data.DeviceName.ToUpper().Equals("VIRTUAL") || devMode != DevMode.CONNECT)
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
                                logger.Debug("[ERROR] DataType is unknown!!! : {Name} / {Type}", data.Name, data.Type.ToString());
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("[ERROR] GetDataFromDevice() => {ex}", ex.Message);
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

                if (data == null)
                {
                    throw new Exception("Data Name is not exist(Name : {name})");
                }
                else if (data.DeviceName.ToUpper().Equals("VIRTUAL"))
                {
                    return false;
                }

                var devMode = this.IsDeviceMode(data.DeviceName);

                if (data.DeviceName.ToUpper().Equals("VIRTUAL") || devMode != DevMode.CONNECT)
                {
                    return false;
                }
                else
                {
                    switch (data.Type)
                    {
                        case DataType.INT:
                            {
                                value = Convert.ChangeType(value, typeof(int));
                                deviceHandlerDict[data.DeviceName].SET_INT_OUT(data.Command, (int)value, ref result);
                            }
                            break;
                        case DataType.DOUBLE:
                            {
                                value = Convert.ChangeType(value, typeof(double));
                                deviceHandlerDict[data.DeviceName].SET_DOUBLE_OUT(data.Command, (double)value, ref result);
                            }
                            break;
                        case DataType.STRING:
                            {
                                value = Convert.ChangeType(value, typeof(string));
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
                                logger.Debug("[ERROR] DataType is unknown!!! :  {Name} / {Type}", data.Name, data.Type.ToString());
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("[ERROR] SetDataToDevice() : {0}", ex.Message);
                return false;
            }

            if (!result) return false;

            return true;
        }

        public List<DeviceConfigEntity> GetAllDevice()
        {
            return dataRepository.DeviceConfig.ToList();
        }

        public void Update(DeviceConfigEntity entity)
        {
            if (entity?.Id == null)
            {
                Save(entity);
                return;
            }

            dataRepository.UpdateDeviceConfig(entity);
        }

        public void Save(DeviceConfigEntity entity)
        {
            dataRepository.AddDeviceConfig(entity);

        }

        public void Delete(int id)
        {
            dataRepository.DeleteDeviceConfig(id);
        }
    }
}
