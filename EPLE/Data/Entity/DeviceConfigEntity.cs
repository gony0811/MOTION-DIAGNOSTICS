using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE.Data.Entity
{
    public class DeviceConfigEntity
    {
        public DeviceConfigEntity() { }
        public virtual int Id { get; set; }
        /// <summary>
        /// 장치 이름 (ex. "ACS", "PMAC", "PLC", "CNC", "ROBOT" 등)
        /// </summary>
        public virtual string DeviceName { get; set; }

        /// <summary>
        /// 장치 타입 (ex. "MOTION CONTROLLER", "I/O DEVICE" 등)
        /// </summary>
        public virtual string DeviceType { get; set; }

        /// <summary>
        /// 장치 DLL Instance Namespace (ex. DEV.ACS, DEV.PMAC, DEV.PLC, DEV.CNC, DEV.ROBOT 등)
        /// </summary>
        public virtual string InstanceName { get; set; }

        /// <summary>
        /// 장치 DLL 파일/경로 (ex. DEV.ACS.DLL, DEV.PMAC.DLL, DEV.PLC.DLL, DEV.CNC.DLL, DEV.ROBOT.DLL 등)
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// 장치 사용/미사용
        /// </summary>
        public virtual bool IsUse { get; set; } = false;
        /// <summary>
        /// 장치 인스턴스 생성 시 필요한 인자값 (ex. IP, PORT 등)
        /// </summary>
        public virtual string Args { get; set; }

        /// <summary>
        /// 장치 설명
        /// </summary>
        public virtual string Description { get; set; }

        //public virtual (bool IsValid, List<string> Errors) Validate()
        //{
        //    var errors = new List<string>();

        //    if (string.IsNullOrWhiteSpace(DeviceName))
        //        errors.Add("DeviceName은 필수 입력 항목입니다.");

        //    if (string.IsNullOrWhiteSpace(DeviceType))
        //        errors.Add("DeviceType은 필수 입력 항목입니다.");

        //    if (string.IsNullOrWhiteSpace(InstanceName))
        //        errors.Add("InstanceName은 필수 입력 항목입니다.");

        //    if (string.IsNullOrWhiteSpace(FileName))
        //        errors.Add("FileName은 필수 입력 항목입니다.");

        //    if (string.IsNullOrWhiteSpace(Args))
        //        errors.Add("Args는 필수 입력 항목입니다.");

        //    return (errors.Count == 0, errors);
        //}

        public override bool Equals(object obj)
        {
            if (obj is DeviceConfigEntity other)
            {
                return Id == other.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public virtual (bool IsValid, List<string> Errors) Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(DeviceName))
                errors.Add("DeviceName은 필수 입력 항목입니다.");

            if (string.IsNullOrWhiteSpace(DeviceType))
                errors.Add("DeviceType은 필수 입력 항목입니다.");

            if (string.IsNullOrWhiteSpace(InstanceName))
                errors.Add("InstanceName은 필수 입력 항목입니다.");

            if (string.IsNullOrWhiteSpace(FileName))
                errors.Add("FileName은 필수 입력 항목입니다.");

            if (string.IsNullOrWhiteSpace(Args))
                errors.Add("Args는 필수 입력 항목입니다.");

            return (errors.Count == 0, errors);
        }
    }
}
