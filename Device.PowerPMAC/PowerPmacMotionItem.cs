using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Device
{
    internal class PowerPmacMotionItem
    {
        #region Variables
        private uint mIndex;                                                    // 축 인덱스 (Key value)
        private string mName;                                                   // 축 이름
        private double mEncoderCountsPerUnit;                                   // CountsPerUnit (cts/mm) : 단위 길이(mm) 당 Encoder count수
        private HomeExecutionMode mHomeExecutionMode = HomeExecutionMode.PLC;   // Home execution mode (PLC or PC)
        private uint mHomeExucutionStartPlcNumber;                              // Home execution plc number e.g)ENA PLC 5
        private uint mHomeStep;                                                 // Home Step
        private uint mHomeCompletePVarNumber;                                   // P501 ? P601 ?...
        private double mCommandPosition;                                        // Command Position
        private double mFeedbackPosition;                                       // Feedback Position
        private double mMovingVelocity;                                         // Moving Velocity    
        private double mFollowingError;                                         // Following Error    
        private bool mIsBusy;                                                   // Desired Velocity 0 is not active, inposition is not active
        private bool mIsEnabled;                                                // Amplifier enabled.
        private bool mIsAmpFault;                                               // Amplifier fault.     
        private bool mInpostion;                                                // Inposition Check     
        private bool mIsHomeSensorDetected;                                     // Home Sensor Detected.

        private bool mIsCwLimitSensorDetected;
        private bool mIsCCwLimitSensorDetected;
        private bool mIsInternalHomeDone;
        private bool mIsHomeDone;
        private bool misMotionDone;
        private UInt32 mDeviceId = 0;  // 선택창에서 리턴되는 값을 사용하지 않아서 고정 값으로 처리함.
        private IntPtr mResponse;
        private const int kMaxChar = 10240;
        private const double kInpositionRange = 0.10;
        public delegate bool MovingPreCheckDelegate();
        private MovingPreCheckDelegate mMovingPreCheckDelegate;
        public MotorStatusYBitField mMotorStatusY = new MotorStatusYBitField();
        public MotorStatusXBitField mMotorStatusX = new MotorStatusXBitField();
        protected Stopwatch mDelayStopwatch = new Stopwatch();
        protected Stopwatch mTimeoutStopwatch = new Stopwatch();
        private const uint kHomeTimeout = 100_000 * 500;
        #endregion

        #region Properties
        public uint Index
        {
            get
            {
                return mIndex;
            }
            set
            {
                mIndex = value;
            }
        }

        public bool VirtualAxisUsing { get; set; }

        public uint VirtualAxisNumber { get; set; }

        public uint VirtualRealAxisFirst { get; set; }

        public uint VirtualRealAxisSecond { get; set; }

        public bool DirectPWMControlUsing { get; set; }

        public string Name
        {
            set { mName = value; }
            get { return mName; }
        }
        public double EncoderCountsPerUnit // e.g) #1->1000X
        {
            get
            {
                return mEncoderCountsPerUnit;
            }
            set
            {
                mEncoderCountsPerUnit = value;
            }
        }

        public HomeExecutionMode HomeExecutionMode
        {
            get
            {
                return mHomeExecutionMode;
            }
            set
            {
                mHomeExecutionMode = value;
            }
        }

        public uint HomeExucutionStartPlcNumber
        {
            get
            {
                return mHomeExucutionStartPlcNumber;
            }
            set
            {
                mHomeExucutionStartPlcNumber = value;
            }
        }
        public uint HomeStep
        {
            get { return mHomeStep; }
        }
        public uint HomeCompletePVarNumber
        {
            get
            {
                return mHomeCompletePVarNumber;
            }
            set
            {
                mHomeCompletePVarNumber = value;
            }
        }
        public double CommandPosition
        {
            get { return mCommandPosition; }
        }
        public double FeedbackPosition
        {
            get { return mFeedbackPosition; }
        }
        public double MovingVelocity
        {
            get { return mMovingVelocity; }
        }
        public double FollowingError
        {
            get { return mFollowingError; }
        }
        public bool Busy
        {
            get { return mIsBusy; }
        }
        public bool Enabled
        {
            get { return mIsEnabled; }
        }
        public bool AmpFault
        {
            get { return mIsAmpFault; }
        }

        public bool Inposition
        {
            get { return mInpostion; }
        }
        public bool HomeSensorDetection
        {
            get { return mIsHomeSensorDetected; }
        }
        public bool CwLimitSensorDetection
        {
            get { return mIsCwLimitSensorDetected; }
        }
        public bool CCwLimitSensorDetection
        {
            get { return mIsCCwLimitSensorDetected; }
        }
        public bool InternalHomeDone
        {
            get { return mIsInternalHomeDone; }
        }
        public bool IsHomeDone
        {
            get { return mIsHomeDone; }
            set { mIsHomeDone = value; }
        }
        public bool IsMotionDone
        {
            get { return misMotionDone; }
            set { misMotionDone = value; }
        }

        /// <summary>
        /// PPMAC 홈 상태 체크 용 문자열
        /// </summary>
        public string HomingStateString { get; set; }

        /// <summary>
        /// PPMAC 홈 완료 체크 용 문자열
        /// </summary>
        public string HomingCompleteString { get; set; }

        /// <summary>
        /// 홈 완료 타임아웃
        /// </summary>
        public int HomeTimeout { get; set; }

        #endregion

        public void SetIndex(uint index)
        {
            mIndex = index;
        }
        public void SetName(string name)
        {
            mName = name;
        }
        public void SetEncoderCountsPerUnit(double encoderCountsPerUnit)
        {
            mEncoderCountsPerUnit = encoderCountsPerUnit;
        }
        public void SetHomeExecutionMode(HomeExecutionMode homeExecutionMode)
        {
            mHomeExecutionMode = homeExecutionMode;
        }
        public void SetHomeExecutionStartPlcNumber(uint homeExecutionStartPlcNumber)
        {
            mHomeExucutionStartPlcNumber = homeExecutionStartPlcNumber;
        }
        public void SetHomeStep(uint homeStep)
        {
            mHomeStep = homeStep;
        }
        public void SetHomeCompletePVarNumber(uint homeCompletePVarNumber)
        {
            mHomeCompletePVarNumber = homeCompletePVarNumber;
        }
        public void SetCommandPosition(double commandPosition)
        {
            mCommandPosition = commandPosition;
        }
        public void SetFeedbackPosition(double feedbackPosition)
        {
            mFeedbackPosition = feedbackPosition;
        }
        public void SetCurrentMovingVelocity(double currentMovingVelocity)
        {
            mMovingVelocity = currentMovingVelocity;
        }
        public void SetFollowingError(double followingError)
        {
            mFollowingError = followingError;
        }
        public void SetBusy(bool busy)
        {
            mIsBusy = busy;
        }
        public void SetEnable(bool enabled)
        {
            mIsEnabled = enabled;
        }
        public void SetAmpFault(bool ampFault)
        {
            mIsAmpFault = ampFault;
        }
        public void SetInposition(bool Inposition)
        {
            mInpostion = Inposition;
        }

        public void SetHomeSensorDetection(bool homeSensorDetection)
        {
            mIsHomeSensorDetected = homeSensorDetection;
        }
        public void SetCwLimitSensorDetection(bool cwLimitSensorDetection)
        {
            mIsCwLimitSensorDetected = cwLimitSensorDetection;
        }
        public void SetCCwLimitSensorDetection(bool ccwLimitSensorDetection)
        {
            mIsCCwLimitSensorDetected = ccwLimitSensorDetection;
        }
        public void SetInternalHomeDone(bool internalHomeDone)
        {
            mIsInternalHomeDone = internalHomeDone;
        }

        public void SetPreCheckFunction(MovingPreCheckDelegate preCheckFunction)
        {
            mMovingPreCheckDelegate = preCheckFunction;
        }

        public void GetStatus()
        {

        }


        public void Enable(bool enable)
        {
            if (!enable)
            {
                mIsHomeDone = false;
            }

            string temp = string.Empty;

            if (enable)
            {
                temp = string.Format("#{0}J/", mIndex);
            }
            else
            {
                temp = string.Format("#{0}K", mIndex);
            }


            PMAC.PMacCommand(temp);
        }

        public bool StartHoming()
        {
            //string temp = string.Empty;
            //temp = string.Format("ENABLE PLC {0:D}", mHomeExucutionStartPlcNumber);
            //PMAC.PMacCommand(temp);
            if (mHomeStep > 0)
                return false;

            mIsHomeDone = false;
            mHomeStep = 1;
            return true;
        }

        public bool StopHoming()
        {
            if (mHomeStep <= 0)
                return false;

            string temp = string.Format("DISABLE PLC {0:D}", mHomeExucutionStartPlcNumber);
            PMAC.PMacCommand(temp);

            return true;
        }

        public void ExecuteHoming()
        {
            bool homeDone;
            string temp = string.Empty;
            IntPtr result;
            switch (mHomeStep)
            {
                case 1:
                    if (mHomeExecutionMode == HomeExecutionMode.PLC)
                    {
                        temp = string.Format("ENABLE PLC {0:D}", mHomeExucutionStartPlcNumber);
                        PMAC.PMacCommand(temp);
                    }
                    else
                    {
                        // TODO : PC로 홈 진행할 경우
                    }

                    mTimeoutStopwatch.Reset();
                    mTimeoutStopwatch.Start();
                    mDelayStopwatch.Reset();
                    mDelayStopwatch.Start();

                    ++mHomeStep;
                    break;

                case 2:
                    if (mDelayStopwatch.ElapsedMilliseconds <= 3000)
                    {
                        break;
                    }

                    homeDone = CheckHomeDoneFlag();
                    if (homeDone)
                    {
                        ++mHomeStep;
                    }
                    else
                    {
                        if (mTimeoutStopwatch.ElapsedMilliseconds >= HomeTimeout)
                        {
                            temp = string.Format("DISABLE PLC {0:D}", mHomeExucutionStartPlcNumber);
                            PMAC.PMacCommand(temp);
                            Stop();
                            mHomeStep = 0;
                        }
                    }
                    break;

                case 3:
                    ++mHomeStep;
                    break;

                case 4:
                    mIsHomeDone = true;
                    mHomeStep = 0;
                    break;

                default:
                    break;
            }
        }
        public bool CheckHomeDoneFlag()
        {
            //long ret = PMAC.PmacGetVariableLong(mDeviceId, 'P', mHomeCompletePVarNumber, 0);
            //return ret > 0 ? true : false;

            String strResponse = PMAC.PMacCommand(HomingCompleteString);
            string final = strResponse.Substring(0, strResponse.IndexOf("\r\n"));

            int.TryParse(final, out var value);

            return value > 0;
        }
        public bool CheckMoving()
        {
            return mIsBusy;
        }

        public bool CheckHomeDone()
        {
            return mIsInternalHomeDone;
        }

        public void Test2()
        {
            string temp = "#3k";
            PMAC.PMacCommand(temp);
        }

        public void MoveJog(double velocity)
        {
            // Check motion moving enabled.
            if (mMovingPreCheckDelegate != null && !mMovingPreCheckDelegate())
            {
                return;
            }

            // Check motion moving status.
            if (CheckMoving())
            {
                return;
            }

            string temp = string.Empty;
            if (VirtualAxisUsing)
            {
                mIndex = VirtualAxisNumber;
            }
            temp = string.Format("I{0:D}22={1:F3} #{2:D}J{3}", mIndex, Math.Abs(velocity), mIndex, velocity > 0 ? "+" : "-");


            PMAC.PMacCommand(temp);
        }

        public void MoveJog(double velocity, int accelerationTime, int sCurveTime)
        {
            // Check motion moving enabled.
            if (mMovingPreCheckDelegate != null && !mMovingPreCheckDelegate())
            {
                return;
            }

            // Check motion moving status.
            if (CheckMoving())
            {
                return;
            }

            string temp = string.Empty;

            if (VirtualAxisUsing)
            {
                mIndex = VirtualAxisNumber;
            }

            temp = string.Format(
                "I{0:D}22={1:F3} I{2:D}20={3:D} I{4:D}21={5:D} #{6:D}J{7}",
                mIndex, Math.Abs(velocity),
                mIndex, accelerationTime,
                mIndex, sCurveTime,
                mIndex, velocity * 5 > 0 ? "+" : "-");


            PMAC.PMacCommand(temp);
        }

        public void MoveRel(double position)
        {
            // Check motion moving enabled.
            if (mMovingPreCheckDelegate != null && !mMovingPreCheckDelegate())
            {
                return;
            }

            // Check motion moving status.
            if (CheckMoving())
            {
                return;
            }

            if (VirtualAxisUsing)
            {
                mIndex = VirtualAxisNumber;
            }

            string temp = string.Empty;
            temp = string.Format("#{0:D}J:{1:F}", mIndex, position * mEncoderCountsPerUnit);


            PMAC.PMacCommand(temp);

            Thread.Sleep(200);
        }

        public void MoveRel(double position, double velocity, int accelerationTime, int sCurveTime)
        {
            // Check motion moving enabled.
            if (mMovingPreCheckDelegate != null && !mMovingPreCheckDelegate())
            {
                return;
            }

            // Check motion moving status.
            if (CheckMoving())
            {
                return;
            }

            if (VirtualAxisUsing)
            {
                mIndex = VirtualAxisNumber;
            }

            string temp = string.Empty;
            temp = string.Format("I{0:D}22={1:F3} I{2:D}20={3:D} I{4:D}21={5:D} #{6:D}J:{7:F}",
                mIndex, velocity,
                mIndex, accelerationTime,
                mIndex, sCurveTime,
                mIndex, position * mEncoderCountsPerUnit);   // todo : test                       


            PMAC.PMacCommand(temp);

            Thread.Sleep(200);
        }

        public void MoveAbs(double position)
        {
            // Check motion moving enabled.
            if (mMovingPreCheckDelegate != null && !mMovingPreCheckDelegate())
            {
                return;
            }

            // Check motion moving status.
            if (CheckMoving())
            {
                return;
            }

            if (!CheckHomeDone())
            {
                return;
            }

            if (VirtualAxisUsing)
            {
                mIndex = VirtualAxisNumber;
            }

            string temp = string.Empty;
            temp = string.Format("#{0:D}J={1:F}", mIndex, position * mEncoderCountsPerUnit);


            PMAC.PMacCommand(temp);

            Thread.Sleep(200);
        }

        public void MoveAbs(double position, double velocity, int accelerationTime, int sCurveTime)
        {
            // Check motion moving enabled.
            //if (mMovingPreCheckDelegate != null && !mMovingPreCheckDelegate())
            //{
            //    return;
            //}

            //// Check motion moving status.
            //if (CheckMoving())
            //{
            //    return;
            //}

            //if (!CheckHomeDone())
            //{
            //    return;
            //}

            //mIsBusy = true;

            if (VirtualAxisUsing)
            {
                mIndex = VirtualAxisNumber;
            }
            //I122=1000 I120=100 I121=50 #1J=1000000
            string temp = string.Empty;
            temp = string.Format("I{0:D}22={1:F3} I{2:D}20={3:D} I{4:D}21={5:D} #{6:D}J={7:F}",
                mIndex, velocity,
                mIndex, accelerationTime,
                mIndex, sCurveTime,
                mIndex, position * mEncoderCountsPerUnit);


            PMAC.PMacCommand(temp);

            Thread.Sleep(200);
        }

        public void Stop()
        {
            if (VirtualAxisUsing)
            {
                mIndex = VirtualAxisNumber;
            }
            string temp = string.Empty;
            temp = string.Format("#{0:D}J/", mIndex);


            PMAC.PMacCommand(temp);
        }

        public void StopEmergency()
        {
            if (VirtualAxisUsing)
            {
                mIndex = VirtualAxisNumber;
            }
            string temp = string.Empty;
            temp = string.Format("#{0:D}J/", mIndex);


            PMAC.PMacCommand(temp);
        }

        public bool CheckCurrentPosition(double position)
        {
            if (!mIsBusy && Math.Abs(mFeedbackPosition - position) <= kInpositionRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SoftPositiveLimit(double mmCnt)
        {

            string temp = string.Empty;
            temp = string.Format("I{0:D}13={1}", mIndex, mmCnt);

            PMAC.PMacCommand(temp);
        }

        public void SoftNegativeLimit(double mmCnt)
        {

            string temp = string.Empty;
            temp = string.Format("I{0:D}14={1}", mIndex, mmCnt);

            PMAC.PMacCommand(temp);
        }
    }


    internal class PowerPmacMotion : IDisposable
    {
        #region Variables
        private UInt32 mDeviceId = 0;  // 선택창에서 리턴되는 값을 사용하지 않아서 고정 값으로 처리함.
        private Int32 mDeviceOpened;
        private bool mIsInitialized;
        private int mMotorCount;
        private Dictionary<COMMAND_KEY_GLOBAL, string> mCommandDictionary = new Dictionary<COMMAND_KEY_GLOBAL, string>();
        private Thread mMonitoringThread;
        private bool mIsTerminated;
        private ManualResetEvent mSyncEvent = new ManualResetEvent(false);
        private string mMotorStatusCommands;
        private IntPtr mResponse;
        private const int kMaxChar = 10240;


        private List<PowerPmacMotionItem> mItems = new List<PowerPmacMotionItem>();


        private const int kMaxCoordinateSystemCount = 16;
        private string mCoordinateSystemCommand1;
        private string mCoordinateSystemCommand2;
        private string mCoordinateSystemCommand3;
        private bool mDownloadStarted;
        private string mDownloadFile;
        private bool mMotionExecutionStarted;
        private Stopwatch stopwatch = new Stopwatch();
        public CoordinateSystemStatus[] mCoordinateSystemStatus;
        private const uint kJequOutportCount = 4;
        private List<string> mGCodeList = new List<string>();
        private bool mExternalPendantUsing;
        #endregion

        internal PowerPmacMotionItem this[int index]
        {
            get { return mItems[index]; }
        }

        #region Properties
        public bool Initialized { get { return mIsInitialized; } }
        public int MotorCount { get { return mMotorCount; } }

        public bool ExternalPendantUsing
        {
            get { return mExternalPendantUsing; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        //public PowerPmacMotion(int axisCounts)
        //{
        //    mMotorCount = axisCounts;
        //    Initialize();                        
        //}
        public void Initialize()
        {
            //PMAC.Initialize();

            //if(PMAC.Initialized)
            mMonitoringThread = new Thread(() => ExecuteMonitoring(this));
            mMonitoringThread.Start();
        }


        public PowerPmacMotion(params PowerPmacMotionItem[] items)
        {
            foreach (var item in items)
            {
                mItems.Add(item);
            }
            mMotorCount = mItems.Count;
            Initialize();
        }

        #endregion

        #region Implement dispose interface.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free other state (managed objects).
                mIsTerminated = true;
            }
            // Free your own state (unmanaged objects).
            // Set large fields to null.

            if (mIsInitialized)
            {
                mIsTerminated = true;
                mSyncEvent.WaitOne();

                closeDevice();
                System.Runtime.InteropServices.Marshal.FreeHGlobal(mResponse);
            }
        }

        ~PowerPmacMotion()
        {
            Dispose(false);
        }
        #endregion

        #region 모션 디바이스 초기화
        /// <summary>
        /// 모션 디바이스 초기화
        /// </summary>
        /// <returns></returns>


        #endregion

        public void GetMotorStatus()
        {

            for (int i = 0; i < mItems.Count; i++)
            {
                if (i == 100)
                {
                    continue;
                }
                try
                {
                    String strCommand = "";
                    String strResponse = "";
                    string[] strResponseArry = new string[10];
                    uint intResponse = 0;

                    strCommand = "Motor[" + (i + 1).ToString() + "].Status[0]";         //모터 상태
                    strCommand += "Motor[" + (i + 1).ToString() + "].HomePos";          //home 위치  
                    strCommand += "Motor[" + (i + 1).ToString() + "].ActPos";           //encode 위치 
                    strCommand += "Motor[" + (i + 1).ToString() + "].DesPos";           //
                    strCommand += "Motor[" + (i + 1).ToString() + "].HomeComplete";     // Home Completed    


                    strResponse = PMAC.PMacCommand(strCommand);
                    strResponseArry[0] = strResponse.Substring(0, strResponse.IndexOf("\r\n"));
                    strResponse = strResponse.Remove(0, strResponse.IndexOf("\r\n") + 2);
                    strResponseArry[1] = strResponse.Substring(0, strResponse.IndexOf("\r\n"));
                    strResponse = strResponse.Remove(0, strResponse.IndexOf("\r\n") + 2);
                    strResponseArry[2] = strResponse.Substring(0, strResponse.IndexOf("\r\n"));
                    strResponse = strResponse.Remove(0, strResponse.IndexOf("\r\n") + 2);
                    strResponseArry[3] = strResponse.Substring(0, strResponse.IndexOf("\r\n"));
                    strResponse = strResponse.Remove(0, strResponse.IndexOf("\r\n") + 2);

                    //strResponseArry[4] = strResponse.Substring(0, strResponse.IndexOf("\r\n"));
                    //strResponse = strResponse.Remove(0, strResponse.IndexOf("\r\n") + 2);


                    intResponse = Convert.ToUInt32(strResponseArry[0].Substring(1, strResponseArry[0].Length - 1), 16);  //Using ToUInt32 not ToUInt64, as per OP comment

                    double homepos = Convert.ToDouble(strResponseArry[1]) / mItems[i].EncoderCountsPerUnit;
                    double feedpos = Convert.ToDouble(strResponseArry[2]) / mItems[i].EncoderCountsPerUnit;
                    double commandpos = Convert.ToDouble(strResponseArry[3]) / mItems[i].EncoderCountsPerUnit;

                    mItems[i].SetFeedbackPosition(feedpos - homepos);
                    mItems[i].SetCommandPosition(commandpos - homepos);


                    mItems[i].SetEnable((intResponse & 0x00001000) == 0x00001000);
                    bool FOnReady = ((intResponse & 0x00002000) == 0x00002000);

                    mItems[i].SetAmpFault((intResponse & 0x01000000) == 0x00100000);

                    mItems[i].SetCwLimitSensorDetection((intResponse & 0x10000000) == 0x10000000);
                    mItems[i].SetCCwLimitSensorDetection((intResponse & 0x20000000) == 0x20000000);

                    mItems[i].IsMotionDone = ((intResponse & 0x00004000) == 0x00004000);

                    //mItems[i].SetInposition((intResponse & 0x00000800) == 0x00000800);
                    //mItems[i].IsHomeDone = ((intResponse & 0x00008000) == 0x00008000);

                    bool Ready = ((intResponse & 0x00002000) == 0x00002000);
                    mItems[i].SetBusy(!Ready);

                    double RangeSpec = 0.005;

                    if (mItems[i].CommandPosition + RangeSpec > mItems[i].FeedbackPosition &&
                        mItems[i].CommandPosition - RangeSpec < mItems[i].FeedbackPosition &&
                       (intResponse & 0x00000800) == 0x00000800)
                    {
                        mItems[i].SetInposition(true);
                    }
                    else
                    {
                        mItems[i].SetInposition(false);
                    }


                    // = ((intResponse & 0x00002000) == 0x00002000);
                    Thread.Sleep(10);
                }
                catch { }

            }
        }




        private void ExecuteMonitoring(object obj)
        {
            mSyncEvent.Set();

            stopwatch.Start();

            while (!mIsTerminated)
            {
                if (PMAC.IsConnected)
                {
                    GetMotorStatus();

                    foreach (var item in mItems)
                    {
                        item.ExecuteHoming();
                    }
                }
                Thread.Sleep(1);
            }

            mSyncEvent.Set();
        }




        #region 모션 디바이스 종료
        public void closeDevice()
        {
            PMAC.DTKPowerPmacClose64(0);
        }
        #endregion

        public enum COMMAND_KEY_GLOBAL
        {
            RESET,
            READING_MOTOR_STATUS,
            READING_COORDINATE_SYSTEM_STATUS_1,
            READING_COORDINATE_SYSTEM_STATUS_2,
            READING_COORDINATE_SYSTEM_STATUS_3,
        }

        /// <summary>
        /// Add command key value.
        /// </summary>
        private void AddCommandKeyValue()
        {
            mCommandDictionary.Add(COMMAND_KEY_GLOBAL.RESET, "$$$");

        }

        /// <summary>
        /// Make motor status reading commands.
        /// </summary>
        private void MakeMotorStatusReadingCommands()
        {
            mMotorStatusCommands = "";

            string temp = string.Empty;
            string str = string.Empty;
            StringBuilder sb = new StringBuilder();

            // Position (in counts)
            // 32축 이상이면 문제가 발생할 수 있음....
            // 추후 DPRAM 사용해서 간접 접근하는게 좋음.
            // TODO: MOTOR STATUS UPDATE!!!!!!!!!!! 2017.12.08
            for (int n = 0; n < mMotorCount; n++)
            {
                temp = string.Format("#{0:D}P#{1:D}V#{2:D}F#{3:D}?", mItems[n].Index, mItems[n].Index, mItems[n].Index, mItems[n].Index);
                sb.Append(temp);
            }
            mMotorStatusCommands = sb.ToString();
            sb = null;
            mCommandDictionary.Add(COMMAND_KEY_GLOBAL.READING_MOTOR_STATUS, mMotorStatusCommands);
        }

        /// <summary>
        /// Get motor status.
        /// </summary>


        /// <summary>
        /// Make coordinate system commands.
        /// </summary>
        private void MakeCoordinateSystemCommands()
        {
            mCoordinateSystemCommand1 = string.Empty;
            mCoordinateSystemCommand2 = string.Empty;
            mCoordinateSystemCommand3 = string.Empty;

            int start = 0x2040;
            int offset = 0x100;
            int current = 0x00;
            string temp = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < kMaxCoordinateSystemCount; index++)
            {
                current = start + offset * index;
                temp = string.Format("RHX:${0,4:X}", current);
                sb.Append(temp);
            }
            mCoordinateSystemCommand1 = sb.ToString();
            sb.Clear();
            current = 0x00;
            mCommandDictionary.Add(COMMAND_KEY_GLOBAL.READING_COORDINATE_SYSTEM_STATUS_1, mCoordinateSystemCommand1);

            start = 0x203F;
            for (int index = 0; index < kMaxCoordinateSystemCount; index++)
            {
                current = start + offset * index;
                temp = string.Format("RHY:${0,4:X}", current);   // 해당 어드레스에서 상위 24Bits Data를 16진수로 읽어온다.
                sb.Append(temp);
            }
            mCoordinateSystemCommand2 = sb.ToString();
            sb.Clear();
            current = 0x00;
            mCommandDictionary.Add(COMMAND_KEY_GLOBAL.READING_COORDINATE_SYSTEM_STATUS_2, mCoordinateSystemCommand2);

            start = 0x2040;
            for (int nIndex = 0; nIndex < kMaxCoordinateSystemCount; nIndex++)
            {
                current = start + offset * nIndex;
                temp = string.Format("RHY:${0,4:X}", current);   // 해당 어드레스에서 상위 24Bits Data를 16진수로 읽어온다.
                sb.Append(temp);
            }
            mCoordinateSystemCommand3 = sb.ToString();
            sb = null;
            mCommandDictionary.Add(COMMAND_KEY_GLOBAL.READING_COORDINATE_SYSTEM_STATUS_3, mCoordinateSystemCommand3);
        }

        /// <summary>
        /// Execute command by dictionary's key.
        /// </summary>
        /// <param name="commandKey"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        //private long ExecuteCommand(COMMAND_KEY_GLOBAL commandKey, out IntPtr result)
        //{
        //    long returnValue;
        //    string command = mCommandDictionary[commandKey];
        //    returnValue = PMAC.PmacGetResponseExA(mDeviceId, mResponse, kMaxChar, new StringBuilder(command));
        //    result = mResponse;
        //    return returnValue;
        //}

        //private long ExecuteCommand(string strCommand, out IntPtr result)
        //{
        //    long returnValue;
        //    returnValue = PMAC.PmacGetResponseExA(mDeviceId, mResponse, kMaxChar, new StringBuilder(strCommand));
        //    result = mResponse;
        //    return returnValue;
        //}




        public void Test()
        {
            IntPtr result;
            //ExecuteCommand("#" + "1" + " HMZ", out result);
            //ExecuteCommand("#" + "2" + " HMZ", out result);
            //ExecuteCommand("#" + "3" + " HMZ", out result);
            //PMAC.PMacCommand("#3k");

            String strResponse = PMAC.PMacCommand("GantryHomeState");
            string final = strResponse.Substring(0, strResponse.IndexOf("\r\n"));
        }

        public void SetClipperReset()
        {
            IntPtr result;
            //ExecuteCommand("$$$", out result);
        }

        public void EmergencyStop()
        {
            foreach (var item in mItems)
            {
                item.Stop();
            }
        }


        public bool CheckAllHomeDone()
        {
            int homeDoneCount = 0;

            foreach (var item in mItems)
            {
                if (item.IsHomeDone)
                {
                    homeDoneCount++;
                }
            }
            return homeDoneCount >= MotorCount ? true : false;
        }
    }
}
