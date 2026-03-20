using EPLE.Data;
using EPLE.Data.Entity;
using EPLE.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPLE.ViewModel.DeviceVMList;

namespace EPLE.ViewModel
{
    public class MeasureVMList
    {
        public class MeasureVM : ViewModel
        {
            private readonly MeasureConfigEntity measureConfigEntity;
            private readonly ISaveChanges saveChanges;

            public MeasureVM(MeasureConfigEntity measureConfigEntity)
            {
                this.measureConfigEntity = measureConfigEntity;
            }

            public MeasureVM(MeasureConfigEntity measureConfigEntity, ISaveChanges saveChanges)
            {
                this.measureConfigEntity = measureConfigEntity;
                this.saveChanges = saveChanges;
            }

            public int Id { get => measureConfigEntity.Id; set => measureConfigEntity.Id = value; }
            public string Name { get => measureConfigEntity.Name; set => measureConfigEntity.Name = value; }
            public int XPos { get => measureConfigEntity.XPos; set => measureConfigEntity.XPos = value; }
            public int YPos { get => measureConfigEntity.YPos; set => measureConfigEntity.YPos = value; }
            public bool IsMeasurePoint { get => measureConfigEntity.IsMeasurePoint; set => measureConfigEntity.IsMeasurePoint = value; }
            public double ErrorX { get => measureConfigEntity.ErrorX; set => measureConfigEntity.ErrorX = value; }
            public double ErrorY { get => measureConfigEntity.ErrorY; set => measureConfigEntity.ErrorY = value; }
            public MeasureResult MeasureResult { get => measureConfigEntity.MeasureResult; set => measureConfigEntity.MeasureResult = value; }
            public string UpdateTime { get => measureConfigEntity.UpdateTime; set => measureConfigEntity.UpdateTime = value; }
            public void SaveChanges() => saveChanges.SaveChanges();

            public MeasureVM Clone()
            {
                return new MeasureVM(new MeasureConfigEntity
                {
                    Id = Id,
                    Name = Name,
                    XPos = XPos,
                    YPos = YPos,
                    IsMeasurePoint = IsMeasurePoint,
                    ErrorX = ErrorX,
                    ErrorY = ErrorY,
                    MeasureResult = MeasureResult,
                    UpdateTime = UpdateTime
                });
            }
        }

        public ObservableCollection<MeasureVM> Measures { get; } = new ObservableCollection<MeasureVM>();
    }
}
