using Autofac;
using EPLE.Manager;
using HalconDotNet;
using EPLE.ImageProcessing;
using MotionDiagnostics.UserControls.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Threading;

namespace MotionDiagnostics.UserControls
{
    /// <summary>
    /// HalconImageProcessingControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HalconImageProcessingControl : UserControl
    {
        //private HDrawingObject ROI;
        //private List<HDrawingObject> drawing_objects = new List<HDrawingObject>();
        //private HDrawingObject selected_drawing_object;

        public HalconImageProcessingControl()
        {
            InitializeComponent();
            DataContext = App.GetService<HalconImageProcessingControlViewModel>();
        }

        private void ImageProcessingOpen()
        {
            if (this.DataContext is HalconImageProcessingControlViewModel vm)
            {
                vm.Open(HalconSmartWindow.HalconWindow, "Base:C:/25M(4tap).cam", simulator: false);
            }
        }

     

        private void HalconSmartWindow_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void HalconSmartWindow_Initialized(object sender, EventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ImageProcessingOpen();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
