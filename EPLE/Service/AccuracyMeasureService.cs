using EPLE.Core.Service;
using EPLE.Data;
using EPLE.Manager;
using EPLE.Manager.Alarm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Serilog;
using System.Runtime.CompilerServices;
using EPLE.ImageProcessing;
using EPLE.ViewModel;
using System.Linq;
using NHibernate.Util;


namespace EPLE.Service
{
    public partial class AccuracyMeasureService : AbstractService
    {
        private readonly new ILogger logger;
        private readonly DataManager dataManager;
        private readonly AlarmManager alarmManager;
        private readonly HalconImageProcessing imageProcessing;
        private readonly MeasureVMList measureVMList;

        public MeasureCommandType CommandType { get; set; }
        public int RepeatCount { get; set; }
        public int SelectedRow { get; set; }
        public int SelectedCol { get; set; }
        public int CenterRow { get; set; }
        public int CenterCol { get; set; }

        public double CenterPositionX { get; set; }
        public double CenterPositionY { get; set; }
        public int Pitch { get; set; }

        public int DelayMiliSecondsAfterMove { get; set; } = 1000;

        // 측정 오차를 error_x, error_y 순서로 누적하여 저장
        public List<Tuple<double, double>> MeasureData = new List<Tuple<double, double>>();

        public AccuracyMeasureService(ILogger logger, DataManager dataManager, AlarmManager alarmManager, HalconImageProcessing imageProcessing, MeasureVMList measureVMList) : base(logger)
        {
            this.logger = logger;
            this.dataManager = dataManager;
            this.alarmManager = alarmManager;
            this.imageProcessing = imageProcessing;
            this.measureVMList = measureVMList;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> Measure(CancellationToken cancellationToken)
        {
            // 선택된 Wafer Grid의 Row, Col 검증 Rol, Col < 0 이면 Error
            if (SelectedRow < 0 || SelectedCol < 0 || RepeatCount <= 0)
            {
                logger.Error("측정에 대한 Row, Col, Repeat 설정을 확인하십시요.");
                return Result.FAILED;
            }

            // 측정 초기화
            // (2) 측정 기준정보 조회
            var measure = this.measureVMList.Measures.Where((item) => { return item.XPos == SelectedCol && item.YPos == SelectedRow; }).First();
            var executeCount = 0;

            measure.ErrorX = 0.0;
            measure.ErrorY = 0.0;

            MeasureData.Clear();

            while (true)
            {
                try
                {
                    // (1) 반복 횟수 확인
                    if (RepeatCount <= executeCount)
                    {
                        // 측정 결과 저장
                        measure.MeasureResult = MeasureResult.OK;
                        measure.ErrorX = MeasureData.Average((item) => { return item.Item1; });
                        measure.ErrorY = MeasureData.Average((item) => { return item.Item2; });
                        measure.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        measure.SaveChanges();
                        logger.Information("측정 완료");
                        return Result.SUCCESS;
                    }

                    // (3) 선택된 위치가 측정가능 한 위치인지 확인

                    if (measure.IsMeasurePoint == false)
                    {
                        logger.Information("선택된 위치는 측정 불가능한 위치입니다.");
                        return Result.FAILED;
                    }

                    // (4) 선택된 위치로 이동
                    if (false == CheckResult("MovePosition", await MovePosition(cancellationToken, SelectedRow, SelectedCol)))
                    {
                        measure.MeasureResult = MeasureResult.NG;
                        measure.SaveChanges();
                        return Result.FAILED;
                    }


                    // (5) 대기 1초 (사용자가 취소할 수 있도록 대기)
                    await Task.Delay(DelayMiliSecondsAfterMove, cancellationToken);

                    // (6) Mark 찾기
                    if (false == CheckResult("MarkFind", await this.MarkFind(cancellationToken)))
                    {
                        measure.MeasureResult = MeasureResult.NG;
                        measure.SaveChanges();
                        return Result.FAILED;
                    }

                    // (7) 측정 데이터 처리
                    var error_x = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_X_OFFSET, out _);
                    var error_y = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_Y_OFFSET, out _);

                    MeasureData.Add(new Tuple<double, double>(error_x, error_y));

                    executeCount++;
                }
                catch (OperationCanceledException)
                {
                    logger.Information("StepMeasure canceled");
                    return Result.CANCELED;
                }
            }
        }
               

