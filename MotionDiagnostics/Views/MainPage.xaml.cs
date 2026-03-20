using EPLE.Data;
using EPLE.Manager;
using MotionDiagnostics.UserControls;
using MotionDiagnostics.ViewModels;
using System.Windows.Controls;

namespace MotionDiagnostics.Views
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = App.GetService<MainPageViewModel>();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var dataManager = App.GetService<DataManager>();

            var mode = dataManager?.GET_INT(DataNameHelper.SYS_MODE_SIMULATION, out _);

            if (mode == 0)
            {
                var halconControl = App.GetService<HalconImageProcessingControl>();
                var visionSettingControl = App.GetService<VisionSettingControl>();

                // Set size for halconControl
                halconControl.Width = 640;
                halconControl.Height = 480;

                Grid.SetRow(halconControl, 0);
                Grid.SetRow(visionSettingControl, 1);
                this.VisionGrid.Children.Add(halconControl);
                this.VisionGrid.Children.Add(visionSettingControl);
            }

        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.VisionGrid.Children.Clear();
        }

        private void WaferLoadingControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
