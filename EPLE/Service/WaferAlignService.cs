using EPLE.Core.Service;
using EPLE.Manager;
using EPLE.Data;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using EPLE.ImageProcessing;

namespace EPLE.Service
{

    public partial class WaferAlignService : AbstractService
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;
        private readonly HalconImageProcessing imageProcessing;
       

        public double AlignCenterX { get; set; }
        public double AlignCenterY { get; set; }
        public double AlignRightX { get; set; }
        public double AlignRightY { get; set; }
        public double AlignLeftX { get; set; }
        public double AlignLeftY { get; set; }

        public double RotationAngle { get; set; }

        public int AlignRetryCount { get; set; }

        public bool IsAlignCompleted { get; set; }


        public WaferAlignService(ILogger logger, DataManager dataManager, DeviceManager deviceManager, HalconImageProcessing imageProcessing) : base(logger)
        {
            this.dataManager = dataManager;
            this.deviceManager = deviceManager;
            this.imageProcessing = imageProcessing;
            IsAlignCompleted = false;
            AlignRetryCount = 3;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var executeCount = 0;
            IsAlignCompleted = false; 

            while (true)
            {
                // 재시도 횟수 초과 시 종료
                if (executeCount >= AlignRetryCount)
                {
                    logger.Information("Align Retry Count Over");
                    IsAlignCompleted = false;
                    break;
                }

                await Task.Delay(100); 

                // 웨이퍼의 Align을 위해 Center Position으로 이동
                // 성공 시 다음 단계 진행
                if (false == CheckResult("MoveToCenter", await this.MoveToCenter(cancellationToken))) 
                    break;

                // Mark 중심까지 좌표 추출
                if (false == CheckResult("MarkFind", await this.MarkFind(cancellationToken)))
                    break;

                AlignCenterX = dataManager.GET_DOUBLE(DataNameHelper.MARK_X_OFFSET, out _);
                AlignCenterY = dataManager.GET_DOUBLE(DataNameHelper.MARK_Y_OFFSET, out _);

                // 웨이퍼의 Align을 위해 Center에서 오른쪽으로 Position 이동
                // 성공 시 다음 단계 진행
                if (false == CheckResult("MoveToRight", await this.MoveToRight(cancellationToken)))
                    break;

                // Mark 중심까지 좌표 추출
                if (false ==CheckResult("MarkFind", await this.MarkFind(cancellationToken)))
                    break;

                AlignRightX = dataManager.GET_DOUBLE(DataNameHelper.MARK_X_OFFSET, out _);
                AlignRightY = dataManager.GET_DOUBLE(DataNameHelper.MARK_Y_OFFSET, out _);

                // 회전 축 수평 정렬 이동 보정 각도 계산
                RotationAngle = CalcRotationAngle() * this.dataManager.GET_INT(DataNameHelper.T_DIRECTION_INVERT, out _);

                if (Math.Abs(RotationAngle) > 0.300)
                {
                    logger.Error("최대 각도를 벗어났습니다. 각도 이상을 점검해주세요.");
                    break;
                }
                else if (false == CheckMeasureAngle(RotationAngle, 0.001))
                {
                    // 실행 횟수 증가
                    executeCount++;
                }

                // Rotation Align 진행
                if (CheckResult("RotationAlign", await this.RotationAlign(cancellationToken)))
                {
                    IsAlignCompleted = true;
                    return;
                }
            }

            IsAlignCompleted = false;
        }

