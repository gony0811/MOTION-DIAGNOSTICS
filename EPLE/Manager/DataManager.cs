using EPLE.Data;
using EPLE.Core.Device.Interface;
using EPLE.ViewModel;

namespace EPLE.Manager
{
    public class DataManager
    {
        private readonly ILogger<DataManager> logger;
        private readonly DeviceManager deviceManager;
        private readonly DataRepository dataRepository;
        private readonly DataVMList dataVMList;

        public DataManager(ILogger<DataManager> logger, DeviceManager deviceManager, DataRepository dataRepository, DataVMList dataVMList)
        {
            this.logger = logger;
            this.deviceManager = deviceManager;
            this.dataRepository = dataRepository;
            this.dataVMList = dataVMList;

            foreach (var dataConfig in dataRepository.DataConfig)
            {
                if (dataConfig.Use == false) continue;
                dataVMList.DataList.Add(new DataVMList.DataVM(dataConfig, dataRepository));
            }
        }

        public DevMode IsDeviceMode(string deviceName)
        {
            return deviceManager?.IsDeviceMode(deviceName)?? DevMode.DISCONNECT;
        }

        public bool SET_DATA(string name, object value)
        {
            bool result = false;

            var dataVM = dataVMList.DataList.SingleOrDefault((item) => (item.Name == name));

            if (dataVM == null)
            {
                logger.LogError("[SET_DATA] {0} : DataVM is null", name);
                return false;
            }
            else if (dataVM.DeviceName.StartsWith('V'))
            {
                logger.LogDebug("[SET_DATA] {0} : {1}", name, value.ToString());
                dataVM.Value = value;
                dataVM.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff");
                dataVM.SaveChanges();
                result = true;
            }
            else if (deviceManager.SetDataToDevice(name, value) && dataVM.Direction == Direction.OUT)
            {
                logger.LogDebug("[SET_DATA] {0} : {1}", name, value.ToString());
                dataVM.Value = value;
                dataVM.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff");
                dataVM.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public bool GET_DATA(string name, out object? value)
        {
            bool result = false;

            var dataVM = dataVMList.DataList.SingleOrDefault((item) => (item.Name == name));

            if (dataVM == null)
            {
                logger.LogError("[GET_DATA] {0} : DataVM is null", name);
                value = null;
                return result;
            }
            else if (dataVM.DeviceName.StartsWith('V'))
            {
                logger.LogDebug("[GET_DATA] {0} : {1}", name, dataVM.Value.ToString());
                value = dataVM.Value;
                result = true;
            }
            else
            {
                result = deviceManager.GetDataFromDevice(name, out value);
                if (result)
                {
                    logger.LogInformation("[GET_DATA] {0} : {1}", name, value?.ToString() ?? "null");
                }
                else
                {
                    value = dataVM.Value;
                }
            }

            return result;
        }

        public bool SET_DATA(string name, object value, bool setDefaultValue)
        {
            bool result = false;

            var dataVM = dataVMList.DataList.SingleOrDefault((item) => (item.Name == name));

            if (dataVM == null)
            {
                logger.LogError("[SET_DATA] {0} : DataVM is null", name);
                return false;
            }
            else if (dataVM.DeviceName.StartsWith('V'))
            {
                logger.LogDebug("[SET_DATA] {0} : {1}", name, value.ToString());
                dataVM.Value = value;
                dataVM.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff");
                dataVM.SaveChanges();
                result = true;
            }
            else if (deviceManager.SetDataToDevice(name, value) && dataVM.Direction == Direction.OUT)
            {
                logger.LogDebug("[SET_DATA] {0} : {1}", name, value.ToString());
                dataVM.Value = value;
                dataVM.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff");
                dataVM.SaveChanges();
                result = true;
            }
            else
            {
                logger.LogError("[SET_DATA] {0} : cannot write to device, value={1}", name, value.ToString());
                result = false;
            }

            if (setDefaultValue && result)
            {
                dataVM.DefaultValue = value?.ToString()?? string.Empty;
                dataVM.SaveChanges();
            }

            return result;
        }
    }
}
