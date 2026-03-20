using ACS.SPiiPlusNET;
using Device.Driver;
using Device.Options;
using EPLE.Core.Device.Interface;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Device
{
    public class ACSMotionControl : IDeviceHandler
    {
        public enum AXIS_INDEX : int
        {
            X = 0,
            Y = 2,
            Z = 3,
            T = 4
        }


        public bool DeviceAttach(string arguments)
        {
            logger.Information("ACS Motion start attaching.");

            if (settingOptions == null)
            {
                logger.Error("DeviceInit is not execute.");
                return false;
            }

            /// Simulation mode setting
            if (arguments.StartsWith("S"))
            {
                simulationMode = true;
                logger.Debug("ACS motion control running simulation mode");
                acsDriver = new ACSDriver(logger, "Simulator");
            }
            else
            {
                simulationMode = false;
                acsDriver = new ACSDriver(logger, settingOptions.Communication);
            }
            

            if (!acsDriver.IsConnected)
            {
                logger.Error("ACS Driver is not connected.");
                devMode = DevMode.DISCONNECT;
                return false;
            }
            else
            {
                devMode = DevMode.CONNECT;
                logger.Information("ACS Driver is connected.");
            }

            var axisCount = settingOptions.AxisCount;
            ACSMotionItem item;
            string name = string.Empty;

            for (int motorIndex = 0; motorIndex < settingOptions.AxisCount; motorIndex++)
            {
                name = settingOptions.AxisParameters[motorIndex].Name;
                item = new ACSMotionItem(acsDriver, settingOptions.AxisParameters[motorIndex], motorIndex) { Name = name };
                motionItems.Add(item);
            }

            return true;
        }

        public bool DeviceDettach()
        {
            if (acsDriver == null || simulationMode)
            {
                logger.Information("acs motion is simulation mode.");
                devMode = DevMode.DISCONNECT;
                DeviceClose();
                return true;
            }
            else
            {
                devMode = DevMode.DISCONNECT;
                logger.Information("ACS motion control is detached.");
                DeviceClose();
                return true;
            }
        }

        public void DeviceInit(ILogger logger)
        {
            this.logger = logger;
            devMode = DevMode.DISCONNECT;
            settingOptions = settingFileParse();

            logger.Information("ACS Motion is initialized.");
        }

        public bool DeviceReset()
        {
            logger.Information("ACS is reset.");
            return true;
        }

        public object GET_DATA_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public double GET_DOUBLE_IN(string command, ref bool result)
        {
            result = true;

            switch (command)
            {
                case "X_IN_ACTPOS":
                    return motionItems[(int)AXIS_INDEX.X].FeedbackPosition;
                case "Y_IN_ACTPOS":
                    return motionItems[(int)AXIS_INDEX.Y].FeedbackPosition;
                case "T_IN_ACTPOS":
                    return motionItems[(int)AXIS_INDEX.T].FeedbackPosition;
                case "Z1_IN_ACTPOS":
                case "Z2_IN_ACTPOS":
                case "Z3_IN_ACTPOS":
                    return motionItems[(int)AXIS_INDEX.Z].FeedbackPosition;

                case "X_IN_ACTVEL":
                    return motionItems[(int)AXIS_INDEX.X].ActualVelocity;
                case "Y_IN_ACTVEL":
                    return motionItems[(int)AXIS_INDEX.Y].ActualVelocity;
                case "Z1_IN_ACTVEL":
                case "Z2_IN_ACTVEL":
                case "Z3_IN_ACTVEL":
                    return motionItems[(int)AXIS_INDEX.Z].ActualVelocity;
                case "X_IN_CMDVEL":
                    return motionItems[(int)AXIS_INDEX.X].CurrentVelocity;
                case "Y_IN_CMDVEL":
                    return motionItems[(int)AXIS_INDEX.Y].CurrentVelocity;
                case "Z1_IN_CMDVEL":
                case "Z2_IN_CMDVEL":
                case "Z3_IN_CMDVEL":
                    return motionItems[(int)AXIS_INDEX.Z].CurrentVelocity;
                case "T_IN_ACTVEL":
                    return motionItems[(int)AXIS_INDEX.T].ActualVelocity;
                case "T_IN_CMDVEL":
                    return motionItems[(int)AXIS_INDEX.T].CurrentVelocity;
                default:
                    result = false;
                    break;
            }

            return 0.0;
        }

        public int GET_INT_IN(string command, ref bool result)
        {
            result = true;

            switch (command)
            {
                case "X_IN_ENABLE":
                    return motionItems[(int)AXIS_INDEX.X].IsEnable ? 1 : 0;
                case "Y_IN_ENABLE":
                    return motionItems[(int)AXIS_INDEX.Y].IsEnable ? 1 : 0;
                case "Z1_IN_ENABLE":
                    return motionItems[(int)AXIS_INDEX.Z].IsEnable ? 1 : 0;
                case "T_IN_ENABLE":
                    return motionItems[(int)AXIS_INDEX.T].IsEnable ? 1 : 0;
                case "X_IN_BUSY":
                    return motionItems[(int)AXIS_INDEX.X].IsBusy ? 1 : 0;
                case "Y_IN_BUSY":
                    return motionItems[(int)AXIS_INDEX.Y].IsBusy ? 1 : 0;
                case "Z1_IN_BUSY":
                    return motionItems[(int)AXIS_INDEX.Z].IsBusy ? 1 : 0;
                case "T_IN_BUSY":
                    return motionItems[(int)AXIS_INDEX.T].IsBusy ? 1 : 0;
                case "X_IS_CALIBRATED":
                    return motionItems[(int)AXIS_INDEX.X].IsHomeDone ? 1 : 0;
                case "Y_IS_CALIBRATED":
                    return motionItems[(int)AXIS_INDEX.Y].IsHomeDone ? 1 : 0;
                case "Z1_IS_CALIBRATED":
                case "Z2_IS_CALIBRATED":
                case "Z3_IS_CALIBRATED":
                    return motionItems[(int)AXIS_INDEX.Z].IsHomeDone ? 1 : 0;
                case "T_IS_CALIBRATED":
                    return motionItems[(int)AXIS_INDEX.T].IsHomeDone ? 1 : 0;
                case "X_IN_INPOS":
                    return motionItems[(int)AXIS_INDEX.X].IsInPosition ? 1 : 0;
                case "Y_IN_INPOS":
                    return motionItems[(int)AXIS_INDEX.Y].IsInPosition ? 1 : 0;
                case "Z1_IN_INPOS":
                case "Z2_IN_INPOS":
                case "Z3_IN_INPOS":
                    return motionItems[(int)AXIS_INDEX.Z].IsInPosition ? 1 : 0;
                case "X_IN_LIMITPLUS":
                    return motionItems[(int)AXIS_INDEX.X].IsHwPositiveLimit ? 1 : 0;
                case "Y_IN_LIMITPLUS":
                    return motionItems[(int)AXIS_INDEX.Y].IsHwPositiveLimit ? 1 : 0;
                case "Z1_IN_LIMITPLUS":
                    return motionItems[(int)AXIS_INDEX.Z].IsHwPositiveLimit ? 1 : 0;
                case "T_IN_LIMITPLUS":
                    return motionItems[(int)AXIS_INDEX.T].IsHwPositiveLimit ? 1 : 0;
                case "X_IN_LIMITMINUS":
                    return motionItems[(int)AXIS_INDEX.X].IsHwNegativeLimit ? 1 : 0;
                case "Y_IN_LIMITMINUS":
                    return motionItems[(int)AXIS_INDEX.Y].IsHwNegativeLimit ? 1 : 0;
                case "Z1_IN_LIMITMINUS":
                case "Z2_IN_LIMITMINUS":
                case "Z3_IN_LIMITMINUS":
                    return motionItems[(int)AXIS_INDEX.Z].IsHwNegativeLimit ? 1 : 0;
                default:
                    result = false;
                    return 0;
            }
        }

        public string GET_STRING_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public DevMode IsDevMode()
        {
            return devMode;
        }

        public void SET_DATA_OUT(string command, object value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public void SET_DOUBLE_OUT(string command, double value, ref bool result)
        {
            result = true;

            switch (command)
            {
                case "X_OUT_VELOCITY":
                    xCommandVelocity = value;
                    motionItems[(int)AXIS_INDEX.X].SetSpeed(value); break;
                case "Y_OUT_VELOCITY":
                    yCommandVelocity = value;
                    motionItems[(int)AXIS_INDEX.Y].SetSpeed(value); break;
                case "T_OUT_VELOCITY":
                    tCommandVelocity = value;
                    motionItems[(int)AXIS_INDEX.T].SetSpeed(value); break;
                case "Z1_OUT_VELOCITY":
                    zCommandVelocity = value;
                    motionItems[(int)AXIS_INDEX.Z].SetSpeed(value); break;
                case "Z2_OUT_VELOCITY":
                case "Z3_OUT_VELOCITY":
                case "X_OUT_MOVEREL":
                    motionItems[(int)AXIS_INDEX.X].MoveRel(value); break;
                case "Y_OUT_MOVEREL":
                    motionItems[(int)AXIS_INDEX.Y].MoveRel(value); break;
                case "T_OUT_MOVEREL":
                    motionItems[(int)AXIS_INDEX.T].MoveRel(value); break;
                case "Z1_OUT_MOVEREL":
                case "Z2_OUT_MOVEREL":
                case "Z3_OUT_MOVEREL":
                    motionItems[(int)AXIS_INDEX.Z].MoveRel(value); break;
                case "X_OUT_MOVEABS":
                    motionItems[(int)AXIS_INDEX.X].MoveAbs(value); break;
                case "Y_OUT_MOVEABS":
                    motionItems[(int)AXIS_INDEX.Y].MoveAbs(value); break;
                case "T_OUT_MOVEABS":
                    motionItems[(int)AXIS_INDEX.T].MoveAbs(value); break;
                case "Z1_OUT_MOVEABS":
                case "Z2_OUT_MOVEABS":
                case "Z3_OUT_MOVEABS":
                    motionItems[(int)AXIS_INDEX.Z].MoveAbs(value); break;
                default:
                    result = false;
                    break;
            }

            
        }

        public void SET_INT_OUT(string command, int value, ref bool result)
        {
            result = true;

            switch (command)
            {
                case "X_OUT_ENABLE":
                    if (value > 0) 
                        motionItems[(int)AXIS_INDEX.X].Enable();
                    else
                        motionItems[(int)AXIS_INDEX.X].Disable();
                    break;
                case "Y_OUT_ENABLE":
                    if (value > 0)
                        motionItems[(int)AXIS_INDEX.Y].Enable();
                    else
                        motionItems[(int)AXIS_INDEX.Y].Disable();
                    break;
                case "T_OUT_ENABLE":
                    if (value > 0)
                        motionItems[(int)AXIS_INDEX.T].Enable();
                    else
                        motionItems[(int)AXIS_INDEX.T].Disable();
                    break;
                case "Z1_OUT_ENABLE":
                case "Z2_OUT_ENABLE":
                case "Z3_OUT_ENABLE":
                    if (value > 0)
                        motionItems[(int)AXIS_INDEX.Z].Enable();
                    else
                        motionItems[(int)AXIS_INDEX.Z].Disable();
                    break;
                case "X_OUT_STOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.X].Stop(); 
                    break;
                case "Y_OUT_STOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Y].Stop();
                    break;
                case "T_OUT_STOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.T].Stop();
                    break;
                case "Z1_OUT_STOP":
                case "Z2_OUT_STOP":
                case "Z3_OUT_STOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Z].Stop();
                    break;
                case "X_OUT_ESTOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.X].Stop();
                    break;
                case "Y_OUT_ESTOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Y].Stop();
                    break;
                case "T_OUT_ESTOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.T].Stop();
                    break;
                case "Z1_OUT_ESTOP":
                case "Z2_OUT_ESTOP":
                case "Z3_OUT_ESTOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Z].Stop();
                    break;
                case "X_EXEC_HOME":
                    if (value > 0) motionItems[(int)AXIS_INDEX.X].StartHome();
                    break;
                case "Y_EXEC_HOME":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Y].StartHome();
                    break;
                case "T_EXEC_HOME":
                    if (value > 0) motionItems[(int)AXIS_INDEX.T].StartHome();
                    break;
                case "Z1_EXEC_HOME":
                case "Z2_EXEC_HOME":
                case "Z3_EXEC_HOME":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Z].StartHome();
                    break;

                // 250121_mh.yun
                case "X_OUT_JOGPLUS":
                    if (value > 0) motionItems[(int)AXIS_INDEX.X].MoveJog(velocity: xCommandVelocity);
                    break;
                case "Y_OUT_JOGPLUS":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Y].MoveJog(velocity: yCommandVelocity);
                    break;
                case "T_OUT_JOGPLUS":
                    if (value > 0) motionItems[(int)AXIS_INDEX.T].MoveJog(velocity: tCommandVelocity);
                    break;
                case "Z1_OUT_JOGPLUS":
                case "Z2_OUT_JOGPLUS":
                case "Z3_OUT_JOGPLUS":
                    motionItems[(int)AXIS_INDEX.Z].MoveJog(velocity: zCommandVelocity);
                    break;
                case "X_OUT_JOGMINUS":
                    if (value > 0) motionItems[(int)AXIS_INDEX.X].MoveJog(velocity: -xCommandVelocity);
                    break;
                case "Y_OUT_JOGMINUS":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Y].MoveJog(velocity: -yCommandVelocity);
                    break;
                case "T_OUT_JOGMINUS":
                    if (value > 0) motionItems[(int)AXIS_INDEX.T].MoveJog(velocity: -tCommandVelocity);
                    break;
                case "Z1_OUT_JOGMINUS":
                case "Z2_OUT_JOGMINUS":
                case "Z3_OUT_JOGMINUS":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Z].MoveJog(velocity: -zCommandVelocity);
                    break;
                case "X_OUT_JOGSTOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.X].Stop();
                    break;
                case "Y_OUT_JOGSTOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Y].Stop();
                    break;
                case "T_OUT_JOGSTOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.T].Stop();
                    break;
                case "Z1_OUT_JOGSTOP":
                case "Z2_OUT_JOGSTOP":
                case "Z3_OUT_JOGSTOP":
                    if (value > 0) motionItems[(int)AXIS_INDEX.Z].Stop();
                    break;
                default:
                    result = false;
                    break;
            }
        }

        public void SET_STRING_OUT(string command, string value, ref bool result)
        {
            throw new NotImplementedException();
        }

        private SettingOptions settingFileParse()
        {
            string filePath = Directory.GetCurrentDirectory() + "\\" + settingFileName;
            string json = File.ReadAllText(filePath);

            try
            {
                // JSON을 C# 객체로 변환
                var data = JsonConvert.DeserializeObject<RootObject>(json);

                return data.SettingOptions;
            }
            catch (Exception e)
            {
                logger.Error(e, "Error while reading appsettings.json");
                return null;
            }
        }

        #region private fields
        private ILogger logger;
        private DevMode devMode = DevMode.DISCONNECT;
        private bool simulationMode = false;
        private ACSEmulator emulator;
        private ACSDriver acsDriver;
        private SettingOptions settingOptions;
        private List<ACSMotionItem> motionItems = new List<ACSMotionItem>();
        private const string settingFileName = "device.acs.settings.json";
        private bool disposed = false;

        private double xCommandVelocity = 0.0;
        private double yCommandVelocity = 0.0;
        private double zCommandVelocity = 0.0;
        private double tCommandVelocity = 0.0;
        #endregion

        private void DeviceClose()
        {
            foreach (var item in motionItems)
            {
                item.Dispose();
            }

            // Dispose unmanaged resources.
            if (acsDriver.Api != null && acsDriver.Api.IsConnected)
            {
                acsDriver.Api.CloseComm();
            }
        }
    }
}
