using System;
using System.Globalization;
using System.Windows.Data;

namespace MotionDiagnostics.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue.ToString("F3", culture); // "F3"은 소수점 이하 3자리까지 포맷팅
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && double.TryParse(stringValue, NumberStyles.Any, culture, out double result))
            {
                return result;
            }
            return value;
        }
    }
}
