using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EPLE.Core.Service.Interface;
using EPLE.Manager;
using EPLE.Manager.Alarm;
using MotionDiagnostics.Common;
using MotionDiagnostics.ViewModels;
using Telerik.Windows.Controls;

namespace MotionDiagnostics
{
    public partial class MainWindow : Window
    {
        private readonly AlarmManager alarmManager;
        private readonly DataManager dataManager;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = App.GetService<ShellViewModel>();
            alarmManager = App.GetService<AlarmManager>();
            dataManager = App.GetService<DataManager>();
            dataManager.SetDialogService(App.GetService<DialogService>());

            alarmManager.SetAlarmEvent += OnAlarmEvent;
        }

        private void OnMessageBoxRequested(object sender, MessageBoxEventArgs e)
        {
            MessageBox.Show(e.Message, e.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private async void OnAlarmEvent(object sender, AlarmEventArgs e)
        {
            string title = string.Empty;
            string message = string.Empty;

            switch (e.Alarm.Level)
            {
                case EPLE.Data.ALCD.HEAVY:
                    title = $"경고: {e.Alarm.Name}";
                    message = $"중대한 알람이 발생했습니다. 즉시 조치가 필요합니다.{e.Alarm.Description}";
                    break;
                case EPLE.Data.ALCD.LIGHT:
                    title = $"알림: {e.Alarm.Name}";
                    message = $"경미한 알람이 발생했습니다. 상황을 확인하세요. {e.Alarm.Description}";
                    break;
                default:
                    title = $"알림: {e.Alarm.Name}";
                    message = $"알 수 없는 알람이 발생했습니다. 확인이 필요합니다. {e.Alarm.Description}";
                    break;
            }

            var userResponse = await ShowErrorDialog(title, message);
            AlarmResponseAwaiter.SetUserResponse(e.Alarm, userResponse);
        }

        private Task<TaskResponseType> ShowErrorDialog(string title, string message)
        {
            var tcs = new TaskCompletionSource<TaskResponseType>();

            Application.Current.Dispatcher.Invoke(() =>
            {
                var dialogParams = new DialogParameters
                {
                    Header = title,
                    Content = message,
                    Closed = (s, args) =>
                    {
                        var result = args.DialogResult;
                        if (result == true)
                            tcs.SetResult(TaskResponseType.RUNNING);  // Ignore
                        else
                            tcs.SetResult(TaskResponseType.STOP);  // Abort
                    }
                };
                dialogParams.OkButtonContent = "Ignore";
                dialogParams.CancelButtonContent = "Abort";

                RadWindow.Confirm(dialogParams);
            });

            return tcs.Task;
        }
    }
}
