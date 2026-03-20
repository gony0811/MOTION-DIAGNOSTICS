using System;
using System.Runtime.InteropServices;

namespace Device
{
    enum DTK_MODE_TYPE
    {
        DM_GPASCII = 0,
        DM_GETSENDS_0 = 1,
        DM_GETSENDS_1 = 2,
        DM_GETSENDS_2 = 3,
        DM_GETSENDS_3 = 4,
        DM_GETSENDS_4 = 5
    };

    enum DTK_STATUS
    {
        DS_Ok = 0,
        DS_Exception = 1,
        DS_TimeOut = 2,
        DS_Connected = 3,
        DS_NotConnected = 4,
        DS_Failed = 5,
        DS_InvalidDevice = 11,
        DS_LengthExceeds = 21,
        DS_RunningDownload = 22,
        DS_RunningRead = 23
    };

    enum DTK_RESET_TYPE
    {
        DR_Reset = 0,
        DR_FullReset = 1
    };

    internal class PMAC
    {
        public delegate void PDOWNLOAD_MESSAGE_A(String lpMessage);
        public delegate void PDOWNLOAD_PROGRESS(Int32 nPercent);
        public delegate void PRECEIVE_PROC_A(String lpReveive);

        // 라이브러리 오픈
        #region PowerPmac64.DLL
        // 인자를 NULL로 할 경우 DTKDeviceSelect 함수를 사용하여 장치를 연결해야 한다.
        [DllImport("PowerPmac64.dll", EntryPoint = "DTKPowerPmacOpen")]
        public static extern UInt32 DTKPowerPmacOpen64(UInt32 dwIPAddress, UInt32 uMode);

        // 라이브리리 클로즈
        [DllImport("PowerPmac32.dll", EntryPoint = "DTKPowerPmacClose")]
        public static extern UInt32 DTKPowerPmacClose64(UInt32 uDeviceID);

        // 등록된 디바이스 갯수
        [DllImport("PowerPmac64.dll", EntryPoint = "DTKGetDeviceCount")]
        public static extern UInt32 DTKGetDeviceCount64(out Int32 pnDeviceCount);

        // IP Address 확인
        [DllImport("PowerPmac64.dll", EntryPoint = "DTKGetIPAddress")]
        public static extern UInt32 DTKGetIPAddress64(UInt32 uDeviceID, out UInt32 pdwIPAddress);

        // 장치를 연결
        [DllImport("PowerPmac64.dll", EntryPoint = "DTKConnect")]
        public static extern UInt32 DTKConnect64(UInt32 uDeviceID);

        // 장치를 해제
        [DllImport("PowerPmac64.dll", EntryPoint = "DTKDisconnect")]
        public static extern UInt32 DTKDisconnect64(UInt32 uDeviceID);

        // 장치가 연결되었는지 확인
        [DllImport("PowerPmac64.dll", EntryPoint = "DTKIsConnected")]
        public static extern UInt32 DTKIsConnected64(UInt32 uDeviceID, out Int32 pbConnected);

        [DllImport("PowerPmac64.dll", EntryPoint = "DTKGetResponseA")]
        public static extern UInt32 DTKGetResponseA64(UInt32 uDeviceID, Byte[] lpCommand, Byte[] lpResponse, Int32 nLength);

        //  [DllImport("PowerPmac64.dll", EntryPoint = "DTKGetResponseW")]
        //  public static extern UInt32 DTKGetResponseW64(UInt32 uDeviceID, String lpwCommand, ref String lpwResponse, Int32 nLength);

        [DllImport("PowerPmac64.dll", EntryPoint = "DTKSendCommandA")]
        public static extern UInt32 DTKSendCommandA64(UInt32 uDeviceID, Byte[] lpCommand);

        [DllImport("PowerPmac64.dll", EntryPoint = "DTKAbort")]
        public static extern UInt32 DTKAbort64(UInt32 uDeviceID);

        [DllImport("PowerPmac64.dll", EntryPoint = "DTKDownloadA")]
        public static extern UInt32 DTKDownloadA64(UInt32 uDeviceID, Byte[] lpwDownload, Int32 bDowoload, PDOWNLOAD_PROGRESS lpDownloadProgress, PDOWNLOAD_MESSAGE_A lpDownloadMessage);

        [DllImport("PowerPmac64.dll", EntryPoint = "DTKSetReceiveA")]
        public static extern UInt32 DTKSetReceiveA64(UInt32 uDeviceID, PRECEIVE_PROC_A lpReveiveProc);
        #endregion

