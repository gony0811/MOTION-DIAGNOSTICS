using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionDiagnostics.Common
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
