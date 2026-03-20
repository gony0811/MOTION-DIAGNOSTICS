
using EPLE.Manager;
using EPLE.Core.Service;
using System.Threading.Tasks;
using System.Threading;
using Serilog;
using EPLE.Data;
using System;
using System.Collections.Generic;
using System.Windows;

namespace EPLE.Service
{
    public partial class SequenceService : AbstractService
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;
        private Task pollingTask;

        public SequenceService(ILogger logger, DataManager dataManager, DeviceManager deviceManager) : base(logger)
        {
            this.deviceManager = deviceManager;
            this.dataManager = dataManager;
        }


        public List<Point> MoveTargetIndexLists = new List<Point>();


        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await deviceManager.AttachDevices(cancellationToken);

            await dataManager.InitializeData(cancellationToken);

            pollingTask = dataManager.PollingStart(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // TaskCompletionSource를 사용하여 비동기적으로 기다림
            var tcs = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                var completedTask = await Task.WhenAny(pollingTask, tcs.Task);
                if (completedTask == tcs.Task)
                {
                    // 취소된 경우
                    await tcs.Task;
                }
                else
                {
                    // pollingTask가 완료된 경우
                    await pollingTask;
                }
            }

            await deviceManager.DetachDevices(cancellationToken);
        }

        public async Task XMoveAbs(CancellationToken cancellationToken, double velocity, double absolute_position, int timeout_second = 60)
        {
            dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, velocity);
            dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEABS, absolute_position);

            DateTime st = DateTime.Now;

            while (!cancellationToken.IsCancellationRequested)
            {
                TimeSpan elipsed = DateTime.Now - st;

                await Task.Delay(100);

                var xBusy = dataManager.GET_INT(DataNameHelper.X_IN_BUSY, out _);

                if (xBusy == 0)
                {
                    break;
                }
                else if (elipsed.TotalMilliseconds >= timeout_second)
                {
                    break;
                }
            }
        }

        public async Task YMoveAbs(CancellationToken cancellationToken, double velocity, double absolute_position, int timeout_second = 60)
        {
            dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, velocity);
            dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEABS, absolute_position);

            DateTime st = DateTime.Now;

            while (!cancellationToken.IsCancellationRequested)
            {
                TimeSpan elipsed = DateTime.Now - st;

                await Task.Delay(100);

                var xBusy = dataManager.GET_INT(DataNameHelper.Y_IN_BUSY, out _);

                if (xBusy == 0)
                {
                    break;
                }
                else if (elipsed.TotalSeconds >= timeout_second)
                {
                    break;
                }
            }
        }

        public async Task Z1MoveRel(CancellationToken cancellationToken, double velocity, double relative_position, int timeout_second = 60)
        {
            dataManager.SET_DATA(DataNameHelper.Z1_OUT_VELOCITY, velocity);
            dataManager.SET_DATA(DataNameHelper.Z1_OUT_MOVEREL, relative_position);

            DateTime st = DateTime.Now;

            while (!cancellationToken.IsCancellationRequested)
            {
                TimeSpan elipsed = DateTime.Now - st;

                await Task.Delay(100);

                var xBusy = dataManager.GET_INT(DataNameHelper.Z1_IN_BUSY, out _);

                if (xBusy == 0)
                {
                    break;
                }
                else if (elipsed.TotalSeconds >= timeout_second)
                {
                    break;
                }
            }
        }

        public void MakeMeasureList()
        {
            MoveTargetIndexLists.Clear();

            var waferGridRow = this.dataManager.GET_INT(DataNameHelper.SET_GRID_ROW, out _);
            var waferGridCol = this.dataManager.GET_INT(DataNameHelper.SET_GRID_COL, out _);
            var waferGridPitch = this.dataManager.GET_INT(DataNameHelper.SET_GRID_PITCH, out _);

            var colStartIndex = 0;
            var rowStartIndex = 0;

            for (int row = 0; row < waferGridRow; row++)
            {
                //var colCount = Display.Instance.FormAuto.waferMapData[row];

                //colStartIndex = (colCount - 1) / 2 * -1;
                //for (int col = 0; col < colCount; col++)
                //{
                //    MoveTargetIndexLists.Add((colStartIndex + col, rowStartIndex + row));
                //}
            }
        }

    }
}
