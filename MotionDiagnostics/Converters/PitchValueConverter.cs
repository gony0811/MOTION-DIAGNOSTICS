using System;
using System.Globalization;
using System.Windows.Data;

namespace MotionDiagnostics.Converters
{
    public class PitchValueConverter : IValueConverter
    {
        // 똑같이 "CustomText"만 특별 취급( TextBox 용 )하고,
        // 나머지는 전부 '파싱 시도 후' RadioButton 값으로 처리
        // "Custom" 라디오버튼 체크는 "KnownValues" 같은 하드코딩 없이
        //  => "파라미터가 double로 파싱 안될 때" 또는 "TextBox 입력" 등으로 구분 가능

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            if (!(value is double d)) d = 0.0;

            string param = parameter.ToString();

            // TextBox 바인딩( CustomText ) 처리
            if (param == "CustomText")
            {
                // pitchValue -> string
                return d.ToString("G", CultureInfo.InvariantCulture);
            }

            // 그 외: 라디오버튼
            // 1) param이 double인지 판단
            if (double.TryParse(param, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedParam))
            {
                // => pitchValue와 같으면 IsChecked = true
                return Math.Abs(d - parsedParam) < 0.000001;
            }
            else if (param == "Custom")
            {
                // 파라미터가 "Custom"인 경우: KnownValues 개념 없이
                //   "param이 숫자로 파싱 안 됐으므로" Custom 라디오버튼 = pitchValue가
                //   다른 라디오버튼 값과 달라야만 true
                // 그냥 '디폴트'로 true/false 판단할 수도 있고, 
                // 또는 "항상 true" 처리 후 TextBox에 맡기는 방식도 있음.
                return true; // 임의
            }

            // 그 외는 false
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return Binding.DoNothing;

            string param = parameter.ToString();

            // TextBox( CustomText )
            if (param == "CustomText")
            {
                // TextBox 문자를 double로 파싱
                if (value is string text &&
                    double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsed))
                {
                    return parsed;
                }
                return Binding.DoNothing;
            }

            // 그 외 RadioButton
            if (value is bool isChecked && isChecked)
            {
                // numeric param
                if (double.TryParse(param, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedParam))
                {
                    return parsedParam;
                }
                else if (param == "Custom")
                {
                    // Custom 라디오버튼이 체크되면
                    // Pitch를 0.0 등으로 초기화, 
                    //  혹은 "유지"하려면 Binding.DoNothing
                    //return 0.0;
                    return Binding.DoNothing;
                }
            }

            return Binding.DoNothing;
        }
    }
}
