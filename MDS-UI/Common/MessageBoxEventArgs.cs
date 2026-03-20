using System;

namespace MDS.UI.Common
{
    public class MessageBoxEventArgs : EventArgs
    {
        public string Message { get; }
        public string Title { get; }

        public MessageBoxEventArgs(string message, string title)
        {
            Message = message;
            Title = title;
        }
    }
}
