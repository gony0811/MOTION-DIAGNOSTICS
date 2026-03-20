using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.Options
{
    public class RootObject  // 최상위 JSON 키를 반영하는 클래스 추가
    {
        public SettingOptions SettingOptions { get; set; }
    }

    public class SettingOptions
    {
        public int AxisCount { get; set; }
        public string Communication { get; set; }
        public bool Simulation { get; set; }
        public List<AxisParameter> AxisParameters { get; set; }  // 배열을 List로 변경 (더 유연하게 활용 가능)
    }
}
