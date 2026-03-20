using MDS.UI.ViewModels;
using System.Windows.Controls;

namespace MDS.UI.Views
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
