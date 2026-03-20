using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EPLE.Core.Service.Interface
{
    public interface IMainService
    {
        //
        // 요약:
        //     Triggered when the application host is ready to start the service.
        //
        // 매개 변수:
        //   cancellationToken:
        //     Indicates that the start process has been aborted.
        //
        // 반환 값:
        //     A System.Threading.Tasks.Task that represents the asynchronous Start operation.
        Task StartAsync(CancellationToken cancellationToken);

        //
        // 요약:
        //     Triggered when the application host is performing a graceful shutdown.
        //
        // 매개 변수:
        //   cancellationToken:
        //     Indicates that the shutdown process should no longer be graceful.
        //
        // 반환 값:
        //     A System.Threading.Tasks.Task that represents the asynchronous Stop operation.
        Task StopAsync(CancellationToken cancellationToken);
    }
}
