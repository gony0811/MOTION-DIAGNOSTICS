
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;

namespace MDS.UI.UserControls
{
    /// <summary>
    /// Interaction logic for JogMoveControl.xaml
    /// </summary>
    public partial class JogMoveControl : UserControl
    {
        public static readonly DependencyProperty InputXYJogVelocityProperty = DependencyProperty.Register(nameof(XYJogInputVelocity), typeof(double), typeof(MotionControl), new PropertyMetadata(0.0));

        public JogMoveControl()
        {
            InitializeComponent();
        }

        public double XYJogInputVelocity
        {
            get { return (double)GetValue(InputXYJogVelocityProperty); }
            set { SetValue(InputXYJogVelocityProperty, value); }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MotionControl;
            if (control != null)
            {
                //if (e.Property == IsInputVelocityCheckedProperty)
                //{
                //    bool isChecked = (bool)e.NewValue;
                //    control.IsInputVelocityChecked = isChecked;

                //    if (isChecked)
                //    {
                //        //control.TextBox_UserInputVelocity.Text = string.Format("{0:F3}", restoreInputVelocity);
                //        //control.InputVelocity = restoreInputVelocity;
                //    }
                //}

            }
        }


        private void Button_JogRight_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogRight_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogLeft_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogLeft_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogUp_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogUp_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogDown_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogDown_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogZDown_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogZDown_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogZUp_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_JogZUp_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }


        private void Button_JogSettings_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
