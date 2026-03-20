namespace EPLE.Manager.Alarm
{
    public enum ShowDialogOptions
    {
        Ok,
        YesNo,
    }
    public interface IDialogService
    {
        TaskResponseType ShowDialog(string title, string message, ShowDialogOptions showDialogOptions = ShowDialogOptions.YesNo);
    }
}
