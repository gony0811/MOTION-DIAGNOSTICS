using EPLE.Data;
using EPLE.Manager;
using Microsoft.Extensions.DependencyInjection;
using MotionDiagnostics.ViewModels;
using System.Windows;

namespace MotionDiagnostics.Modal
{
    /// <summary>
    /// Interaction logic for MotionModal.xaml
    /// </summary>
    public partial class MotionModal : Window
    {
        private static MotionModal _instance;
        private readonly object _bitmapLock = new object();
        public static MotionModal Instance
        {
            get
            {
                if (_instance == null || !_instance.IsLoaded)
                {
                    _instance = new MotionModal();
                }
                return _instance;
            }
        }

        public MotionModal()
        {
            DataContext = App.GetService<MotionModalVM>();
            InitializeComponent();
        }
      
        public void SetAndDisplayMotion(string name)
        {
            name = name.Split('_')[0];

            App.GetService<MotionModalVM>().SetData(name);
            ShowDialog();
        }

    }
}
