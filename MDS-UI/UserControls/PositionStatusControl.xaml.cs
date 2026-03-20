using MDS.UI.Model;
using System.Windows;
using System.Windows.Controls;

namespace MDS.UI.UserControls
{
    /// <summary>
    /// Interaction logic for PositionStatusControl.xaml
    /// </summary>
    public partial class PositionStatusControl : UserControl
    {
        public static readonly DependencyProperty PositionStatusProperty =
            DependencyProperty.Register(
                 nameof(PositionStatus),
                 typeof(PositionStatus),
                 typeof(PositionStatusControl),
                 new FrameworkPropertyMetadata(
                     null,
                     FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                     OnChanged
         ));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PositionStatusControl)d;
            var newValue = e.NewValue as PositionStatus;
            var oldValue = e.OldValue as PositionStatus;

            if (newValue is PositionStatus positionStatus)
            {
                control.PositionStatus = positionStatus;
            }
        }

        public PositionStatus PositionStatus
        {
            get => (PositionStatus)GetValue(PositionStatusProperty);
            set => SetValue(PositionStatusProperty, value);
        }

        public PositionStatusControl()
        {
            InitializeComponent();
        }


        //private void StartTimer()
        //{
        //    _timer = new System.Timers.Timer(3000); // 3초마다 실행
        //    _timer.Elapsed += TimerElapsed;
        //    _timer.AutoReset = true; // 반복 실행
        //    _timer.Enabled = true;
        //}

        //private void TimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        //{
        //    Dispatcher.Invoke(() =>
        //    {
        //        // 3초마다 실행할 작업
        //        string positionName = positionStatusViewModel.positionStatus.PositionName;
        //        Debug.WriteLine($"Position Name: {positionName}");

        //        // Test Code
        //        PositionStatus status = positionStatusViewModel.positionStatus;
        //        status.PlusLimit = !status.PlusLimit; // 상태 변경
        //        status.MinusLimit = !status.MinusLimit;
        //    });
        //}

        //private void StopTimer()
        //{
        //    if (_timer != null)
        //    {
        //        _timer.Stop();
        //        _timer.Dispose();
        //        _timer = null;
        //    }
        //}
    }
}
