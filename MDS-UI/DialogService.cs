using System.Windows;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace MDS.UI
{
    public class DialogService
    {
        public enum ShowDialogOptions { Ok, YesNo }
        public enum TaskResponseType { RUNNING, STOP }

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
                        OkButtonContent = "OK"
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
                        OkButtonContent = "Yes",
                        CancelButtonContent = "No",
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
