
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EPLE.Data.Entity
{
    public class DataConfigEntity
    {
        /// <summary>
        /// 데이터 이름 (ex. "MOTION.HOME", "MOTION.JOG.PLUS", "MOTION.JOG.MINUS" 등)
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 데이터 모듈 (ex. "MOTION", "I/O", "CAMERA" 등)
        /// </summary>
        public string? Module { get; set; }

        /// <summary>
        /// 데이터 그룹 (ex. "MOTION", "I/O", "CAMERA" 등)
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// 데이터 타입 (ex. INT, DOUBLE, STRING, OBJECT 등)
        /// </summary>
        public DataType Type { get; set; }

        /// <summary>
        //  DEVICE DRIVER 이름 (ex. "ACS", "PMAC", "PLC", "CNC", "ROBOT" 등)
        /// </summary>
        public string DeviceName { get; set; } = null!;

        /// <summary>
        /// DATA DIRECTION (ex. INPUT, OUTPUT, BOTH)
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Device에 데이터를 요청할 때 사용할 Command
        /// </summary>
        public string Command { get; set; } = null!;

        /// <summary>
        /// Device에 데이터를 주기적으로 요청할 때 사용할 Polling Time
        /// </summary>
        public int PollingTime { get; set; }

        /// <summary>
        /// Device에 데이터를 쓰기 요청하고 난 뒤 자동으로 Default로 되돌리고자 할 때 사용할 시간
        /// </summary>
        public int DataResetTimeout { get; set; }

        /// <summary>
        /// Data의 사용 여부
        /// </summary>
        public bool Use { get; set; }
        
        /// <summary>
        /// Data에 대한 설명
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Data의 Default 값
        /// </summary>
        public string? DefaultValue { get; set; }

        public string? UpdateTime { get; set; }
    }
}