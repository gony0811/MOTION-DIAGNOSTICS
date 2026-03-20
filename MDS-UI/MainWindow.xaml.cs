using System.Windows;
using MDS.UI.ViewModels;

namespace MDS.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.GetService<ShellViewModel>();
        }
    }
}
