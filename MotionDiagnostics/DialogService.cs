using EPLE.Manager.Alarm;
using System;
using System.Windows;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace MotionDiagnostics
{
    public class DialogService : IDialogService
    {
        public TaskResponseType ShowDialog(string title, string message, ShowDialogOptions options = ShowDialogOptions.YesNo)
        {
            TaskResponseType result = TaskResponseType.STOP;

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (options == ShowDialogOptions.Ok)
                {
                    RadWindow.Alert(new DialogParameters
                    {
                        Header = title,
                        Content = message,
                        OkButtonContent = "확인"
                    });
                    result = TaskResponseType.RUNNING;
                }
                else if (options == ShowDialogOptions.YesNo)
                {
                    var frame = new DispatcherFrame();
                    RadWindow.Confirm(new DialogParameters
                    {
                        Header = title,
                        Content = message,
                        OkButtonContent = "예",
                        CancelButtonContent = "아니오",
                        Closed = (s, args) =>
                        {
                            result = args.DialogResult == true
                                ? TaskResponseType.RUNNING
                                : TaskResponseType.STOP;
                            frame.Continue = false;
                        }
                    });
                    Dispatcher.PushFrame(frame);
                }
            });

            return result;
        }
    }
}
