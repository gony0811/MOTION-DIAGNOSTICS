using EPLE.Data;
using EPLE.Data.Entity;
using EPLE.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPLE.ViewModel.DataVMList;

namespace EPLE.ViewModel
{
    public class AlarmVMList
    {
        public class AlarmVM : ViewModel
        {
            private readonly AlarmConfigEntity alarmConfigEntity;
            private readonly ISaveChanges saveChanges;

            internal AlarmVM(AlarmConfigEntity alarmConfigEntity, ISaveChanges saveChanges)
            {
                this.alarmConfigEntity = alarmConfigEntity;
                this.saveChanges = saveChanges;
            }

            public int Id { get => alarmConfigEntity.Id; set => alarmConfigEntity.Id = value; }
            public string Name { get => alarmConfigEntity.Name; set => alarmConfigEntity.Name = value; }
            public ALCD Level { get => alarmConfigEntity.Level; set => alarmConfigEntity.Level = value; }

            public string Text { get => alarmConfigEntity.Text; set => alarmConfigEntity.Text = value; }

            public ALST Status { get => alarmConfigEntity.Status; set => alarmConfigEntity.Status = value; }

            public ALED Enable { get => alarmConfigEntity.Enable; set => alarmConfigEntity.Enable = value; }

            public string Description { get => alarmConfigEntity.Description; set => alarmConfigEntity.Description = value; }

            public void SaveChanges() => saveChanges.SaveChanges();
        }

        public ObservableCollection<AlarmVM> AlarmList { get; } = new ObservableCollection<AlarmVM>();
    }
}