        #region PowerPmac32.DLL
        // 인자를 NULL로 할 경우 DTKDeviceSelect 함수를 사용하여 장치를 연결해야 한다.
        [DllImport("PowerPmac32.dll", EntryPoint = "DTKPowerPmacOpen")]
        public static extern UInt32 DTKPowerPmacOpen32(UInt32 dwIPAddress, UInt32 uMode);

        // 라이브리리 클로즈
        [DllImport("PowerPmac32.dll", EntryPoint = "DTKPowerPmacClose")]
        public static extern UInt32 DTKPowerPmacClose32(UInt32 uDeviceID);

        // 등록된 디바이스 갯수
        [DllImport("PowerPmac32.dll", EntryPoint = "DTKGetDeviceCount")]
        public static extern UInt32 DTKGetDeviceCount32(out Int32 pnDeviceCount);

        // IP Address 확인
        [DllImport("PowerPmac32.dll", EntryPoint = "DTKGetIPAddress")]
        public static extern UInt32 DTKGetIPAddress32(UInt32 uDeviceID, out UInt32 pdwIPAddress);

        // 장치를 연결
        [DllImport("PowerPmac32.dll", EntryPoint = "DTKConnect")]
        public static extern UInt32 DTKConnect32(UInt32 uDeviceID);

        // 장치를 해제
        [DllImport("PowerPmac32.dll", EntryPoint = "DTKDisconnect")]
        public static extern UInt32 DTKDisconnect32(UInt32 uDeviceID);

        // 장치가 연결되었는지 확인
        [DllImport("PowerPmac32.dll", EntryPoint = "DTKIsConnected")]
        public static extern UInt32 DTKIsConnected32(UInt32 uDeviceID, out Int32 pbConnected);

        [DllImport("PowerPmac32.dll", EntryPoint = "DTKGetResponseA")]
        public static extern UInt32 DTKGetResponseA32(UInt32 uDeviceID, Byte[] lpCommand, Byte[] lpResponse, Int32 nLength);

        //  [DllImport("PowerPmac32.dll", EntryPoint = "DTKGetResponseW")]
        //  public static extern UInt32 DTKGetResponseW32(UInt32 uDeviceID, String lpwCommand, ref String lpwResponse, Int32 nLength);

        [DllImport("PowerPmac32.dll", EntryPoint = "DTKSendCommandA")]
        public static extern UInt32 DTKSendCommandA32(UInt32 uDeviceID, Byte[] lpCommand);

        [DllImport("PowerPmac32.dll", EntryPoint = "DTKAbort")]
        public static extern UInt32 DTKAbort32(UInt32 uDeviceID);

        [DllImport("PowerPmac32.dll", EntryPoint = "DTKDownloadA")]
        public static extern UInt32 DTKDownloadA32(UInt32 uDeviceID, Byte[] lpwDownload, Int32 bDowoload, PDOWNLOAD_PROGRESS lpDownloadProgress, PDOWNLOAD_MESSAGE_A lpDownloadMessage);

        [DllImport("PowerPmac32.dll", EntryPoint = "DTKSetReceiveA")]
        public static extern UInt32 DTKSetReceiveA32(UInt32 uDeviceID, PRECEIVE_PROC_A lpReveiveProc);
        #endregion

        public static UInt32 DTKPowerPmacOpen(UInt32 dwIPAddress, UInt32 uMode)
        {
            if (Environment.Is64BitProcess) return DTKPowerPmacOpen64(dwIPAddress, uMode);
            else return DTKPowerPmacOpen32(dwIPAddress, uMode);
        }

        public static UInt32 DTKPowerPmacClose(UInt32 uDeviceID)
        {
            if (Environment.Is64BitProcess) return DTKPowerPmacClose64(uDeviceID);
            else return DTKPowerPmacClose32(uDeviceID);
        }

        public static UInt32 DTKGetDeviceCount(out Int32 pnDeviceCount)
        {
            if (Environment.Is64BitProcess) return DTKGetDeviceCount64(out pnDeviceCount);
            else return DTKGetDeviceCount32(out pnDeviceCount);
        }
        public static UInt32 DTKGetIPAddress(UInt32 uDeviceID, out UInt32 pdwIPAddress)
        {
            if (Environment.Is64BitProcess) return DTKGetIPAddress64(uDeviceID, out pdwIPAddress);
            else return DTKGetIPAddress32(uDeviceID, out pdwIPAddress);
        }
        public static UInt32 DTKConnect(UInt32 uDeviceID)
        {
            if (Environment.Is64BitProcess) return DTKConnect64(uDeviceID);
            else return DTKConnect32(uDeviceID);
        }
        public static UInt32 DTKDisconnect(UInt32 uDeviceID)
        {
            if (Environment.Is64BitProcess) return DTKDisconnect64(uDeviceID);
            else return DTKDisconnect32(uDeviceID);
        }
        public static UInt32 DTKIsConnected(UInt32 uDeviceID, out Int32 pbConnected)
        {
            if (Environment.Is64BitProcess) return DTKIsConnected64(uDeviceID, out pbConnected);
            else return DTKIsConnected32(uDeviceID, out pbConnected);
        }
        public static UInt32 DTKGetResponseA(UInt32 uDeviceID, Byte[] lpCommand, Byte[] lpResponse, Int32 nLength)
        {
            if (Environment.Is64BitProcess) return DTKGetResponseA64(uDeviceID, lpCommand, lpResponse, nLength);
            else return DTKGetResponseA32(uDeviceID, lpCommand, lpResponse, nLength);
        }

