using MDS.UI.ViewModels;
using System.Windows;

namespace MDS.UI.Modal
{
    public partial class MotionModal : Window
    {
        private static MotionModal _instance;

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