        public async Task<Result> StandingRepeatMeasure(CancellationToken cancellationToken)
        {
            // 선택된 Wafer Grid의 Row, Col 검증 Rol, Col < 0 이면 Error
            if (SelectedRow < 0 || SelectedCol < 0 || RepeatCount <= 0)
            {
                logger.Error("측정에 대한 Row, Col, Repeat 설정을 확인하십시요.");
                return Result.FAILED;
            }

            // 측정 초기화
            // (2) 측정 기준정보 조회
            var measure = this.measureVMList.Measures.Where((item) => { return item.XPos == SelectedCol && item.YPos == SelectedRow; }).First();
            var executeCount = 0;
            var result = Result.PROCESSING;
            measure.ErrorX = 0.0;
            measure.ErrorY = 0.0;

            MeasureData.Clear();

            while (true)
            {
                try
                {
                    // (1) 반복 횟수 확인
                    if (RepeatCount <= executeCount)
                    {
                        // 측정 결과 저장
                        measure.MeasureResult = MeasureResult.OK;
                        measure.ErrorX = MeasureData.Average((item) => { return item.Item1; });
                        measure.ErrorY = MeasureData.Average((item) => { return item.Item2; });
                        measure.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        measure.SaveChanges();
                        logger.Information("측정 완료");
                        return Result.SUCCESS;
                    }

                    // (3) 선택된 위치가 측정가능 한 위치인지 확인

                    if (measure.IsMeasurePoint == false)
                    {
                        logger.Information("선택된 위치는 측정 불가능한 위치입니다.");
                        return Result.FAILED;
                    }

                    result = await MovePosition(cancellationToken, SelectedRow, SelectedCol);

                    // (4) 선택된 위치로 이동
                    if (false == CheckResult("MovePosition", result))
                    {
                        measure.MeasureResult = MeasureResult.NG;
                        measure.SaveChanges();
                        return result;
                    }


                    // (5) 대기 1초 (사용자가 취소할 수 있도록 대기)
                    await Task.Delay(DelayMiliSecondsAfterMove, cancellationToken);

                    result = await this.MarkFind(cancellationToken);

                    // (6) Mark 찾기
                    if (false == CheckResult("MarkFind", result))
                    {
                        measure.MeasureResult = MeasureResult.NG;
                        measure.SaveChanges();
                        return result;
                    }

                    // (7) 측정 데이터 처리
                    var error_x = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_X_OFFSET, out _);
                    var error_y = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_Y_OFFSET, out _);

                    MeasureData.Add(new Tuple<double, double>(error_x, error_y));

                    executeCount++;
                }
                catch (OperationCanceledException)
                {
                    logger.Information("Repeat measure canceled");
                    return Result.CANCELED;
                }
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


        public async Task<Result> MovePosition(CancellationToken cancellationToken, int selectedRow, int selectedCol, int timeout_seconds = 60)
        {
            bool result = false;

            var x_direction_invert = this.dataManager.GET_INT(DataNameHelper.X_DIRECTION_INVERT, out result);
            var y_direction_invert = this.dataManager.GET_INT(DataNameHelper.Y_DIRECTION_INVERT, out result);

            CenterPositionX = this.dataManager.GET_DOUBLE(DataNameHelper.X_CENTER_POSITION, out result);
            CenterPositionY = this.dataManager.GET_DOUBLE(DataNameHelper.Y_CENTER_POSITION, out result);

            // 선택된 Wafer Grid의 Row, Col에 해당하는 Stage 좌표 계산
            var targetPositionX = CenterPositionX + ((selectedCol - CenterCol) * Pitch) * x_direction_invert;
            var targetPositionY = CenterPositionY + ((selectedRow - CenterRow) * Pitch) * y_direction_invert;

            try
            {
                var ret_x = dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEABS, targetPositionX);
                var ret_y = dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEABS, targetPositionY);


                DateTime started = DateTime.Now;

