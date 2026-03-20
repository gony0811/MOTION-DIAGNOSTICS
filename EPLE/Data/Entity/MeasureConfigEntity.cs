using EPLE.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE.Data.Entity
{
    public class MeasureConfigEntity
    {
        public MeasureConfigEntity() { }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int XPos { get; set; }
        public virtual int YPos { get; set; }
        public virtual bool IsMeasurePoint { get; set; }
        public virtual double ErrorX { get; set; }
        public virtual double ErrorY { get; set; }

        public virtual MeasureResult MeasureResult { get; set; }

        public virtual string MeasureResultString
        {
            get => MeasureResult.ToString();
            set => MeasureResult = (MeasureResult)Enum.Parse(typeof(MeasureResult), value);
        }

        public virtual string UpdateTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is MeasureConfigEntity other)
            {
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