        /*
        public static UInt32 DTKGetResponseW(UInt32 uDeviceID, String lpwCommand, ref String lpwResponse, Int32 nLength)
        {
            if (Environment.Is64BitProcess) return DTKGetResponseW64(uDeviceID, lpwCommand, ref lpwResponse, nLength);
            else                            return DTKGetResponseW32(uDeviceID, lpwCommand, ref lpwResponse, nLength);
        }
        */

        public static UInt32 DTKSendCommandA(UInt32 uDeviceID, Byte[] lpCommand)
        {
            if (Environment.Is64BitProcess) return DTKSendCommandA64(uDeviceID, lpCommand);
            else return DTKSendCommandA32(uDeviceID, lpCommand);
        }
        public static UInt32 DTKAbort(UInt32 uDeviceID)
        {
            if (Environment.Is64BitProcess) return DTKAbort64(uDeviceID);
            else return DTKAbort32(uDeviceID);
        }
        public static UInt32 DTKDownloadA(UInt32 uDeviceID, Byte[] lpwDownload, Int32 bDowoload, PDOWNLOAD_PROGRESS lpDownloadProgress, PDOWNLOAD_MESSAGE_A lpDownloadMessage)
        {
            if (Environment.Is64BitProcess) return DTKDownloadA64(uDeviceID, lpwDownload, bDowoload, lpDownloadProgress, lpDownloadMessage);
            else return DTKDownloadA32(uDeviceID, lpwDownload, bDowoload, lpDownloadProgress, lpDownloadMessage);
        }
        public static UInt32 DTKSetReceiveA(UInt32 uDeviceID, PRECEIVE_PROC_A lpReveiveProc)
        {
            if (Environment.Is64BitProcess) return DTKSetReceiveA64(uDeviceID, lpReveiveProc);
            else return DTKSetReceiveA32(uDeviceID, lpReveiveProc);
        }

        public static string IP { private get; set; } = "192.168.0.200";

        static public UInt32 deviceID = 0xFFFFFFFF;

        static private string directory = string.Empty;
        public string Directory { get { return directory; } }

        public static bool IsConnected { get; private set; } = false;


        /// <summary>
        /// PMAC Device와 통신 연결 및 초기화
        /// deviceID가 0xFFFFFFFF이면 초기화 실패
        /// </summary>
        /// <returns></returns>
        public static bool Initialize(string ipAddr)
        {
            byte[] byCommand;
            UInt32 uRet;
            UInt32 uIPAddress;
            string[] strIP = new string[4];

            if (string.IsNullOrEmpty(ipAddr))
            {
                ipAddr = IP;
            }

            strIP = ipAddr.Split('.');

            uIPAddress = (Convert.ToUInt32(strIP[0]) << 24) | (Convert.ToUInt32(strIP[1]) << 16) | (Convert.ToUInt32(strIP[2]) << 8) | Convert.ToUInt32(strIP[3]);

            deviceID = PMAC.DTKPowerPmacOpen(uIPAddress, (UInt32)DTK_MODE_TYPE.DM_GPASCII);

            uRet = PMAC.DTKConnect(deviceID);

            if ((DTK_STATUS)uRet == DTK_STATUS.DS_Ok)
            {
                byCommand = new Byte[255];
                byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes("echo 3");
                uRet = PMAC.DTKSendCommandA(deviceID, byCommand);
                IsConnected = true;
            }
            else
            {
                PMAC.DTKPowerPmacClose(deviceID);
                deviceID = 0xFFFFFFFF;
                IsConnected = false;
            }

            return IsConnected;
        }

