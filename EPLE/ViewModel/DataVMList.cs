using EPLE.Data;
using EPLE.Data.Entity;
using EPLE.Data.Interface;
using System.Collections.ObjectModel;

namespace EPLE.ViewModel
{
    public class DataVMList
    {
        public class DataVM : ViewModel
        {
            private readonly DataConfigEntity dataConfigEntity;
            private readonly ISaveChanges saveChanges;

            internal DataVM(DataConfigEntity dataConfigEntity, ISaveChanges saveChanges)
            {
                this.dataConfigEntity = dataConfigEntity;
                this.saveChanges = saveChanges;

                switch (this.Type)
                {
                    case DataType.INT:
                        Value = int.Parse(dataConfigEntity.DefaultValue ?? "0");
                        break;
                    case DataType.DOUBLE:
                        Value = int.Parse(dataConfigEntity.DefaultValue ?? "0.0");
                        break;
                    case DataType.STRING:
                        Value = float.Parse(dataConfigEntity.DefaultValue ?? "");
                        break;
                    case DataType.OBJECT:
                        Value = dataConfigEntity.DefaultValue ?? "";
                        break;
                    default:
                        Value = 0;
                        break;
                }
            }

            public string Name { get => dataConfigEntity.Name; set => dataConfigEntity.Name = value; }
            public string Module { get => dataConfigEntity.Module ?? "UNKNOWN"; set => dataConfigEntity.Module = value; }
            public string Group { get => dataConfigEntity.Group ?? "UNKNOWN"; set => dataConfigEntity.Group = value; }
            public string Description { get => dataConfigEntity.Description ?? ""; set => dataConfigEntity.Description = value; }
            public DataType Type { get => dataConfigEntity.Type; set => dataConfigEntity.Type = value; }
            public string DeviceName { get => dataConfigEntity.DeviceName; set => dataConfigEntity.DeviceName = value; }

            public Direction Direction { get => dataConfigEntity.Direction; set => dataConfigEntity.Direction = value; }

            public string Command { get => dataConfigEntity.Command; set => dataConfigEntity.Command = value; }

            public int PollingTime { get => dataConfigEntity.PollingTime; set => dataConfigEntity.PollingTime = value; }

            public int DataResetTimeout { get => dataConfigEntity.DataResetTimeout; set => dataConfigEntity.DataResetTimeout = value; }

            public bool Use { get => dataConfigEntity.Use; set => dataConfigEntity.Use = value; }

            public string DefaultValue { get => dataConfigEntity.DefaultValue ?? ""; set => dataConfigEntity.DefaultValue = value; }
            public string UpdateTime { get => dataConfigEntity.UpdateTime?? DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss.ffff"); set => dataConfigEntity.UpdateTime = value; }
            public void SaveChanges() => saveChanges.SaveChanges();

            public object Value { get; set; }
        }

        public ObservableCollection<DataVM> DataList { get; } = new ObservableCollection<DataVM>();
    }

}
