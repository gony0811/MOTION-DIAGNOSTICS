
using MotionDiagnostics.UserControls;
using MotionDiagnostics.UserControls.ViewModels;
using MotionDiagnostics.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace MotionDiagnostics.Views
{
    //private readonly AlarmManager alarmManager = null!;

    public partial class AccuracyPage : UserControl
    {
        private bool _isMouseCursorEntered = false;
        private MappingAndMeasureControlViewModel mappingAndMeasureControlViewModel;
        private WaferControlViewModel waferControlViewModel;


        public AccuracyPage()
        {
            InitializeComponent();
            this.DataContext = App.GetService<AccuracyViewModel>();
            this.mappingAndMeasureControlViewModel = App.GetService<MappingAndMeasureControlViewModel>();
            this.waferControlViewModel = App.GetService<WaferControlViewModel>();
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                WaferControlViewModel vm = App.GetService<WaferControlViewModel>();
                vm.IsMultiSelection = true;
            }
            else if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                WaferControlViewModel vm = App.GetService<WaferControlViewModel>();
                vm.IsDragSelection = true;

                if (_isMouseCursorEntered)
                {
                    Mouse.OverrideCursor = Cursors.Cross;
                }
            }

        }

        private void Page_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                WaferControlViewModel vm = App.GetService<WaferControlViewModel>();
                vm.IsMultiSelection = false;
            }
            else if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                WaferControlViewModel vm = App.GetService<WaferControlViewModel>();
                vm.IsDragSelection = false;

                if (_isMouseCursorEntered)
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void ucWaferControl_MouseEnter(object sender, MouseEventArgs e)
        {
            _isMouseCursorEntered = true;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Mouse.OverrideCursor = Cursors.Cross;
            }
        }

        private void ucWaferControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
            _isMouseCursorEntered = false;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Focus();
        }

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void ucWaferControl_WaferSelectionChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            this.mappingAndMeasureControlViewModel.Selections = this.ucWaferControl.Selections as System.Collections.Generic.List<System.Tuple<int, int>>;
        }
    }
}
