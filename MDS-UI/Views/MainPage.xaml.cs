using MDS.UI.ViewModels;
using System.Windows.Controls;

namespace MDS.UI.Views
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
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void WaferLoadingControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }
    }
}
