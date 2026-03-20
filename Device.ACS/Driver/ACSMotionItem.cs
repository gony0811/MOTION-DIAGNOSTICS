using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Device.Options;
using ACS.SPiiPlusNET;
using System.Diagnostics;

namespace Device.Driver
{
    public class ACSMotionItem
    {
        public enum RepeatResultValue
        {
            Ok = 0,
            IsNotHomeDone,
            IsAlreadyRunning,
        }

        public enum MovingDirection
        {
            NEGATIVE = -1, // Minus(Negative) direction
            NOT_SELECTED, // Not selected
            POSITIVE, // Plus(Positive) direction
        }

        /// <summary>
        /// 순번
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 드라이브 알람 상태 확인 
        /// </summary>
        public bool IsAlarm { get; set; }

        public bool IsEnable { get; set; }

        /// <summary>
        /// 서보 온/오프 상태 확인
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 인포지션 범위
        /// </summary>
        public double InPositionRange { get; private set; } = 5;
        /// <summary>
        /// 지령 위치값
        /// </summary>
        public double CommandPosition { get; set; }
        /// <summary>
        /// 실제 위치값
        /// </summary>
        public double ActualPosition { get; private set; }
        /// <summary>
        /// 실제 속도
        /// </summary>
        public double ActualVelocity { get; private set; }
        /// <summary>
        /// 포지션 에러 (Following Error)
        /// </summary>
        public double PositionError { get; private set; }
        /// <summary>
        /// 인포지션 범위에 들어가는지 확인
        /// </summary>
        public bool IsInPosition { get; private set; }
        /// <summary>
        /// 드라이브 구동 중 확인
        /// </summary>
        public bool IsBusy { get; set; }
        /// <summary>
        /// 가속 중
        /// </summary>
        public bool IsAcceleration { get; private set; }
        /// <summary>
        /// 하드웨어 CW Limit 센서 감지 확인
        /// </summary>
        public bool IsHwPositiveLimit { get; private set; }
        /// <summary>
        /// 하드웨어 CCW Limit 센서 감지 확인
        /// </summary>
        public bool IsHwNegativeLimit { get; private set; }
        /// <summary>
        /// 소프트웨어 CW Limit 센서 감지 확인
        /// </summary>
        public bool IsSwPositiveLimit { get; private set; }
        /// <summary>
        /// 소프트웨어 CCW Limit 센서 감지 확인
        /// </summary>
        public bool IsSwNegativeLimit { get; private set; }
        /// <summary>
        /// +방향
        /// </summary>
        public MovingDirection MoveDirection { get; private set; }
        /// <summary>
        /// 반복 탈출 플래그
        /// </summary>
        public bool IsRepeatTerminated { get; private set; }
        /// <summary>
        /// 반복할 수량
        /// </summary>
        public int RepeatCount { get; set; }
        /// <summary>
        /// 반복 진행 횟수
        /// </summary>
        public int RepeatIndex { get; set; }

        public double FeedbackPosition { get; set; }

        /// <summary>
        /// 현재 속도
        /// </summary>
        public double CurrentVelocity { get; private set; }

        /// <summary>
        /// Unit에 해당하는 단위를 맞추기 위한 값 (1mm 당 이동 값1000, 1도 당 이동 값)
        /// </summary>
        public double UnitFactor { get; set; }

        /// <summary>
        /// 알람 해제 후 서보 온 하기 전 딜레이 시간
        /// </summary>
        public double AlarmClearTimeFromSeconds { get; set; } = 0.5;
        /// <summary>
        /// 서보 온 후 딜레이 시간
        /// </summary>
        public double EnableTimeFromSeconds { get; set; } = 0.5;

        /// <summary>
        /// 움직이기 전 인터락 체크 함수
        /// </summary>
        public Func<int, bool> MovingInterlockPassMethod;
        /// <summary>
        /// 홈 하기 전 인터락 체크
        /// </summary>
        public Func<int, bool> HomingInterlockPassMethod;


