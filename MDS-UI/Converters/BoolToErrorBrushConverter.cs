using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MDS.UI.Converters
{
    public class BoolToErrorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked)
            {
                return new SolidColorBrush(Colors.Red);
            }

            return new SolidColorBrush(Colors.DarkRed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