        private bool CheckMeasureAngle(double measuredAngle, double tolerance_angle)
        {
            if (Math.Abs(measuredAngle) <= tolerance_angle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckResult(string func_name, Result result)
        {
            if (result == Result.SUCCESS)
            {
                logger.Information($"{func_name} success.");
                return true;
            }
            else if (result == Result.CANCELED)
            {
                logger.Information($"{func_name} canceled.");
                return false;
            }
            else if (result == Result.TIMEOUT)
            {
                logger.Information($"{func_name} timeout.");
                return false;
            }
            else if (result == Result.FAILED)
            {
                logger.Information($"{func_name} failed.");
                return false;
            }
            else
            {
                return false;
            }
        }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Wafer의 Align을 위해 Center Position으로 이동
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result> MoveToCenter(CancellationToken cancellationToken, int timeout_seconds = 60)
        {
            logger.Information("MoveToCenter Start");

            var inspectionCenterX = dataManager.GET_DOUBLE(DataNameHelper.X_CENTER_POSITION, out _);
            var inspectionCenterY = dataManager.GET_DOUBLE(DataNameHelper.Y_CENTER_POSITION, out _);

            logger.Information($"좌측 이동 위치 값 X: {inspectionCenterX:F3} mm, Y: {inspectionCenterY:F3} mm");

            dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, 30);
            dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, 30);
            dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEABS, inspectionCenterX);
            dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEABS, inspectionCenterY);

            var st = DateTime.Now;

            while (true)
            {
                await Task.Delay(100);
                var xBusy = dataManager.GET_INT(DataNameHelper.X_IN_BUSY, out _);
                var yBusy = dataManager.GET_INT(DataNameHelper.Y_IN_BUSY, out _);

                TimeSpan ts = DateTime.Now - st;

                if (xBusy == 0 && yBusy == 0)
                {
                    logger.Information("MoveToCenter End");
                    return Result.SUCCESS;
                }
                else if (cancellationToken.IsCancellationRequested)
                {
                    dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, 1);
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, 1);
                    logger.Information("MoveToCenter Cancelled");
                    return Result.CANCELED;
                }
                else if (ts.TotalSeconds >= timeout_seconds)
                {
                    logger.Information("MoveToCenter Timeout");
                    dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, 1);
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, 1);
                    return Result.TIMEOUT;
                }
            }
        }

        /// <summary>
        /// Wafer의 Align을 위해 Center Position으로 이동 후 Mark를 찾아서 중심으로 이동
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="timeout_seconds"></param>
        /// <returns></returns>

        public async Task<Result> MarkFind(CancellationToken cancellationToken, int timeout_seconds = 60)
        {
            logger.Information("MarkFind Start");

            if (!imageProcessing.IsGrabStart)
            {
                imageProcessing.GrapStart();
                logger.Information("Image grap start");
            }

            bool find_result = false;
            var st = DateTime.Now;

            Task<(double x, double y)> mark_find = Task.Run(() => imageProcessing.Find(out find_result));

            while (true)
            {
                await Task.Delay(100);

                var elipsed = DateTime.Now - st;

                if (cancellationToken.IsCancellationRequested)
                {
                    logger.Information("MarkFind Cancelled");
                    return Result.CANCELED;
                }
                else if (mark_find.IsCompleted && find_result)
                {
                    logger.Information("MarkFind success");
                    break;
                }
                else if (mark_find.IsCompleted && find_result == false)
                {
                    logger.Information("MarkFind failed");
                    return Result.FAILED;
                }
                else if (elipsed.TotalSeconds >= timeout_seconds)
                {
                    logger.Error("MarkFind timeout");
                    return Result.TIMEOUT;
                }
            }

            var direction_invert_x = this.dataManager.GET_INT(DataNameHelper.X_DIRECTION_INVERT, out _);
            var direction_invert_y = this.dataManager.GET_INT(DataNameHelper.Y_DIRECTION_INVERT, out _);

            var inspectionOffsetX = mark_find.Result.x * direction_invert_x;
            var inspectionOffsetY = mark_find.Result.y * direction_invert_y;

            logger.Information($"마크 중심 위치 옵셋 X: {inspectionOffsetX:F3} mm, Y: {inspectionOffsetY:F3} mm");

            dataManager.SET_DATA(DataNameHelper.MARK_X_OFFSET, inspectionOffsetX);
            dataManager.SET_DATA(DataNameHelper.MARK_Y_OFFSET, inspectionOffsetY);

            return Result.SUCCESS;
        }

        public async Task<Result> MoveToRight(CancellationToken cancellationToken, int timeout_seconds = 60)
        {
            logger.Information("MoveToRight Start");
            var inspectionRightX = dataManager.GET_DOUBLE(DataNameHelper.X_RIGHTEDGE_POSITION, out _);
            var inspectionRightY = dataManager.GET_DOUBLE(DataNameHelper.Y_RIGHTEDGE_POSITION, out _);
            logger.Information($"우측 이동 위치 값 X: {inspectionRightX:F3} mm, Y: {inspectionRightY:F3} mm");

            dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, 30);
            dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, 30);
            dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEABS, inspectionRightX);
            dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEABS, inspectionRightY);
            var st = DateTime.Now;

            while (true)
            {
                await Task.Delay(100);
                var xBusy = dataManager.GET_INT(DataNameHelper.X_IN_BUSY, out _);
                var yBusy = dataManager.GET_INT(DataNameHelper.Y_IN_BUSY, out _);
                TimeSpan ts = DateTime.Now - st;
                if (xBusy == 0 && yBusy == 0)
                {
                    logger.Information("MoveToRight End");
                    return Result.SUCCESS;
                }
                else if (cancellationToken.IsCancellationRequested)
                {
                    dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, 1);
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, 1);
                    logger.Information("MoveToRight Cancelled");
                    return Result.CANCELED;
                }
                else if (ts.TotalSeconds >= timeout_seconds)
                {
                    logger.Information("MoveToRight Timeout");
                    dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, 1);
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, 1);
                    return Result.TIMEOUT;
                }
            }
        }

        public async Task<Result> RotationAlign(CancellationToken cancellationToken, int timeout_seconds = 60)
        {
            logger.Information("RotationAlign Start");


            logger.Information($"회전 축 수평 정렬 이동 보정 각도: {RotationAngle:F3} Degree");

            dataManager.SET_DATA(DataNameHelper.T_OUT_VELOCITY, 1);
            dataManager.SET_DATA(DataNameHelper.T_OUT_MOVEREL, RotationAngle);

            var st = DateTime.Now;

            while (true)
            {
                await Task.Delay(100);

                TimeSpan ts = DateTime.Now - st;
                var tBusy = dataManager.GET_INT(DataNameHelper.T_IN_BUSY, out _);
                if (tBusy == 0)
                {
                    logger.Information("RotationAlign End");
                    return Result.SUCCESS;
                }
                else if (cancellationToken.IsCancellationRequested)
                {
                    dataManager.SET_DATA(DataNameHelper.T_OUT_STOP, 1);
                    logger.Information("RotationAlign Cancelled");
                    return Result.CANCELED;
                }
                else if (ts.TotalSeconds >= timeout_seconds)
                {
                    logger.Information("RotationAlign Timeout");
                    dataManager.SET_DATA(DataNameHelper.T_OUT_STOP, 1);
                    return Result.TIMEOUT;
                }
            }
        }

        private double CalcRotationAngle()
        {
            var tolerance_angle = 0.001;

            var measuredAngle = EPLE.Core.Utility.MathLib.GetAngle(AlignCenterX, AlignCenterY, AlignRightX, AlignRightY);

            logger.Information($"회전 각도: {measuredAngle:F6} Degree, 유효 범위 ({tolerance_angle * -1:F3} ~ {tolerance_angle * 1:F3})");

            return measuredAngle;
        }
    }
}