        /// <summary>
        /// 홈 옵셋
        /// </summary>
        public double HomeOffset { get; set; }
        /// <summary>
        /// 홈 진행 스텝
        /// </summary>
        public int HomeStep { get; set; }

        /// <summary>
        /// 제어기 내부 홈 완료 체크
        /// </summary>
        public bool IsInternalHomeDone { get; set; }

        /// <summary>
        /// 실제 최종 홈 완료 확인
        /// </summary>
        public bool IsHomeDone { get; set; }
        /// <summary>
        /// 홈 시작 체크
        /// </summary>
        public bool IsHomeStart { get; set; }
        /// <summary>
        /// 홈 동작 임의 중지 상태 확인
        /// </summary>
        public bool IsHomeTerminated { get; set; }
        public bool IsHomeTimeoutError { get; set; }
        public bool IsHomeFaultError { get; set; }
        public bool IsHomeEnableError { get; set; }

        public double CompensationOffsetValue { get; set; }
        public bool IsJogTerminated { get; set; }
        public bool IsNegativeSwLimitEnabled { get; set; }
        public double NegativeSwLimitValue { get; set; }
        public bool IsPositiveSwLimitEnabled { get; set; }
        public double PositiveSwLimitValue { get; set; }

        /// <summary>
        /// 이동 목표 위치
        /// </summary>
        public double TargetPosition { get; set; }
        /// <summary>
        /// 에러 메시지
        /// </summary>
        public string ErrorMessage { get; private set; }

        public ACSMotionItem(ACSDriver acsDriver, AxisParameter axisParam, int index)
        {
            this.acsDriver = acsDriver;
            this.Index = index;
            this.axisParameter = axisParam;
            this.UnitFactor = axisParam.UnitFactor;

            monitoringThread = new Thread(() =>
            {
                while (isMonitoringTerminated == false)
                {
                    //if (!Inno6AcsMotion.IsSimulationMode)
                    GetAllMotorState();

                    Thread.Sleep(TimeSpan.FromMilliseconds(20));
                }
            });

            monitoringThread.IsBackground = true;
            monitoringThread.Start();
        }

        public void Dispose()
        {
            isMonitoringTerminated = true;
            monitoringThread.Join();
        }

