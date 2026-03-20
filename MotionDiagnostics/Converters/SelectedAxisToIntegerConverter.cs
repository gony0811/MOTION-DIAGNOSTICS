using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MotionDiagnostics.Converters
{
    public class SelectedAxisToIntegerConverter : IValueConverter
    {
        private static readonly string[] AxisNames = { "X", "Y", "T", "Z1", "Z2", "Z3" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string stringValue ? GetAxisIndex(stringValue) : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int intValue && intValue >= 0 && intValue < AxisNames.Length ? AxisNames[intValue] : string.Empty;
        }

        private int GetAxisIndex(string axisName)
        {
            for (int i = 0; i < AxisNames.Length; i++)
            {
                if (AxisNames[i] == axisName)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
