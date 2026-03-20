using MotionDiagnostics.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MotionDiagnostics.UserControls
{
    /// <summary>
    /// MotionControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MotionControl : UserControl
    {
        public static readonly DependencyProperty MotionAxisChangedProperty = DependencyProperty.Register(nameof(MotionAxisChangedCommand), typeof(DelegateCommand<SelectionChangedEventArgs>), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty MotionAxisListProperty = DependencyProperty.Register(nameof(MotionAxisList), typeof(List<string>), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty IsPlusLimitCheckedProperty = DependencyProperty.Register(nameof(IsPlusLimitChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public static readonly DependencyProperty IsMinusLimitCheckedProperty = DependencyProperty.Register(nameof(IsMinusLimitChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty IsEnableCheckedProperty = DependencyProperty.Register(nameof(IsEnableChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public static readonly DependencyProperty IsDisableCheckedProperty = DependencyProperty.Register(nameof(IsDisableChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty IsInPositionCheckedProperty = DependencyProperty.Register(nameof(IsInPositionChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public static readonly DependencyProperty IsBusyCheckedProperty = DependencyProperty.Register(nameof(IsBusyChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty IsErrorCheckedProperty = DependencyProperty.Register(nameof(IsErrorChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty IsHommingCheckedProperty = DependencyProperty.Register(nameof(IsHommingChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        public static readonly DependencyProperty IsCalibrationCheckedProperty = DependencyProperty.Register(nameof(IsCalibrationChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty IsInputVelocityCheckedProperty = DependencyProperty.Register(nameof(IsInputVelocityChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty IsLowVelocityCheckedProperty = DependencyProperty.Register(nameof(IsLowVelocityChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty IsMidVelocityCheckedProperty = DependencyProperty.Register(nameof(IsMidVelocityChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty IsHighVelocityCheckedProperty = DependencyProperty.Register(nameof(IsHighVelocityChecked), typeof(bool), typeof(MotionControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty ServoEnableCommandProperty = DependencyProperty.Register(nameof(ServoEnableCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ServoDisableCommandProperty = DependencyProperty.Register(nameof(ServoDisableCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ServoHomeCommandProperty = DependencyProperty.Register(nameof(ServoHomeCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ServoStopCommandProperty = DependencyProperty.Register(nameof(ServoStopCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty JogPlusCommandProperty = DependencyProperty.Register(nameof(JogPlusCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));
        public static readonly DependencyProperty JogMinusCommandProperty = DependencyProperty.Register(nameof(JogMinusCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty MoveAbsCommandProperty = DependencyProperty.Register(nameof(MoveAbsCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty MoveRelCommandProperty = DependencyProperty.Register(nameof(MoveRelCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ErrorResetCommandProperty = DependencyProperty.Register(nameof(ErrorResetCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty EStopCommandProperty = DependencyProperty.Register(nameof(EStopCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty LoadedCommandProperty = DependencyProperty.Register(nameof(LoadedCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty UnloadedCommandProperty = DependencyProperty.Register(nameof(UnloadedCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty UserInputVelocityCommandProperty = DependencyProperty.Register(nameof(UserInputVelocityCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty VelocityCheckedCommandProperty = DependencyProperty.Register(nameof(VelocityCheckedCommand), typeof(ICommand), typeof(MotionControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ActualPositionProperty = DependencyProperty.Register(nameof(ActualPosition), typeof(double), typeof(MotionControl), new PropertyMetadata(0.0));
        public static readonly DependencyProperty CommandPositionProperty = DependencyProperty.Register(nameof(CommandPosition), typeof(double), typeof(MotionControl), new PropertyMetadata(0.0));

        public static readonly DependencyProperty MoveAbsPositionProperty = DependencyProperty.Register(nameof(MoveAbsPosition), typeof(double), typeof(MotionControl), new PropertyMetadata(0.0));

        public static readonly DependencyProperty MoveRelDistanceProperty = DependencyProperty.Register(nameof(MoveRelDistance), typeof(double), typeof(MotionControl), new PropertyMetadata(0.0));

        public static readonly DependencyProperty InputVelocityProperty = DependencyProperty.Register(nameof(InputVelocity), typeof(double), typeof(MotionControl), new PropertyMetadata(0.0));

        public static readonly DependencyProperty LowVelocityValueProperty = DependencyProperty.Register(nameof(LowVelocityValue), typeof(double), typeof(MotionControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty MidVelocityValueProperty = DependencyProperty.Register(nameof(MidVelocityValue), typeof(double), typeof(MotionControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty HighVelocityValueProperty = DependencyProperty.Register(nameof(HighVelocityValue), typeof(double), typeof(MotionControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));

        public static readonly DependencyProperty SelectedMotionAxisProperty = DependencyProperty.Register(nameof(SelectedMotionAxis), typeof(string), typeof(MotionControl), new PropertyMetadata(""));

        public static readonly DependencyProperty ServoErrorTextProperty = DependencyProperty.Register(nameof(ServoErrorText), typeof(string), typeof(MotionControl), new PropertyMetadata(""));

        // 250121_mh.yun
        public static readonly DependencyProperty ActualVelocityProperty = DependencyProperty.Register(nameof(ActualVelocity), typeof(double), typeof(MotionControl), new PropertyMetadata(0.0));
        public static readonly DependencyProperty CommandVelocityProperty = DependencyProperty.Register(nameof(CommandVelocity), typeof(double), typeof(MotionControl), new PropertyMetadata(0.0));



        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MotionControl;
            if (control != null)
            {
                if (e.Property == IsPlusLimitCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsPlusLimitChecked = isChecked;
                }
                else if (e.Property == IsMinusLimitCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsMinusLimitChecked = isChecked;
                }
                else if (e.Property == IsEnableCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsEnableChecked = isChecked;
                }
                else if (e.Property == IsDisableCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsDisableChecked = isChecked;
                }
                else if (e.Property == IsInPositionCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsInPositionChecked = isChecked;
                }
                else if (e.Property == IsBusyCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsBusyChecked = isChecked;
                }
                else if (e.Property == IsErrorCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsErrorChecked = isChecked;
                }
                else if (e.Property == IsHommingCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsHommingChecked = isChecked;
                }
                else if (e.Property == IsCalibrationCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsCalibrationChecked = isChecked;
                }
                else if (e.Property == IsInputVelocityCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsInputVelocityChecked = isChecked;

                    if (isChecked)
                    {
                        //control.TextBox_UserInputVelocity.Text = string.Format("{0:F3}", restoreInputVelocity);
                        //control.InputVelocity = restoreInputVelocity;
                    }
                }
                else if (e.Property == IsLowVelocityCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsLowVelocityChecked = isChecked;

                    if (isChecked)
                    {
                        //control.TextBox_UserInputVelocity.Text = string.Format("{0:F3}", control.LowVelocityValue);
                        //control.InputVelocity = control.LowVelocityValue;
                    }
                }
                else if (e.Property == IsMidVelocityCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsMidVelocityChecked = isChecked;

                    if (isChecked)
                    {
                        //control.TextBox_UserInputVelocity.Text = string.Format("{0:F3}", control.MidVelocityValue);
                        //ontrol.InputVelocity = control.MidVelocityValue;
                    }
                }
                else if (e.Property == IsHighVelocityCheckedProperty)
                {
                    bool isChecked = (bool)e.NewValue;
                    control.IsHighVelocityChecked = isChecked;

                    if (isChecked)
                    {
                        //control.TextBox_UserInputVelocity.Text = string.Format("{0:F3}", control.HighVelocityValue);
                        //control.InputVelocity = control.HighVelocityValue;
                    }

                }
                else if (e.Property == LowVelocityValueProperty)
                {
                    double value = (double)e.NewValue;
                    control.LowVelocityValue = value;
                    control.RadioButton_LowVelocity.ToolTip = string.Format("{0:F2} mm/s", value);
                }
                else if (e.Property == MidVelocityValueProperty)
                {
                    double value = (double)e.NewValue;
                    control.MidVelocityValue = value;
                    control.RadioButton_MidVelocity.ToolTip = string.Format("{0:F2} mm/s", value);
                }
                else if (e.Property == HighVelocityValueProperty)
                {
                    double value = (double)e.NewValue;
                    control.HighVelocityValue = value;
                    control.RadioButton_HighVelocity.ToolTip = string.Format("{0:F2} mm/s", value);
                }
            }
        }

        public List<string> MotionAxisList
        {
            get { return (List<string>)GetValue(MotionAxisListProperty); }
            set { SetValue(MotionAxisListProperty, value); }
        }

        public bool IsPlusLimitChecked
        {
            get { return (bool)GetValue(IsPlusLimitCheckedProperty); }
            set { SetValue(IsPlusLimitCheckedProperty, value); }
        }

        public bool IsMinusLimitChecked
        {
            get { return (bool)GetValue(IsMinusLimitCheckedProperty); }
            set { SetValue(IsMinusLimitCheckedProperty, value); }
        }

        public bool IsEnableChecked
        {
            get { return (bool)GetValue(IsEnableCheckedProperty); }
            set { SetValue(IsEnableCheckedProperty, value); }
        }

        public bool IsDisableChecked
        {
            get { return (bool)GetValue(IsDisableCheckedProperty); }
            set { SetValue(IsDisableCheckedProperty, value); }
        }

        public bool IsInPositionChecked
        {
            get { return (bool)GetValue(IsInPositionCheckedProperty); }
            set { SetValue(IsInPositionCheckedProperty, value); }
        }

        public bool IsBusyChecked
        {
            get { return (bool)GetValue(IsBusyCheckedProperty); }
            set { SetValue(IsBusyCheckedProperty, value); }
        }

        public bool IsErrorChecked
        {
            get { return (bool)GetValue(IsErrorCheckedProperty); }
            set { SetValue(IsErrorCheckedProperty, value); }
        }

        public bool IsHommingChecked
        {
            get { return (bool)GetValue(IsHommingCheckedProperty); }
            set { SetValue(IsHommingCheckedProperty, value); }
        }

        public bool IsCalibrationChecked
        {
            get { return (bool)GetValue(IsCalibrationCheckedProperty); }
            set { SetValue(IsCalibrationCheckedProperty, value); }
        }

        public bool IsInputVelocityChecked
        {
            get { return (bool)GetValue(IsInputVelocityCheckedProperty); }
            set { SetValue(IsInputVelocityCheckedProperty, value); }
        }

        public bool IsLowVelocityChecked
        {
            get { return (bool)GetValue(IsLowVelocityCheckedProperty); }
            set { SetValue(IsLowVelocityCheckedProperty, value); }
        }
        public bool IsMidVelocityChecked
        {
            get { return (bool)GetValue(IsMidVelocityCheckedProperty); }
            set { SetValue(IsMidVelocityCheckedProperty, value); }
        }
        public bool IsHighVelocityChecked
        {
            get { return (bool)GetValue(IsHighVelocityCheckedProperty); }
            set { SetValue(IsHighVelocityCheckedProperty, value); }
        }

        public DelegateCommand<SelectionChangedEventArgs> MotionAxisChangedCommand
        {
            get { return (DelegateCommand<SelectionChangedEventArgs>)GetValue(MotionAxisChangedProperty); }
            set { SetValue(MotionAxisChangedProperty, value); }
        }

        public ICommand ServoEnableCommand
        {
            get { return (ICommand)GetValue(ServoEnableCommandProperty); }
            set { SetValue(ServoEnableCommandProperty, value); }
        }

        public ICommand ServoDisableCommand
        {
            get { return (ICommand)GetValue(ServoDisableCommandProperty); }
            set { SetValue(ServoDisableCommandProperty, value); }
        }

        public ICommand ServoHomeCommand
        {
            get { return (ICommand)GetValue(ServoHomeCommandProperty); }
            set { SetValue(ServoHomeCommandProperty, value); }
        }

        public ICommand ServoStopCommand
        {
            get { return (ICommand)GetValue(ServoStopCommandProperty); }
            set { SetValue(ServoStopCommandProperty, value); }
        }

        public ICommand JogPlusCommand
        {
            get { return (ICommand)GetValue(JogPlusCommandProperty); }
            set { SetValue(JogPlusCommandProperty, value); }
        }

        public ICommand JogMinusCommand
        {
            get { return (ICommand)GetValue(JogMinusCommandProperty); }
            set { SetValue(JogMinusCommandProperty, value); }
        }

        public ICommand MoveAbsCommand
        {
            get { return (ICommand)GetValue(MoveAbsCommandProperty); }
            set { SetValue(MoveAbsCommandProperty, value); }
        }
        public ICommand MoveRelCommand
        {
            get { return (ICommand)GetValue(MoveRelCommandProperty); }
            set { SetValue(MoveRelCommandProperty, value); }
        }

        public ICommand UserInputVelocityCommand
        {
            get { return (ICommand)GetValue(UserInputVelocityCommandProperty); }
            set { SetValue(UserInputVelocityCommandProperty, value); }
        }

        public ICommand ErrorResetCommand
        {
            get { return (ICommand)GetValue(ErrorResetCommandProperty); }
            set { SetValue(ErrorResetCommandProperty, value); }
        }

        public ICommand EStopCommand
        {
            get { return (ICommand)GetValue(EStopCommandProperty); }
            set { SetValue(EStopCommandProperty, value); }
        }

        public ICommand LoadedCommand
        {
            get { return (ICommand)GetValue(LoadedCommandProperty); }
            set { SetValue(LoadedCommandProperty, value); }
        }

        public ICommand UnloadedCommand
        {
            get { return (ICommand)GetValue(UnloadedCommandProperty); }
            set { SetValue(UnloadedCommandProperty, value); }
        }

        public ICommand VelocityCheckedCommand
        {
            get { return (ICommand)GetValue(VelocityCheckedCommandProperty); }
            set { SetValue(VelocityCheckedCommandProperty, value); }
        }

        public double ActualPosition
        {
            get { return (double)GetValue(ActualPositionProperty); }
            set { SetValue(ActualPositionProperty, value); }
        }

        public double CommandPosition
        {
            get { return (double)GetValue(CommandPositionProperty); }
            set { SetValue(CommandPositionProperty, value); }
        }

        public double MoveAbsPosition
        {
            get { return (double)GetValue(MoveAbsPositionProperty); }
            set { SetValue(MoveAbsPositionProperty, value); }
        }

        // 250121_mh.yun
        public double ActualVelocity
        {
            get { return (double)GetValue(ActualVelocityProperty); }
            set { SetValue(ActualVelocityProperty, value); }
        }

        public double CommandVelocity
        {
            get { return (double)GetValue(CommandVelocityProperty); }
            set { SetValue(CommandVelocityProperty, value); }
        }

        public double MoveRelDistance
        {
            get { return (double)GetValue(MoveRelDistanceProperty); }
            set { SetValue(MoveRelDistanceProperty, value); }
        }

        public double InputVelocity
        {
            get { return (double)GetValue(InputVelocityProperty); }
            set { SetValue(InputVelocityProperty, value); }
        }
        public double LowVelocityValue
        {
            get { return (double)GetValue(LowVelocityValueProperty); }
            set { SetValue(LowVelocityValueProperty, value); }
        }
        public double MidVelocityValue
        {
            get { return (double)GetValue(MidVelocityValueProperty); }
            set { SetValue(MidVelocityValueProperty, value); }
        }
        public double HighVelocityValue
        {
            get { return (double)GetValue(HighVelocityValueProperty); }
            set { SetValue(HighVelocityValueProperty, value); }
        }
        public string SelectedMotionAxis
        {
            get { return (string)GetValue(SelectedMotionAxisProperty); }
            set { SetValue(SelectedMotionAxisProperty, value); }
        }

        public string ServoErrorText
        {
            get { return (string)GetValue(ServoErrorTextProperty); }
            set { SetValue(ServoErrorTextProperty, value); }
        }

        public MotionControl()
        {
            InitializeComponent();
        }

        private void ButtonServoEnable_Click(object sender, RoutedEventArgs e)
        {
            if (ServoEnableCommand != null && ServoEnableCommand.CanExecute(null))
            {
                ServoEnableCommand.Execute(null);
            }
        }

        private void ButtonServoDisable_Click(object sender, RoutedEventArgs e)
        {
            if (ServoDisableCommand != null && ServoDisableCommand.CanExecute(null))
            {
                ServoDisableCommand.Execute(null);
            }
        }

        private void ButtonHome_Click(object sender, RoutedEventArgs e)
        {
            if (ServoHomeCommand != null && ServoHomeCommand.CanExecute(null))
            {
                ServoHomeCommand.Execute(null);
            }
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            if (ServoStopCommand != null && ServoStopCommand.CanExecute(null))
            {
                ServoStopCommand.Execute(null);
            }
        }

        private void ButtonJogPlus_Click(object sender, RoutedEventArgs e)
        {
            if (JogPlusCommand != null && JogPlusCommand.CanExecute(null))
            {
                JogPlusCommand.Execute(null);
            }
        }

        private void ButtonJogMinus_Click(object sender, RoutedEventArgs e)
        {
            if (JogMinusCommand != null && JogMinusCommand.CanExecute(null))
            {
                JogMinusCommand.Execute(null);
            }
        }

        private void ButtonMoveAbs_Click(object sender, RoutedEventArgs e)
        {
            if (MoveAbsCommand != null && MoveAbsCommand.CanExecute(null))
            {
                MoveAbsCommand.Execute(null);
            }
        }

        private void ButtonMoveRel_Click(object sender, RoutedEventArgs e)
        {
            if (MoveRelCommand != null && MoveRelCommand.CanExecute(null))
            {
                MoveRelCommand.Execute(null);
            }
        }

        private void ButtonUserInputVelocity_Click(object sender, RoutedEventArgs e)
        {
            if (UserInputVelocityCommand != null && UserInputVelocityCommand.CanExecute(null))
            {
                UserInputVelocityCommand.Execute(null);
            }
        }

        private void TextBox_UserInputVelocity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return (text.All(char.IsNumber) || text == ".");
        }

        private void ButtonErrorReset_Click(object sender, RoutedEventArgs e)
        {
            if (ErrorResetCommand != null && ErrorResetCommand.CanExecute(null))
            {
                ErrorResetCommand.Execute(null);
            }
        }

        private void ButtonESTOP_Click(object sender, RoutedEventArgs e)
        {
            if (EStopCommand != null && EStopCommand.CanExecute(null))
            {
                EStopCommand.Execute(null);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoadedCommand != null && LoadedCommand.CanExecute(null))
            {
                LoadedCommand.Execute(null);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (UnloadedCommand != null && UnloadedCommand.CanExecute(null))
            {
                UnloadedCommand.Execute(null);
            }
        }


        private void RadioButton_VelocityChecked(object sender, RoutedEventArgs e)
        {
            if (VelocityCheckedCommand != null && VelocityCheckedCommand.CanExecute(null))
            {
                VelocityCheckedCommand.Execute(null);
            }
        }

        // 250122_mh.yun
        private void ButtonJogMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (JogMinusCommand != null && JogMinusCommand.CanExecute(null))
            {
                JogMinusCommand.Execute(null);
            }
        }

        private void ButtonJogPlus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (JogPlusCommand != null && JogPlusCommand.CanExecute(null))
            {
                JogPlusCommand.Execute(null);
            }
        }

        // Jog Plus/Minus 버튼 공통
        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ServoStopCommand != null && ServoStopCommand.CanExecute(null))
            {
                ServoStopCommand.Execute(null);
            }
        }

        private void ComboBox_AxisName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MotionAxisChangedCommand != null && MotionAxisChangedCommand.CanExecute(e))
            {
                MotionAxisChangedCommand.Execute(e);
            }
        }



        // 마우스가 버튼을 벗어났을 때 동작 중지
        //private void Button_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (ServoStopCommand != null && ServoStopCommand.CanExecute(null))
        //    {
        //        ServoStopCommand.Execute(null);
        //    }
        //}
    }
}
