using EPLE.ImageProcessing;
using MotionDiagnostics.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using System.IO;
using MotionDiagnostics.UserControls.ViewModels;

namespace MotionDiagnostics.UserControls
{
    /// <summary>
    /// Interaction logic for ModelManagementControl.xaml
    /// </summary>
    public partial class VisionSettingControl : UserControl
    {

        // DependencyProperty 선언
        public static readonly DependencyProperty ModelManagementProperty =
           DependencyProperty.Register(
               nameof(ModelManagement),
               typeof(ModelManagement),
               typeof(VisionSettingControl),
               new PropertyMetadata(null, OnModelManagementChanged));

        // ModelManagement 속성
        public ModelManagement ModelManagement
        {
            get => (ModelManagement)GetValue(ModelManagementProperty);
            set => SetValue(ModelManagementProperty, value);
        }

        // DependencyProperty 변경 시 호출되는 콜백 메서드
        private static void OnModelManagementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VisionSettingControl control)
            {
                var newValue = e.NewValue as ModelManagement;
                var oldValue = e.OldValue as ModelManagement;

                // 변경된 값에 대해 추가 작업을 수행할 수 있음
                control.OnModelManagementUpdated(newValue, oldValue);
            }
        }

        // ModelManagement 변경에 따른 추가 작업을 수행하는 메서드
        private void OnModelManagementUpdated(ModelManagement newValue, ModelManagement oldValue)
        {
            // 필요한 추가 작업 작성
        }

        // 생성자
        public VisionSettingControl()
        {

            InitializeComponent();
            ModelManagement = ModelManagement.GetInstance();

            DataContext = App.GetService<VisionSettingControlViewModel>();

        }
    }
}
