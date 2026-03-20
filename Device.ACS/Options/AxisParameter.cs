using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.Options
{
    public class AxisParameter
    {
        public string Name { get; set; }
        /// <summary>
        /// 홈 시작 PLC, BUFFER 프로그램 번호
        /// </summary>
        public int HomeBufferNumber { get; set; }
        /// <summary>
        /// 홈 속도
        /// </summary>
        public double HomeSearchVelocity { get; set; }
        /// <summary>
        /// 홈 옵셋
        /// </summary>
        public double HomeOffset { get; set; }
        /// <summary>
        /// 홈 타임아웃
        /// </summary>
        public int HomeTimeout { get; set; }
        /// <summary>
        /// 홈 완료 체크 번호
        /// </summary>
        public int HomeCompleteCheckNumber { get; set; }
        /// <summary>
        /// 조그 속도
        /// </summary>
        public double JogVelocity { get; set; }
        /// <summary>
        /// 조그 가속도
        /// </summary>
        public double JogAcceleration { get; set; }
        /// <summary>
        /// 조그 감속도
        /// </summary>
        public double JogDeceleration { get; set; }
        /// <summary>
        /// 메인트 속도
        /// </summary>
        public double MaintVelocity { get; set; }
        /// <summary>
        /// 메인트 가속도
        /// </summary>
        public double MaintAcceleration { get; set; }
        /// <summary>
        /// 메인트 감속도
        /// </summary>
        public double MaintDeceleration { get; set; }
        /// <summary>
        /// 런 속도
        /// </summary>
        public double RunVelocity { get; set; }
        /// <summary>
        /// 런 가속도
        /// </summary>
        public double RunAcceleration { get; set; }
        /// <summary>
        /// 런 감속도
        /// </summary>
        public double RunDeceleration { get; set; }
        /// <summary>
        /// Unit에 해당하는 단위를 맞추기 위한 값 (1mm 당 이동 값1000, 1도 당 이동 값)
        /// </summary>
        public double UnitFactor { get; set; }
    }
}
