using MDS.UI.Model;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MDS.UI.UserControls
{
    /// <summary>
    /// Interaction logic for HorizontalSettingControl.xaml
    /// </summary>
    public partial class WaferAlignControl : UserControl
    {
        // DependencyProperty 선언
        public static readonly DependencyProperty HorizontalSettingProperty =
            DependencyProperty.Register(
                nameof(HorizontalSetting),
                typeof(HorizontalSetting),
                typeof(WaferAlignControl),
                new PropertyMetadata(null, OnHorizontalSettingChanged));

        // HorizontalSetting 속성
        public HorizontalSetting HorizontalSetting
        {
            get => (HorizontalSetting)GetValue(HorizontalSettingProperty);
            set => SetValue(HorizontalSettingProperty, value);
        }

        // DependencyProperty 변경 시 호출되는 콜백 메서드
        private static void OnHorizontalSettingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WaferAlignControl control)
            {
                var newValue = e.NewValue as HorizontalSetting;
                var oldValue = e.OldValue as HorizontalSetting;

                // 변경된 값에 대해 추가 작업 수행
                control.OnHorizontalSettingUpdated(newValue, oldValue);
            }
        }

        // HorizontalSetting 변경에 따른 추가 작업을 수행하는 메서드
        private void OnHorizontalSettingUpdated(HorizontalSetting newValue, HorizontalSetting oldValue)
        {
            // 필요한 로직을 추가할 수 있음
            // 예: UI 갱신
            if (newValue != null)
            {
                // 새 값을 기반으로 작업
            }
        }

        // 생성자
        public WaferAlignControl()
        {
            InitializeComponent();
            HorizontalSetting = HorizontalSetting.GetInstance();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 허용된 문자: 숫자, ., +, -
            Regex regex = new Regex("[^0-9.+-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