        static public void ReleaseInstance()
        {
            if (!IsConnected) return;

#if (__PMAC__ == false)
            if (IsConnected)
            {
                if (deviceID != 0xFFFFFFFF)
                {
                    Int32 nConnected = 0;

                    PMAC.DTKIsConnected(deviceID, out nConnected);
                    if (nConnected == 1)
                        PMAC.DTKDisconnect(deviceID);
                    PMAC.DTKPowerPmacClose(deviceID);
                    deviceID = 0xFFFFFFFF;
                }
            }
#endif
            //ParameterSave();

        }

        static public string PMacCommand(String AString)
        {
            String strResponse = "";
#if (__PMAC__ == false)
            Byte[] byCommand;
            Byte[] byResponse;
            byCommand = new Byte[255];
            byResponse = new Byte[255];

            String stringcmd = AString;

            byCommand = System.Text.Encoding.GetEncoding("euc-kr").GetBytes(stringcmd);
            PMAC.DTKGetResponseA(deviceID, byCommand, byResponse, Convert.ToInt32(byResponse.Length - 1));
            strResponse = System.Text.Encoding.GetEncoding("euc-kr").GetString(byResponse);
#endif
            return strResponse;
        }
    }

    public class CoordinateSystemStatus
    {
        public bool bit_Running_Program;
        public bool bit_Single_Step_Mode;
        public bool bit_Continuous_Motion_Mode;
        public bool bit_Lookahead_In_Progress;
        public bool bit_Runtime_Error;
        public bool bit_Amp_Fault_Error;
        public bool bit_Fatal_Following_Error;
        public bool bit_Warning_Following_Error;
        public bool bit_In_Position;
        public bool bit_Rotary_Buffer_Full;
        public bool bit_Pvt_Spline_Move_Mode;
        public bool bit_Cutter_Comp_Left;
        public bool bit_Cutter_Comp_On;
        public bool bit_CCW_Circle_Rapid_Move_Mode;
        public bool bit_Circle_Spline_Move_Mode;
        public bool bit_Radius_Error;
        public bool bit_Program_Resume_Error;
        public bool bit_Desirecd_Position_Limit_Stop;
    }

    public class MotorStatusYBitField
    {
        public bool bit_00_Inposition;
        public bool bit_01_WarningFollowingError;
        public bool bit_02_FatalFollowingError;
        public bool bit_03_AmpFault;
        public bool bit_04_BacklashDirectionFlag;
        public bool bit_05_I2T_A;
        public bool bit_06_IntegratedFatalFollowingError;
        public bool bit_07_TriggerMove;
        public bool bit_08_PhasingSearchError;
        public bool bit_09_MotorPhaseRequest;
        public bool bit_10_HomeComplete;
        public bool bit_11_StoppedOnPositionLimit;
        public bool bit_12_DesiredPositionLimitStop;
        public bool bit_13_ForegroundInPosition;
        public bool bit_14_ReservedForFutureUse;
        public bool bit_15_AssignedToCS;
        public bool bit_16_CS_AxisDefinitionBit0;
        public bool bit_17_CS_AxisDefinitionBit1;
        public bool bit_18_CS_AxisDefinitionBit2;
        public bool bit_19_CS_AxisDefinitionBit3;
        public bool bit_20_CS_1_Bit0_LSB;
        public bool bit_21_CS_1_Bit1;
        public bool bit_22_CS_1_Bit2;
        public bool bit_23_CS_1_Bit3_MSB;
    }

    public class MotorStatusXBitField
    {
        public bool bit_00_RapidMaxVelocitySelect;
        public bool bit_01_SignMagnitudeServoEnable;
        public bool bit_02_SoftwareCaptureEnable;
        public bool bit_03_CaptureOnErrorEnable;
        public bool bit_04_PosFollowEnable;
        public bool bit_05_PosFollowOffsetMode;
        public bool bit_06_CommutationEnable;
        public bool bit_07_YAddrCommuteEnc;
        public bool bit_08_UserWrittenServoEnable;
        public bool bit_09_UserWrittenPhaseEnable;
        public bool bit_10_HomeSearchInProgress;
        public bool bit_11_BlockRequest;
        public bool bit_12_AbortDecelerationInProgress;
        public bool bit_13_DesiredVelocity0;
        public bool bit_14_DataBlockError;
        public bool bit_15_DwellInProgress;
        public bool bit_16_IntegrationMode;
        public bool bit_17_MoveTimerActive;
        public bool bit_18_OpenLoopMode;
        public bool bit_19_AmplifierEnabled;
        public bool bit_20_ExtServoAlgorithmEnable;
        public bool bit_21_PositiveEndLimitSet;
        public bool bit_22_NegativeEndLimitSet;
        public bool bit_23_MotorActivated;
    }
    public enum HomeExecutionMode
    {
        PLC,
        PC,
    }
}
