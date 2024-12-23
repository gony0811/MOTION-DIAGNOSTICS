using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE.Data.Entity
{
    public class DeviceConfigEntity
    {
        /// <summary>
        /// 장치 이름 (ex. "ACS", "PMAC", "PLC", "CNC", "ROBOT" 등)
        /// </summary>
        public string DeviceName { get; set; } = null!;

        /// <summary>
        /// 장치 타입 (ex. "MOTION CONTROLLER", "I/O DEVICE" 등)
        /// </summary>
        public string DeviceType { get; set; } = null!;

        /// <summary>
        /// 장치 DLL Instance Namespace (ex. DEV.ACS, DEV.PMAC, DEV.PLC, DEV.CNC, DEV.ROBOT 등)
        /// </summary>
        public string InstanceName { get; set; } = null!;

        /// <summary>
        /// 장치 DLL 파일/경로 (ex. DEV.ACS.DLL, DEV.PMAC.DLL, DEV.PLC.DLL, DEV.CNC.DLL, DEV.ROBOT.DLL 등)
        /// </summary>
        public string FileName { get; set; } = null!;

        /// <summary>
        /// 장치 사용/미사용
        /// </summary>
        public bool Use { get; set; } = false;
        /// <summary>
        /// 장치 인스턴스 생성 시 필요한 인자값 (ex. IP, PORT 등)
        /// </summary>
        public string Args { get; set; } = null!;

        /// <summary>
        /// 장치 설명
        /// </summary>
        public string? Description { get; set; }
    }
}
