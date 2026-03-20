using MotionDiagnostics.ViewModels;
using System.Windows.Controls;

namespace MotionDiagnostics.Views
{
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            this.DataContext = App.GetService<SettingsViewModel>();
            InitializeComponent();

        }
    }
}
