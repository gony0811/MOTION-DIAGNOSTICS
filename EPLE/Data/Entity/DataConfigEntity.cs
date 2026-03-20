
using NHibernate.Type;
using System;

namespace EPLE.Data.Entity
{

    public class DataConfigEntity
    {
        public DataConfigEntity() { }
        public virtual int Id { get; set; }
        /// <summary>
        /// 데이터 이름 (ex. "MOTION.HOME", "MOTION.JOG.PLUS", "MOTION.JOG.MINUS" 등)
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 데이터 모듈 (ex. "MOTION", "I/O", "CAMERA" 등)
        /// </summary>
        public virtual string Module { get; set; }

        /// <summary>
        /// 데이터 그룹 (ex. "MOTION", "I/O", "CAMERA" 등)
        /// </summary>
        //public virtual string Group { get; set; }

        /// <summary>
        /// 데이터 타입 (ex. INT, DOUBLE, STRING, OBJECT 등)
        /// </summary>
        public virtual DataType Type { get; set; }

        public virtual string TypeString
        {
            get => Type.ToString();
            set => Type = (DataType)Enum.Parse(typeof(DataType), value);
        }

        /// <summary>
        //  DEVICE DRIVER 이름 (ex. "ACS", "PMAC", "PLC", "CNC", "ROBOT" 등)
        /// </summary>
        public virtual string DeviceName { get; set; }

        /// <summary>
        /// DATA DIRECTION (ex. INPUT, OUTPUT, BOTH)
        /// </summary>
        public virtual Direction Direction { get; set; }

        public virtual string DirectionString
        {
            get => Direction.ToString();
            set => Direction = (Direction)Enum.Parse(typeof(Direction), value);
        }

        /// <summary>
        /// Device에 데이터를 요청할 때 사용할 Command
        /// </summary>
        public virtual string Command { get; set; }

        /// <summary>
        /// Device에 데이터를 주기적으로 요청할 때 사용할 Polling Time
        /// </summary>
        public virtual int? PollingTime { get; set; }

        /// <summary>
        /// Device에 데이터를 쓰기 요청하고 난 뒤 자동으로 Default로 되돌리고자 할 때 사용할 시간
        /// </summary>
        public virtual int? DataResetTimeout { get; set; }

        /// <summary>
        /// Data의 사용 여부
        /// </summary>
        public virtual bool IsUse { get; set; }

        /// <summary>
        /// Data에 대한 설명
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Data의 Default 값
        /// </summary>
        public virtual string DefaultValue { get; set; }

        public virtual string UpdateTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DataConfigEntity other)
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