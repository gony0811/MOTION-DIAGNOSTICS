using System.Windows;
using System.Windows.Controls;


namespace MotionDiagnostics.UserControls
{
    /// <summary>
    /// PneumaticOptionUserControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PneumaticOptionControl : UserControl
    {

        public static readonly DependencyProperty IsPneumaticOptionCheckedProperty = DependencyProperty.Register(nameof(IsPneumaticOptionChecked), typeof(bool), typeof(PneumaticOptionControl), new PropertyMetadata(false));

        public static readonly DependencyProperty IsXGuideSolOptionCheckedProperty = DependencyProperty.Register(nameof(IsXGuideSolOptionChecked), typeof(bool), typeof(PneumaticOptionControl), new PropertyMetadata(false));

        public static readonly DependencyProperty IsXSupportSolOptionCheckedProperty = DependencyProperty.Register(nameof(IsXSupportSolOptionChecked), typeof(bool), typeof(PneumaticOptionControl), new PropertyMetadata(false));

        public static readonly DependencyProperty IsY1GuideSolOptionCheckedProperty = DependencyProperty.Register(nameof(IsY1GuideSolOptionChecked), typeof(bool), typeof(PneumaticOptionControl), new PropertyMetadata(false));

        public static readonly DependencyProperty IsY1SupportSolOptionCheckedProperty = DependencyProperty.Register(nameof(IsY1SupportSolOptionChecked), typeof(bool), typeof(PneumaticOptionControl), new PropertyMetadata(false));

        public static readonly DependencyProperty IsY2GuideSolOptionCheckedProperty = DependencyProperty.Register(nameof(IsY2GuideSolOptionChecked), typeof(bool), typeof(PneumaticOptionControl), new PropertyMetadata(false));


        public static readonly DependencyProperty IsY2SupportSolOptionCheckedProperty = DependencyProperty.Register(nameof(IsY2SupportSolOptionChecked), typeof(bool), typeof(PneumaticOptionControl), new PropertyMetadata(false));

        public bool IsPneumaticOptionChecked
        {
            get => (bool)GetValue(IsPneumaticOptionCheckedProperty);
            set => SetValue(IsPneumaticOptionCheckedProperty, value);
        }

        public bool IsXGuideSolOptionChecked
        {
            get => (bool)GetValue(IsXGuideSolOptionCheckedProperty);
            set => SetValue(IsXGuideSolOptionCheckedProperty, value);
        }

        public bool IsXSupportSolOptionChecked
        {
            get => (bool)GetValue(IsXSupportSolOptionCheckedProperty);
            set => SetValue(IsXSupportSolOptionCheckedProperty, value);
        }   

        public bool IsY1GuideSolOptionChecked
        {
            get => (bool)GetValue(IsY1GuideSolOptionCheckedProperty);
            set => SetValue(IsY1GuideSolOptionCheckedProperty, value);
        }

        public bool IsY1SupportSolOptionChecked
        {
            get => (bool)GetValue(IsY1SupportSolOptionCheckedProperty);
            set => SetValue(IsY1SupportSolOptionCheckedProperty, value);
        }

        public bool IsY2GuideSolOptionChecked
        {
            get => (bool)GetValue(IsY2GuideSolOptionCheckedProperty);
            set => SetValue(IsY2GuideSolOptionCheckedProperty, value);
        }

        public bool IsY2SupportSolOptionChecked
        {
            get => (bool)GetValue(IsY2SupportSolOptionCheckedProperty);
            set => SetValue(IsY2SupportSolOptionCheckedProperty, value);
        }

        public PneumaticOptionControl()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is CheckBox checkBox)
            {
                switch(checkBox.Name)
                {
                    case "CheckBox_UsePneumeticOption":
                        IsPneumaticOptionChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
                        break;
                    case "CheckBox_XGuideSolOption":
                        IsXGuideSolOptionChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
                        break;
                    case "CheckBox_XSupportSolOption":
                        IsXSupportSolOptionChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
                        break;
                    case "CheckBox_Y1GuideSolOption":
                        IsY1GuideSolOptionChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
                        break;
                    case "CheckBox_Y1SupportSolOption":
                        IsY1SupportSolOptionChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
                        break;
                    case "CheckBox_Y2GuideSolOption":
                        IsY2GuideSolOptionChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
                        break;
                    case "CheckBox_Y2SupportSolOption":
                        IsY2SupportSolOptionChecked = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value;
                        break;
                }
            }
        }
    }
}