                while (ret_x && ret_y)
                {
                    await Task.Delay(100);

                    var ret_xInPos = false;
                    var ret_yInPos = false;

                    TimeSpan ellipsed = DateTime.Now - started;

                    var xInPos = dataManager.GET_INT(DataNameHelper.X_IN_INPOS, out ret_xInPos);
                    var yInPos = dataManager.GET_INT(DataNameHelper.Y_IN_INPOS, out ret_yInPos);

                    if (ret_xInPos && ret_yInPos && xInPos == (int)STATUS.ON && yInPos == (int)STATUS.ON)
                    {
                        return Result.SUCCESS;
                    }
                    else if (cancellationToken.IsCancellationRequested)
                    {
                        dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, 1);
                        dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, 1);
                        cancellationToken.ThrowIfCancellationRequested();
                        return Result.CANCELED;
                    }
                    else if (ellipsed.TotalSeconds > timeout_seconds)
                    {
                        dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, 1);
                        dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, 1);

                        logger.Error("StandingRepeatMeasure move target position timeout");
                        alarmManager.SetAlarm("E1001");
                        return Result.TIMEOUT;
                    }
                    else
                    {
                        continue;
                    }
                }

                return Result.FAILED;
            }
            catch (OperationCanceledException)
            {
                dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, 1);
                dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, 1);
                logger.Information("MovePosition canceled");
                return Result.CANCELED;
            }
        }


 
        public bool ExecuteHommingStop(AXES axisIndex)
        {
            string homeCmd;
            switch (axisIndex)
            {
                case AXES.X:
                    homeCmd = DataNameHelper.X_OUT_HOME;
                    break;
                case AXES.Y:
                    homeCmd = DataNameHelper.Y_OUT_HOME;
                    break;
                case AXES.Z1:
                    homeCmd = DataNameHelper.Z1_OUT_HOME;
                    break;
                case AXES.Z2:
                    homeCmd = DataNameHelper.Z2_OUT_HOME;
                    break;
                case AXES.Z3:
                    homeCmd = DataNameHelper.Z3_OUT_HOME;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return dataManager.SET_DATA(homeCmd, EXECUTE.STOP);
        }



        public async Task ExecuteHomming(AXES axisIndex, CancellationToken cancellationToken)
        {
            logger.Information("ExecuteHomming started");

            string homeCmd;
            string homeStatus;
            switch (axisIndex)
            {
                case AXES.X:
                    homeCmd = DataNameHelper.X_OUT_HOME;
                    homeStatus = DataNameHelper.X_IN_CALIBRATED;
                    break;
                case AXES.Y:
                    homeCmd = DataNameHelper.Y_OUT_HOME;
                    homeStatus = DataNameHelper.Y_IN_CALIBRATED;
                    break;
                case AXES.Z1:
                    homeCmd = DataNameHelper.Z1_OUT_HOME;
                    homeStatus = DataNameHelper.Z1_IN_CALIBRATED;
                    break;
                case AXES.Z2:
                    homeCmd = DataNameHelper.Z2_OUT_HOME;
                    homeStatus = DataNameHelper.Z2_IN_CALIBRATED;
                    break;
                case AXES.Z3:
                    homeCmd = DataNameHelper.Z3_OUT_HOME;
                    homeStatus = DataNameHelper.Z3_IN_CALIBRATED;
                    break;
                default:
                    throw new NotImplementedException();
            }

            await Task.Run(() =>
            {
                logger.Debug($"{axisIndex}축 Homming 시작");
                var homeCmd_result1 = dataManager.SET_DATA(homeCmd, EXECUTE.START);

                while (homeCmd_result1)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (dataManager.GET_DATA(homeStatus, out var homeStatusData) && homeStatusData is int statusData && statusData == (int)STATUS.ON)
                    {
                        homeCmd_result1 = dataManager.SET_DATA(homeCmd, STATUS.OFF);
                        logger.Debug($"{axisIndex}축 Homming 완료");
                        return;
                    }
                    logger.Debug($"{axisIndex}축 Homming 중");
                    Task.Delay(100, cancellationToken);
                }

            }, cancellationToken);

            var homeStatus_result = dataManager.GET_DATA(homeStatus, out var standby);

            standby = standby ?? STATUS.OFF;

            if (!homeStatus_result || (STATUS)standby == STATUS.ON)
            {
                logger.Debug($"{axisIndex}축 Homming 완료");
                return;
            }

            logger.Debug($"{axisIndex}축 Homming 시작");
            var homeCmd_result2 = dataManager.SET_DATA(homeCmd, EXECUTE.START);


            while (homeCmd_result2)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (dataManager.GET_DATA(homeStatus, out var homeStatusData) && homeStatusData is int statusData && statusData == (int)STATUS.ON)
                {
                    homeCmd_result2 = dataManager.SET_DATA(homeCmd, STATUS.OFF);
                    logger.Debug($"{axisIndex}축 Homming 완료");
                    return;
                }

                logger.Debug($"{axisIndex}축 Homming 중");

                //if (completedTask == timeoutTask)
                //{
                //    logger.LogWarning($"{axisIndex}축 Homming 타임아웃");
                //    return;
                //}

                await Task.Delay(100, cancellationToken);
            }
        }


        public async Task Initialize(CancellationToken cancellationToken, int centerGridRow, int centerGridCol, int gridPitch)
        {
            this.CenterCol = centerGridCol;
            this.CenterRow = centerGridRow;
            this.Pitch = gridPitch;

            await Task.CompletedTask;
        }
        /// <summary>
        /// 측정 명령 타입
        /// </summary>
        public enum MeasureCommandType
        {
            /// <summary>
            /// 초기화
            /// </summary>
            Initialize,

            /// <summary>
            /// 제자리 반복 측정
            /// </summary>
            StandingRepeatSequence,

            /// <summary>
            /// 위치 이동 반복 측정
            /// </summary>
            StepRepeatSequence
        }
    }
}
