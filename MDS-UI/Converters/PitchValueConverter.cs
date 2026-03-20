using System;
using System.Globalization;
using System.Windows.Data;

namespace MDS.UI.Converters
{
    public class PitchValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            if (!(value is double d)) d = 0.0;

            string param = parameter.ToString();

            if (param == "CustomText")
            {
                return d.ToString("G", CultureInfo.InvariantCulture);
            }

            if (double.TryParse(param, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedParam))
            {
                return Math.Abs(d - parsedParam) < 0.000001;
            }
            else if (param == "Custom")
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return Binding.DoNothing;

            string param = parameter.ToString();

            if (param == "CustomText")
            {
                if (value is string text &&
                    double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsed))
                {
                    return parsed;
                }
                return Binding.DoNothing;
            }

            if (value is bool isChecked && isChecked)
            {
                if (double.TryParse(param, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedParam))
                {
                    return parsedParam;
                }
                else if (param == "Custom")
                {
                    return Binding.DoNothing;
                }
            }

            return Binding.DoNothing;
        }
    }
}