        private void GetAllMotorState()
        {
            if (acsDriver == null)
                return;

            if (acsDriver.Api == null)
                return;

            MotorStates state = acsDriver.Api.GetMotorState((Axis)Index);

            IsEnable = IsEnabled = state.HasFlag(MotorStates.ACSC_MST_ENABLE);
            IsInPosition = state.HasFlag(MotorStates.ACSC_MST_INPOS);
            IsBusy = state.HasFlag(MotorStates.ACSC_MST_MOVE);
            IsAcceleration = state.HasFlag(MotorStates.ACSC_MST_ACC);

            SafetyControlMasks safetyControlMasks = acsDriver.Api.GetFault((Axis)Index);
            IsHwNegativeLimit = safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_LL) || safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_SLL);
            IsHwPositiveLimit = safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_RL);

            _GetPosition();
            _GetFault();
        }

        /// <summary>
        /// 위치값 읽기
        /// </summary>
        private void _GetPosition()
        {
            //ActualPosition = Inno6AcsMotion.Ch.GetFPosition((Axis)Index);
            //CommandPosition = Inno6AcsMotion.Ch.GetRPosition((Axis)Index);

            if (!acsDriver.IsSimulationMode)
            {

                // 2024.10.02 S_FAULT ERROR로 변경
                FeedbackPosition = ActualPosition = (double)acsDriver.ReadVariable($"APOS", -1, Index, Index, 0, 0) / UnitFactor; //Inno6AcsMotion.Ch.GetFPosition((Axis)Index);
                CommandPosition = (double)acsDriver.ReadVariable($"APOS", -1, Index, Index, 0, 0) / UnitFactor; //Inno6AcsMotion.Ch.GetRPosition((Axis)Index);    

                //FeedbackPosition = ActualPosition = Inno6AcsMotion.Ch.GetFPosition((Axis)Index) / UnitFactor;
                //CommandPosition = Inno6AcsMotion.Ch.GetRPosition((Axis)Index) / UnitFactor;

                CurrentVelocity = acsDriver.Api.GetVelocity((Axis)Index) / UnitFactor;
                ActualVelocity = acsDriver.Api.GetFVelocity((Axis)Index) / UnitFactor;
                PositionError = CommandPosition - ActualPosition;
            }
            else
            {
                FeedbackPosition = ActualPosition = acsDriver.Api.GetFPosition((Axis)Index) / UnitFactor;
                CommandPosition = acsDriver.Api.GetRPosition((Axis)Index) / UnitFactor;
            }
        }


        /// <summary>
        /// 프로그램 상태 읽기
        /// </summary>
        /// <param name="index"></param>
        private void _GetProgramState(int index)
        {
            ProgramStates programStates = acsDriver.Api.GetProgramState((ProgramBuffer)index);
        }

        private bool _StartHome(bool isWaitable = true, int timeout = timeoutFromSeconds)
        {
            acsDriver.WriteVariable("HomeFlag", -1, -1, 0, 0, Index, Index);
            acsDriver.RunBuffer(axisParameter.HomeBufferNumber, null);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (isWaitable == true)
            {
                bool isTimeout = false;
                bool isHomeDone = false;

                DateTime startTime = DateTime.Now;
                TimeSpan timeoutSpan = new TimeSpan(0, 0, 0, axisParameter.HomeTimeout/*kTimeoutFromSeconds*/); // 1분

                do
                {
                    var isHomeCompleted = acsDriver.ReadVariable("HomeFlag", -1, 0, 0, Index, Index);
                    isTimeout = (DateTime.Now.Subtract(timeoutSpan) >= startTime);

                    if ((int)isHomeCompleted >= 1)
                    {
                        isHomeDone = true;
                        break;
                    }

                    // todo: 시스템 abort 처리
                    //if (JmpSystemController.Instance.IsInitialStopByUser == true)
                    //{
                    //    Inno6AcsMotion.StopBuffer(HomeBufferNumber);
                    //    //Stop();
                    //    break;
                    //}

                    Thread.Sleep(TimeSpan.FromMilliseconds(1));
                }
                while (isTimeout == false && isHomeDone == false);

                if (isTimeout == true)
                {
                    acsDriver.StopBuffer(axisParameter.HomeBufferNumber);
                    //Stop();

                    if (IsAlarm == true)
                    {
                        IsHomeFaultError = true;
                    }
                    else
                    {
                        IsHomeTimeoutError = true;
                    }
                }

                IsInternalHomeDone = isHomeDone;

                return isHomeDone;
            }
            else
            {
                return true;
            }
        }



        /// <summary>
        /// 에러 읽기
        /// </summary>
        /// <returns></returns>
        private bool _GetFault()
        {
            SafetyControlMasks safetyControlMasks = acsDriver.Api.GetFault((Axis)Index);
            if (safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_AL) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_CL) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_CPE) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_DRIVE) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_ENC) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_ENC2) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_ENC2NC) == true ||
                //safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_ENCNC) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_ES) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_EXTNT) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_FAILURE) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_HOT) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_HSSINC) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_INT) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_INTGR) == true ||
                //safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_LL) == true || // Negative Limit
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_MEM) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_NETWORK) == true ||
                //safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_PE) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_PROG) == true ||
                //safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_RL) == true || // Positive Limit
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_SLL) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_SP) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_SRL) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_STO) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_TEMP) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_TIME) == true ||
                safetyControlMasks.HasFlag(SafetyControlMasks.ACSC_SAFETY_VL) == true)
            {
                IsAlarm = true;
            }
            else
            {
                IsAlarm = false;
            }

            return IsAlarm;
        }

        /// <summary>
        /// 홈 사이클 동작
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private int _ExecuteHoming(object obj)
        {
            while (!IsHomeTerminated)
            {
                switch (HomeStep)
                {
                    case 1: // 알람 해제
                        GetAllMotorState();

                        if (IsAlarm == true)
                        {
                            ClearFault();
                            Thread.Sleep(TimeSpan.FromSeconds(AlarmClearTimeFromSeconds));
                        }

                        if (IsNegativeSwLimitEnabled) ResetNegativeLimit();
                        if (IsPositiveSwLimitEnabled) ResetPositiveLimit();

                        Thread.Sleep(TimeSpan.FromSeconds(1));

                        ++HomeStep;
                        break;

                    case 2: // 서보 온
                        GetAllMotorState();

                        if (IsAlarm == true)
                        {
                            HomeStep = 0;
                            IsHomeFaultError = true;
                            IsHomeTerminated = true;
                            IsHomeStart = false;
                            Stop();
                            break;
                        }

                        if (IsEnabled == false)
                        {
                            Enable();
                            Thread.Sleep(TimeSpan.FromSeconds(EnableTimeFromSeconds));
                        }

                        ++HomeStep;
                        break;

                    case 3:
                        if (acsDriver.IsSimulationMode == false)
                        {
                            GetAllMotorState();

                            if (IsEnabled == false)
                            {
                                HomeStep = 0;
                                IsHomeEnableError = true;
                                IsHomeTerminated = true;
                                IsHomeStart = false;
                                Stop();
                                break;
                            }

                            if (_StartHome() == true)
                            {
                                ++HomeStep;
                            }
                            else
                            {
                                HomeStep = 0;
                                IsHomeTerminated = true;
                                IsHomeStart = false;
                            }
                        }
                        else
                        {
                            ResetCount();
                            ++HomeStep;
                        }
                        break;

                    case 4:
                        SetSpeed(SpeedMode.Maint);
                        MoveRel(HomeOffset);
                        ++HomeStep;
                        break;

                    case 5:
                        if (!CheckMoveDone())
                            break;

                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        ResetCount();
                        ++HomeStep;
                        break;

                    case 6:
                        Thread.Sleep(TimeSpan.FromSeconds(1));

                        if (IsNegativeSwLimitEnabled)
                        {
                            SetNegativeLimit(NegativeSwLimitValue);
                        }

                        Thread.Sleep(100);

                        if (IsPositiveSwLimitEnabled)
                        {
                            SetPositiveLimit(PositiveSwLimitValue);
                        }

                        Thread.Sleep(100);

                        ResetCount();

                        HomeStep = 0;
                        IsHomeTerminated = true;
                        IsHomeStart = false;
                        IsHomeDone = true;
                        break;
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }


            return 1;
        }

        /// <summary>
        /// 서보 온
        /// </summary>
        public void Enable()
        {
            acsDriver.Api.Enable((Axis)Index);
        }

        /// <summary>
        /// 서보 오프
        /// </summary>
        public void Disable()
        {
            acsDriver.Api.Disable((Axis)Index);
        }



        /// <summary>
        /// 드라이브 알람 해제
        /// </summary>
        public void ClearFault()
        {
            acsDriver.Api.FaultClearAsync((Axis)Index);
        }

        public void SetServoOn(bool isOn)
        {
            if (isOn)
                acsDriver.Api.Enable((Axis)Index);
            else
            {
                acsDriver.Api.Disable((Axis)Index);
            }
        }

        public void ResetAlarm()
        {
            acsDriver.Api.FaultClearAsync((Axis)Index);
        }

        public void StartHome()
        {
            StartHoming();
        }
        public void SetSpeed(SpeedMode speedMode)
        {
            switch (speedMode)
            {
                case SpeedMode.Jog:
                    SetSpeed(axisParameter.JogVelocity, axisParameter.JogAcceleration, axisParameter.JogDeceleration);
                    break;

                case SpeedMode.Maint:
                    SetSpeed(axisParameter.MaintVelocity, axisParameter.MaintAcceleration, axisParameter.MaintDeceleration);
                    break;

                case SpeedMode.Run:     
                    SetSpeed(axisParameter.RunVelocity, axisParameter.RunAcceleration, axisParameter.RunDeceleration);
                    break;
            }
        }

        /// <summary>
        /// Referecne & Feedback 위치 클리어
        /// </summary>
        public void ResetCount()
        {
            acsDriver.Api.SetRPositionAsync((Axis)Index, 0.0);
            acsDriver.Api.SetFPositionAsync((Axis)Index, 0.0);
        }

        public void SetNegativeLimit(double negativeLimitValue)
        {
            //if (negativeLimitValue > 0)
            //{
            //    return;
            //}

            acsDriver.Api.Command($"FDEF({Index}).#SLL=1");

            acsDriver.Api.Command($"SLLIMIT({Index})={negativeLimitValue * UnitFactor}");

            SafetyControlMasks safetyControlMasks = acsDriver.Api.GetFaultMask((Axis)Index);
            acsDriver.Api.SetFaultMask((Axis)Index, safetyControlMasks | SafetyControlMasks.ACSC_SAFETY_SLL);

            IsNegativeSwLimitEnabled = true;
            NegativeSwLimitValue = negativeLimitValue;
        }

        public void ResetNegativeLimit()
        {
            acsDriver.Api.Command($"FDEF({Index}).#SLL=0");

            SafetyControlMasks safetyControlMasks = acsDriver.Api.GetFaultMask((Axis)Index);
            acsDriver.Api.SetFaultMask((Axis)Index, safetyControlMasks & ~SafetyControlMasks.ACSC_SAFETY_SLL);
        }

        public void SetPositiveLimit(double positiveLimitValue)
        {
            //if (positiveLimitValue < 0)
            //{
            //    return;
            //}
            acsDriver.Api.Command($"FDEF({Index}).#SRL=1");

            acsDriver.Api.Command($"SRLIMIT({Index})={positiveLimitValue * UnitFactor}");

            SafetyControlMasks safetyControlMasks = acsDriver.Api.GetFaultMask((Axis)Index);
            acsDriver.Api.SetFaultMask((Axis)Index, safetyControlMasks | SafetyControlMasks.ACSC_SAFETY_SRL);

            IsPositiveSwLimitEnabled = true;
            PositiveSwLimitValue = positiveLimitValue;
        }

        public void ResetPositiveLimit()
        {
            acsDriver.Api.Command($"FDEF({Index}).#SRL=0");

            SafetyControlMasks safetyControlMasks = acsDriver.Api.GetFaultMask((Axis)Index);
            acsDriver.Api.SetFaultMask((Axis)Index, safetyControlMasks & ~SafetyControlMasks.ACSC_SAFETY_SRL);
        }

        public void SetSpeed(double velocity, double acceleration, double deceleration)
        {
            SetSpeed(velocity, acceleration, deceleration, acceleration * 10);
        }

        /// <summary>
        /// 속도 변경
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="deceleration"></param>
        /// <param name="jerk"></param>
        public void SetSpeed(double velocity, double acceleration, double deceleration, double jerk)
        {
            acsDriver.Api.SetVelocityAsync((Axis)Index, velocity * UnitFactor);
            acsDriver.Api.SetAccelerationAsync((Axis)Index, acceleration * UnitFactor);
            acsDriver.Api.SetDecelerationAsync((Axis)Index, deceleration * UnitFactor);
            double jerkValue = .0;
            if (Index == 0 || Index == 1 || Index == 2)
                jerkValue = 500;
            else
                jerkValue = jerk;
            acsDriver.Api.SetJerkAsync((Axis)Index, jerkValue * UnitFactor);
        }

        public void MoveAbs(double position)
        {
            MoveAbs(position, false);
        }

        public void MoveAbs(double position, double velocity, double acceleration, double deceleration)
        {
            SetSpeed(velocity, acceleration, deceleration);
            MoveAbs(position, false);
        }

        public void MoveRel(double position)
        {
            MoveRel(position, false);
        }

        public void MoveRel(double position, double velocity, int accelerationTime, int sCurveTime)
        {
            //throw new NotImplementedException();
        }

        public void MoveJog(double velocity)
        {
            MoveJog(velocity, axisParameter.JogAcceleration, axisParameter.JogDeceleration);
        }

        public void MoveJog(double velocity, int accelerationTime, int sCurveTime)
        {
            //throw new NotImplementedException();
        }

        public void MoveJog(double velocity, double acceleration, double deceleration)
        {
            MoveJog(velocity, acceleration, deceleration, acceleration * 10);
        }

        /// <summary>
        /// 홈 시작
        /// </summary>
        /// <returns></returns>
        private void StartHoming()
        {
            if (HomingInterlockPassMethod != null &&
                !HomingInterlockPassMethod(Index))
            {
                return;
            }

            if (axisParameter.HomeBufferNumber < 0)
            {
                return;
            }

            if (HomeStep > 0)
            {
                return;
            }

            IsInternalHomeDone = false;
            HomeStep = 1;
            IsHomeDone = false;
            IsHomeStart = true;
            IsHomeTerminated = false;
            IsHomeTimeoutError = false;
            IsHomeFaultError = false;
            IsHomeEnableError = false;

            Task.Factory.StartNew(_ExecuteHoming, null);
        }

        /// <summary>
        /// 조그 이동
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="deceleration"></param>
        /// <param name="jerk"></param>
        public void MoveJog(double velocity, double acceleration, double deceleration, double jerk)
        {
            MoveDirection = velocity > 0 ? MovingDirection.POSITIVE : MovingDirection.NEGATIVE;

            //if (MovingInterlockPassMethod != null &
            //    !MovingInterlockPassMethod(Index))
            //{
            //    return;
            //}

            MotorStates state = MotorStates.ACSC_NONE;
            state = acsDriver.Api.GetMotorState((Axis)Index);
            IsBusy = state.HasFlag(MotorStates.ACSC_MST_MOVE);

            if (IsBusy == true)
            {
                return;
            }

            acsDriver.Api.SetVelocityAsync((Axis)Index, velocity * UnitFactor);
            acsDriver.Api.SetAccelerationAsync((Axis)Index, acceleration * UnitFactor);
            acsDriver.Api.SetDecelerationAsync((Axis)Index, deceleration * UnitFactor);
            double jerkValue = .0;
            if (Index == 0 || Index == 1 || Index == 2)
                jerkValue = 500;
            else
                jerkValue = jerk;
            acsDriver.Api.SetJerkAsync((Axis)Index, jerkValue * UnitFactor);
            acsDriver.Api.JogAsync(MotionFlags.ACSC_NONE, (Axis)Index, velocity * UnitFactor);
        }

        public bool MoveJog(double velocity, double acceleration, Func<bool> func)
        {
            MoveDirection = velocity > 0 ? MovingDirection.POSITIVE : MovingDirection.NEGATIVE;

            //if (MovingInterlockPassMethod != null &
            //    !MovingInterlockPassMethod(Index))
            //{
            //    return;
            //}

            MotorStates state = MotorStates.ACSC_NONE;
            state = acsDriver.Api.GetMotorState((Axis)Index);
            IsBusy = state.HasFlag(MotorStates.ACSC_MST_MOVE);

            if (IsBusy == true)
            {
                return false;
            }

            bool inposition = false;
            double inpositionValue = 0;
            bool moving = false;
            bool timeoutOccured = false;

            acsDriver.Api.SetVelocityAsync((Axis)Index, velocity * UnitFactor);
            acsDriver.Api.SetAccelerationAsync((Axis)Index, acceleration * UnitFactor);
            acsDriver.Api.SetDecelerationAsync((Axis)Index, acceleration * UnitFactor);

            double jerkValue = .0;
            if (Index == 0 || Index == 1 || Index == 2)
                jerkValue = 500;
            else
                jerkValue = acceleration * 10;

            acsDriver.Api.SetJerkAsync((Axis)Index, jerkValue * UnitFactor);
            acsDriver.Api.JogAsync(MotionFlags.ACSC_NONE, (Axis)Index, velocity * UnitFactor);

            if (func != null)
            {
                TimeSpan timeoutDuration = new TimeSpan(0, 0, 5, 0);
                DateTime startTime = DateTime.Now;

                do
                {
                    if (IsJogTerminated)
                        break;

                    timeoutOccured = (DateTime.Now.Subtract(timeoutDuration) >= startTime);
                    Thread.Sleep(TimeSpan.FromMilliseconds(10));
                } while (func() == false && timeoutOccured == false);

                Stop();

                if (timeoutOccured || IsJogTerminated)
                {
                    if (timeoutOccured == true)
                    {
                        ErrorMessage = $"{Name}[{Index + 1}] IsTimeoutOccured Error";
                    }
                    else if (IsJogTerminated)
                    {
                        ErrorMessage = $"{Name}[{Index + 1}] IsJogTerminated";
                    }
                    return false;
                }
                else
                {
                    ErrorMessage = "";
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void SetSpeed(double velocity)
        {
            double acceleration = velocity * 3;
            double deceleration = velocity * 3;
            double jerk = velocity * 30;
            acsDriver.Api.SetVelocityAsync((Axis)Index, velocity * UnitFactor);
            acsDriver.Api.SetAccelerationAsync((Axis)Index, acceleration * UnitFactor);
            acsDriver.Api.SetDecelerationAsync((Axis)Index, deceleration * UnitFactor);
            double jerkValue = .0;
            if (Index == 0 || Index == 1 || Index == 2)
                jerkValue = 500;
            else
                jerkValue = jerk;
            acsDriver.Api.SetJerkAsync((Axis)Index, jerkValue * UnitFactor);
        }

        /// <summary>
        /// 상대 위치 이동
        /// </summary>
        /// <param name="position"></param>
        /// <param name="waitMoveDone"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool MoveRel(double position, bool waitMoveDone = true, int timeout = timeoutFromSeconds)
        {
            MotorStates state = MotorStates.ACSC_NONE;
            //state = Inno6AcsMotion.Ch.GetMotorState((Axis)Index);
            //IsBusy = state.HasFlag(MotorStates.ACSC_MST_MOVE);

            //if (IsBusy == true)
            //{
            //    return false;
            //}

            string errorMessage = string.Empty;
            double currentPosition = 0.0;
            double targetPosition = 0.0;

            bool inposition = false;
            double inpositionValue = 0;
            bool moving = false;
            bool timeoutOccured = false;

            currentPosition = acsDriver.Api.GetFPosition((Axis)Index) / UnitFactor;
            targetPosition = currentPosition + position;

            TargetPosition = targetPosition;

            MoveDirection = targetPosition - currentPosition > 0 ? MovingDirection.POSITIVE : MovingDirection.NEGATIVE;

            //if (MovingInterlockPassMethod != null &
            //    !MovingInterlockPassMethod(Index))
            //{
            //    return false;
            //}

            acsDriver.Api.ToPointAsync(MotionFlags.ACSC_AMF_RELATIVE, (Axis)Index, position * UnitFactor);

            if (waitMoveDone == true)
            {
                TimeSpan timeoutDuration = new TimeSpan(0, 0, 0, timeout);
                DateTime startTime = DateTime.Now;

                do
                {
                    state = acsDriver.Api.GetMotorState((Axis)Index);
                    inposition = Convert.ToBoolean(state & MotorStates.ACSC_MST_INPOS);
                    moving = Convert.ToBoolean(state & MotorStates.ACSC_MST_MOVE);
                    timeoutOccured = DateTime.Now.Subtract(timeoutDuration) >= startTime;

                    Thread.Sleep(0);
                } while ((inposition == false || moving == true) && timeoutOccured == false);

                inpositionValue = Math.Abs(targetPosition - acsDriver.Api.GetFPosition((Axis)Index) / UnitFactor);
                return inposition == true && moving == false && timeoutOccured == false && inpositionValue <= InPositionRange;
            }
            else
            {
                return true;//return (true, 0, string.Empty);                
            }
        }

        /// <summary>
        /// 절대 위치 이동
        /// </summary>
        /// <param name="position"></param>
        /// <param name="waitMoveDone"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool MoveAbs(double position, bool waitMoveDone = true, int timeout = timeoutFromSeconds)
        {
            MotorStates state = MotorStates.ACSC_NONE;
            //state = Inno6AcsMotion.Ch.GetMotorState((Axis)Index);
            //IsBusy = state.HasFlag(MotorStates.ACSC_MST_MOVE);

            //if (IsBusy == true)
            //{
            //    return false;
            //}

            string errorMessage = string.Empty;
            double currentPosition = 0.0;
            double targetPosition = 0.0;

            bool inposition = false;
            double inpositionValue = 0;
            bool moving = false;
            bool timeoutOccured = false;

            currentPosition = acsDriver.Api.GetFPosition((Axis)Index) / UnitFactor;
            targetPosition = position;
            TargetPosition = position;
            MoveDirection = targetPosition - currentPosition > 0 ? MovingDirection.POSITIVE : MovingDirection.NEGATIVE;

            //if (MovingInterlockPassMethod != null &
            //    !MovingInterlockPassMethod(Index))
            //{
            //    return false;
            //}

            // 알람
            //if (Alarm)
            //{
            //    return false;
            //}
            // 서보 온
            // 인터락 함수 체크
            // 방향 플레그 지정

            acsDriver.Api.ToPointAsync(MotionFlags.ACSC_NONE, (Axis)Index, position * UnitFactor);

            if (waitMoveDone == true)
            {
                TimeSpan timeoutDuration = new TimeSpan(0, 0, 0, timeout);
                DateTime startTime = DateTime.Now;

                do
                {
                    state = acsDriver.Api.GetMotorState((Axis)Index);
                    inposition = Convert.ToBoolean(state & MotorStates.ACSC_MST_INPOS);
                    moving = Convert.ToBoolean(state & MotorStates.ACSC_MST_MOVE);
                    timeoutOccured = DateTime.Now.Subtract(timeoutDuration) >= startTime;

                    // todo: 시스템 abort처리
                    //if (JmpSystemController.Instance.IsInitialStopByUser)
                    //{
                    //    break;
                    //}

                    Thread.Sleep(1);
                } while ((inposition == false || moving == true) && timeoutOccured == false);

                //if (JmpSystemController.Instance.IsInitialStopByUser)
                //{
                //    ErrorMessage = "Stop By User.";
                //    Stop();
                //    return false;
                //}
                //else
                //{
                if (timeoutOccured)
                {
                    ErrorMessage = "Time is up.";
                    Stop();
                    return false;
                }
                else
                {
                    inpositionValue = Math.Abs(targetPosition - acsDriver.Api.GetFPosition((Axis)Index) / UnitFactor);
                    return inposition == true && moving == false && timeoutOccured == false && inpositionValue <= InPositionRange;
                }
                //}
            }
            else
            {
                return true;
            }
        }

        public bool CheckMoveDone()
        {
            MotorStates state = MotorStates.ACSC_NONE;
            state = acsDriver.Api.GetMotorState((Axis)Index);
            bool inposition = Convert.ToBoolean(state & MotorStates.ACSC_MST_INPOS);
            bool moving = Convert.ToBoolean(state & MotorStates.ACSC_MST_MOVE);

            if (inposition == true &&
                moving == false &&
                TargetPosition >= (ActualPosition - 0.005) &&
                TargetPosition <= (ActualPosition + 0.005))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 모션 정지
        /// </summary>
        public void Stop()
        {
            acsDriver.Api.StopBuffer((ProgramBuffer)axisParameter.HomeBufferNumber);
            acsDriver.Api.HaltAsync((Axis)Index);

            Stopwatch.Stop();
            Stopwatch.Reset();

            if (IsHomeStart)
            {
                IsHomeStart = false;
                IsHomeTerminated = true;
                HomeStep = 0;
            }
        }

        private ACSDriver acsDriver;
        private AxisParameter axisParameter;
        private Thread monitoringThread;
        private bool isMonitoringTerminated = false;
        private const int timeoutFromSeconds = 600;
        private Stopwatch Stopwatch = new Stopwatch();

        /// <summary>
        /// 스피드 설정 모드
        /// Jog : Low
        /// Maint : Middle
        /// Run : High
        /// </summary>
        public enum SpeedMode
        {
            Jog,
            Maint,
            Run,
        }

    }
}
