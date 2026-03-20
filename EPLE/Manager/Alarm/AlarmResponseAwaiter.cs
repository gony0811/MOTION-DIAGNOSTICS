
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPLE.ViewModel.AlarmVMList;

namespace EPLE.Manager.Alarm
{
    public enum TaskResponseType
    {
        RUNNING,
        RESTART,
        PAUSE,
        STOP
    }

    public static class AlarmResponseAwaiter
    {
        private static readonly Dictionary<AlarmVM, TaskCompletionSource<TaskResponseType>> AwaitingTasks = new Dictionary<AlarmVM, TaskCompletionSource<TaskResponseType>>();

        public static Task<TaskResponseType> GetUserResponseAsync(AlarmVM alarm)
        {
            var tcs = new TaskCompletionSource<TaskResponseType>();
            lock (AwaitingTasks)
            {
                AwaitingTasks[alarm] = tcs;
            }
            return tcs.Task;
        }

        public static void SetUserResponse(AlarmVM alarm, TaskResponseType response)
        {
            lock (AwaitingTasks)
            {
                if (AwaitingTasks.TryGetValue(alarm, out var tcs))
                {
                    tcs.SetResult(response);
                    AwaitingTasks.Remove(alarm);
                }
            }
        }
    }
}
