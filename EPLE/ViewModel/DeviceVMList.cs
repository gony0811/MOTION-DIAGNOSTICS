using EPLE.Data.Entity;
using EPLE.Data.Interface;
using System.Collections.ObjectModel;

namespace EPLE.ViewModel
{
    public class DeviceVMList
    {
        public class DeviceVM : ViewModel
        {
            private readonly DeviceConfigEntity deviceConfigEntity;
            private readonly ISaveChanges saveChanges;
            internal DeviceVM(DeviceConfigEntity deviceConfigEntity, ISaveChanges saveChanges)
            {
                this.deviceConfigEntity = deviceConfigEntity;
                this.saveChanges = saveChanges;
            }
            public string DeviceName { get => deviceConfigEntity.DeviceName; set => deviceConfigEntity.DeviceName = value; }
            public string DeviceType { get => deviceConfigEntity.DeviceType; set => deviceConfigEntity.DeviceType = value; }
            public string InstanceName { get => deviceConfigEntity.InstanceName; set => deviceConfigEntity.InstanceName = value; }
            public string FileName { get => deviceConfigEntity.FileName; set => deviceConfigEntity.FileName = value; }
            public bool Use { get => deviceConfigEntity.Use; set => deviceConfigEntity.Use = value; }
            public string Args { get => deviceConfigEntity.Args; set => deviceConfigEntity.Args = value; }
            public string Description { get => deviceConfigEntity.Description ?? ""; set => deviceConfigEntity.Description = value; }

            private bool? isConnected = null;

            public void SaveChanges() => saveChanges.SaveChanges();
        }

        public ObservableCollection<DeviceVM> Devices { get; } = new();
    }
}
