using System.ComponentModel;


namespace EPLE.ViewModel
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
#pragma warning disable CS0067 
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067 
    }
}
