using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MotionDiagnostics.Converters
{
    public class MultiBindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0]; // 첫 번째 값을 반환하여 TextBox.Text에 설정
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
           return new object[] { value, value }; // TextBox.Text 값을 두 개의 바인딩 소스에 설정
        }
    }
}
