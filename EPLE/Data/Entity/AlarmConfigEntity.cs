using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE.Data.Entity
{
    public class AlarmConfigEntity
    {
        public AlarmConfigEntity() { }
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        public virtual ALCD Level { get; set; }

        public virtual string LevelString
        {
            get => Level.ToString();
            set => Level = (ALCD)Enum.Parse(typeof(ALCD), value);
        }

        public virtual string Text { get; set; }
        public virtual ALST Status { get; set; }

        public virtual string StatusString
        {
            get => Status.ToString();
            set => Status = (ALST)Enum.Parse(typeof(ALST), value);
        }
        public virtual ALED Enable { get; set; }

        public virtual string EnableString
        {
            get => Enable.ToString();
            set => Enable = (ALED)Enum.Parse(typeof(ALED), value);
        }

        public virtual string Description { get; set; }

        public virtual string SetTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is AlarmConfigEntity other)
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
