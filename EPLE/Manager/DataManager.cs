using EPLE.Data;
using EPLE.Core.Device.Interface;
using EPLE.ViewModel;
using EPLE.Manager.Alarm;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using EPLE.Data.Repository;
using Serilog;

namespace EPLE.Manager
{
    public class DataManager
    {
        private readonly ILogger logger;
        private readonly DeviceManager deviceManager;
        private readonly DataRepository dataRepository;
        private readonly DataVMList dataVMList;
        private readonly object eventLock = new object();
        private IDialogService dialogService = null;
        public EventHandler<DataVMList.DataVM> DataChangedEvent;


        public DataManager(ILogger logger, DeviceManager deviceManager, DataRepository dataRepository, DataVMList dataVMList)
        {
            this.logger = logger;
            this.deviceManager = deviceManager;
            this.dataRepository = dataRepository;
            this.dataVMList = dataVMList;

            foreach (var dataConfig in dataRepository.DataConfig)
            {
                if (dataConfig.IsUse == false) continue;
                dataVMList.DataList.Add(new DataVMList.DataVM(dataConfig, dataRepository));
            }
        }

        public async Task InitializeData(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    foreach (var data in dataVMList.DataList)
                    {
                        if (data.Use == false) continue;
                        if (string.IsNullOrEmpty(data.DefaultValue)) continue;
                        if (data.Direction == Direction.OUT || data.Direction == Direction.BOTH)
                        {

                            if (SET_DATA(data.Name, data.DefaultValue))
                            {
                                logger.Debug("[InitializeData] {0} : {1}", data.Name, data.DefaultValue.ToString());
                            }
                            else
                            {
                                logger.Error("[InitializeData] {0} : cannot write to device, value={1}", data.Name, data.DefaultValue.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "InitializeData Error");
                }
            }, cancellationToken);
        }

        public async Task PollingStart(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    foreach (var data in dataVMList.DataList)
                    {
                        if (data.Use == false) continue;
                        if (data.PollingTime == 0) continue;
                        if (data.Direction != Direction.IN) continue;

                        DateTime lastUpdateTime;

                        if (!DateTime.TryParseExact(data.UpdateTime, "yyyy-MM-dd-HH:mm:ss.ff", null, System.Globalization.DateTimeStyles.None, out lastUpdateTime))
                        {
                            data.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ff");
                            continue;
                        }
                        else
                        {
                            TimeSpan timeSpan = DateTime.Now - lastUpdateTime;

                            if (timeSpan.TotalMilliseconds >= data.PollingTime && GET_DATA(data.Name, out object value))
                            {
                                data.Value = value ?? new object();
                                data.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ff");
                            }
                        }
                    }

                    await Task.Delay(10, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.Debug(ex, "Polling Thread End.");
            }
        }

        public DevMode IsDeviceMode(string deviceName)
        {
            return deviceManager?.IsDeviceMode(deviceName) ?? DevMode.DISCONNECT;
        }

        public bool SET_DATA(string name, object value)
        {
            var dataVM = dataVMList.DataList.FirstOrDefault(item => item.Name == name);

            if (dataVM == null)
            {
                logger.Error("[SET_DATA] {0} : DataVM is null, may be your data name is wrong.", name);
                return false;
            }

            bool result = dataVM.DeviceName.StartsWith("V") ||
                          (deviceManager.SetDataToDevice(name, value) && dataVM.Direction == Direction.OUT);

            if (result)
            {
                logger.Debug("[SET_DATA] {0} : {1}", name, value.ToString());
                dataVM.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ff");

                if (value.GetType() == typeof(bool))
                {
                    value = (bool)value ? 1 : 0;
                }

                if (result && dataVM.Value != null && dataVM.Value.GetType() == value.GetType() && !Object.Equals(dataVM.Value, value))
                {
                    dataVM.Value = value;
                    DataChangedEvent?.Invoke(this, dataVM);
                }
            }

            return result;
        }

        public bool GET_BOOL(string name, out bool result)
        {
            result = GET_DATA(name, out object value);

            if (result && value is int intValue)
            {
                return intValue > 0 ? true : false;
            }
            else
            {
                result = false;
                return false;
            }
        }


        public int GET_INT(string name, out bool result)
        {
            result = GET_DATA(name, out object value);

            if (result && value is int intValue)
            {
                return intValue;
            }
            else
            {
                result = false;
                return 0;
            }
        }

        public double GET_DOUBLE(string name, out bool result)
        {
            result = GET_DATA(name, out object value);

            if (result && value is double dValue)
            {
                return dValue;
            }
            else
            {
                result = false;
                return 0.0;
            }
        }

        public string GET_STRING(string name, out bool result)
        {
            result = GET_DATA(name, out object value);

            if (result && value is string strValue)
            {
                return strValue;
            }
            else
            {
                result = false;
                return "";
            }
        }

        public object GET_OBJECT(string name, out bool result)
        {
            result = GET_DATA(name, out object value);

            if (result)
            {
                return value;
            }
            else
            {
                result = false;
                return null;
            }
        }

        public bool GET_DATA(string name, out object value)
        {
            bool result = false;
            value = null;
            var dataVM = dataVMList.DataList.SingleOrDefault((item) => (item.Name == name));

            

            if (dataVM == null)
            {
                logger.Error("[GET_DATA] {0} : Data is null, may be your data name is wrong.", name);

                return result;
            }

            value = dataVM.Value;

            result = dataVM.DeviceName.StartsWith("V") || deviceManager.GetDataFromDevice(name, out value);

            object objValue = value;

            if (result && dataVM.Value != null && dataVM.Value.GetType() == objValue.GetType() && !Object.Equals(dataVM.Value, objValue))
            {
                dataVM.Value = objValue;
                dataVM.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff");
                DataChangedEvent?.Invoke(this, dataVM);
            }

            return result;
        }

        public bool SET_DATA(string name, object value, bool setDefaultValue)
        {
            bool result = false;

            var dataVM = dataVMList.DataList.SingleOrDefault((item) => (item.Name == name));
            
            if (dataVM == null)
            {
                logger.Error("[SET_DATA] {0} : DataVM is null, may be your data name is wrong.", name);
                return false;
            }
            else if (dataVM.DeviceName.StartsWith("V"))
            {
                logger.Debug("[SET_DATA] {0} : {1}", name, value.ToString());
                dataVM.Value = value;
                if (setDefaultValue) dataVM.DefaultValue = value.ToString();
                dataVM.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff");
                dataVM.SaveChanges();
                DataChangedEvent?.Invoke(this, dataVM);
                result = true;
            }
            else if (deviceManager.SetDataToDevice(name, value) && dataVM.Direction == Direction.OUT)
            {
                logger.Debug("[SET_DATA] {0} : {1}", name, value.ToString());
                dataVM.Value = value;
                if (setDefaultValue) dataVM.DefaultValue = value.ToString();
                dataVM.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff");
                dataVM.SaveChanges();
                DataChangedEvent?.Invoke(this, dataVM);
                result = true;
            }
            else
            {
                logger.Error("[SET_DATA] {0} : cannot write to device, value={1}", name, value.ToString());
                result = false;
            }

            return result;
        }

        public void SetDialogService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public TaskResponseType ShowMessageBox(string title, string message, ShowDialogOptions showDialogOptions = ShowDialogOptions.YesNo)
        {
            if (dialogService == null)
            {
                throw new ArgumentNullException("dialogService가 null입니다. 유효한 IDialogService 인스턴스를 전달해야 합니다.");
            }
            return dialogService.ShowDialog(title, message, showDialogOptions);

        }
    }
}
