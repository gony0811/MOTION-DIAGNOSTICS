using EPLE.Core.Device.Interface;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Serilog;

namespace Device
{
    /// <summary>
    /// Motor 맵
    /// </summary>
    public enum AXIS_INDEX : int
    {
        Y, // 0
        X,
        T,
        Z1,
        Z2,
        Z3,
    }


    public struct MoveParameter
    {
        public double Velocity;
        public int AccelerationTime;
        public int S_CurveTime;
    }
    public class PowerPMAC : IDeviceHandler
    {
        private PowerPmacMotion powerPmacMotion;
        private ILogger logger;
        private DevMode devMode;
        private double xTargetOffset = 0.0;
        private bool simulationMode = false;
        private MoveParameter moveParameter = new MoveParameter();

        private Dictionary<string, PowerPMACEmulator> emulators = new Dictionary<string, PowerPMACEmulator>();

        private void InitPowerPMACEmulator()
        {
            emulators.Clear();
            emulators.Add("Y", new PowerPMACEmulator("Y", 1));
            emulators.Add("X", new PowerPMACEmulator("X", 3));
            emulators.Add("T", new PowerPMACEmulator("T", 4));
            emulators.Add("Z1", new PowerPMACEmulator("Z1", 5));
            emulators.Add("Z2", new PowerPMACEmulator("Z2", 6));
            emulators.Add("Z3", new PowerPMACEmulator("Z3", 7));
        }

        public bool DeviceAttach(string ipAddress)
        {
            if (ipAddress == null || ipAddress.StartsWith("S"))
            {
                simulationMode = true;
                logger.Error("PowerPMAC IP address is null, Running Simulation Mode");
                devMode = DevMode.CONNECT;

                InitPowerPMACEmulator();

                return true;
            }

            var bInit = PMAC.Initialize(ipAddress);

            if (bInit)
            {
                powerPmacMotion = new PowerPmacMotion(
                 new PowerPmacMotionItem() // Y1
                 {
                     Index = 1,
                     Name = "Y1",
                     EncoderCountsPerUnit = 1_000,
                     HomeExucutionStartPlcNumber = 3,
                     HomeCompletePVarNumber = 8200,
                     HomingStateString = "Y1HomeState",
                     HomingCompleteString = "Y1HomeComplete",
                     HomeTimeout = 600_000
                 },
                        new PowerPmacMotionItem() // Y2
                        {
                            Index = 2,
                            Name = "Y2",
                        },
                        new PowerPmacMotionItem() // X
                        {
                            Index = 3,
                            Name = "X",
                            EncoderCountsPerUnit = 1_000,
                            HomeExucutionStartPlcNumber = 4,
                            HomeCompletePVarNumber = 8300,
                            HomingStateString = "XHomeState",
                            HomingCompleteString = "XHomeComplete",
                            HomeTimeout = 600_000
                        },
                        new PowerPmacMotionItem() // T
                        {
                            Index = 4,
                            Name = "Theta",
                            EncoderCountsPerUnit = 1_000,
                            HomeExucutionStartPlcNumber = 5,
                            HomeCompletePVarNumber = 8400,
                            HomingStateString = "THomeState",
                            HomingCompleteString = "THomeComplete",
                            HomeTimeout = 600_000
                        },
                        new PowerPmacMotionItem() // Z1
                        {
                            Index = 5,
                            Name = "Z1",
                            EncoderCountsPerUnit = 1_000,
                            HomeExucutionStartPlcNumber = 6,
                            HomeCompletePVarNumber = 8500,
                            HomingStateString = "Z1HomeState",
                            HomingCompleteString = "Z1HomeComplete",
                            HomeTimeout = 600_000
                        },
                        new PowerPmacMotionItem() // Z2
                        {
                            Index = 6,
                            Name = "Z2",
                            EncoderCountsPerUnit = 1_000,
                            HomeExucutionStartPlcNumber = 6,
                            HomeCompletePVarNumber = 8500,
                            HomingStateString = "Z2HomeState",
                            HomingCompleteString = "Z2HomeComplete",
                            HomeTimeout = 600_000
                        },
                        new PowerPmacMotionItem() // Z3
                        {
                            Index = 7,
                            Name = "Z3",
                            EncoderCountsPerUnit = 1_000,
                            HomeExucutionStartPlcNumber = 6,
                            HomeCompletePVarNumber = 8500,
                            HomingStateString = "Z3HomeState",
                            HomingCompleteString = "Z3HomeComplete",
                            HomeTimeout = 600_000
                        }
                        );
                logger.Information("PowerPMAC is attached.");

                devMode = DevMode.CONNECT;
                return true;
            }
            else
            {
                logger.Error("PowerPMAC attach is failed.");
                return false;
            }
        }

        public bool DeviceDettach()
        {
            if (powerPmacMotion == null || simulationMode)
            {
                logger.Information("PowerPMAC is simulation mode.");
                devMode = DevMode.DISCONNECT;
                emulators.Clear();
                return true;
            }
            else
            {
                powerPmacMotion.Dispose();
                PMAC.ReleaseInstance();
                devMode = DevMode.DISCONNECT;
                logger.Information("PowerPMAC is detached.");

                return true;
            }
        }

        public void DeviceInit(ILogger logger)
        {
            this.logger = logger;
            devMode = DevMode.DISCONNECT;
            logger.Information("PowerPMAC is initialized.");
        }

        public bool DeviceReset()
        {
            logger.Information("PowerPMAC is reset.");
            return true;
        }

        public object GET_DATA_IN(string command, ref bool result)
        {
            throw new NotImplementedException();
        }

        public double GET_DOUBLE_IN(string command, ref bool result)
        {
            var axis = string.Empty;

            switch (command)
            {
                case "X_IN_ACTPOS":
                case "Y_IN_ACTPOS":
                case "Z1_IN_ACTPOS":
                case "Z2_IN_ACTPOS":
                case "Z3_IN_ACTPOS":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        result = true;
                        return emulators[axis].ActualPosition;
                    }
                    else
                    {
                        result = true;
                        return powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].FeedbackPosition;
                    }
                case "X_IN_ACTVEL":
                case "Y_IN_ACTVEL":
                case "Z1_IN_ACTVEL":
                case "Z2_IN_ACTVEL":
                case "Z3_IN_ACTVEL":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        result = true;
                        return emulators[axis].ActualVelocity;
                    }
                    else
                    {
                        result = true;
                        return powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].MovingVelocity;
                    }
            }

            return 0.0;
        }

        public int GET_INT_IN(string command, ref bool result)
        {
            int value = 0;
            var axis = string.Empty;

            switch (command)
            {
                case "X_IN_ENABLE":
                case "Y_IN_ENABLE":
                case "Z1_IN_ENABLE":
                case "Z2_IN_ENABLE":
                case "Z3_IN_ENABLE":
                case "T_IN_ENABLE":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        result = true;
                        return emulators[axis].IsEnable ? 1 : 0;
                    }
                    else
                    {
                        result = true;
                        return powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].Enabled ? 1 : 0;
                    }
                case "X_IS_BUSY":
                case "Y_IS_BUSY":
                case "Z1_IS_BUSY":
                case "Z2_IS_BUSY":
                case "Z3_IS_BUSY":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        result = true;
                        return emulators[axis].Busy ? 1 : 0;
                    }
                    else
                    {
                        result = true;
                        return powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].Busy ? 1 : 0;
                    }
                case "X_IS_CALIBRATED":
                case "Y_IS_CALIBRATED":
                case "Z1_IS_CALIBRATED":
                case "Z2_IS_CALIBRATED":
                case "Z3_IS_CALIBRATED":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        result = true;
                        return emulators[axis].IsCalibrated ? 1 : 0;
                    }
                    else
                    {
                        result = true;
                        return powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].IsHomeDone ? 1 : 0;
                    }
                case "X_IN_INPOS":
                case "Y_IN_INPOS":
                case "Z1_IN_INPOS":
                case "Z2_IN_INPOS":
                case "Z3_IN_INPOS":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        result = true;
                        return emulators[axis].InPosition ? 1 : 0;
                    }
                    else
                    {
                        result = true;
                        return powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].Inposition ? 1 : 0;
                    }
                case "X_IN_LIMITPLUS":
                case "Y_IN_LIMITPLUS":
                case "Z1_IN_LIMITPLUS":
                case "Z2_IN_LIMITPLUS":
                case "Z3_IN_LIMITPLUS":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        result = true;
                        return emulators[axis].IsLimitPlus ? 1 : 0;
                    }
                    else
                    {
                        result = true;
                        return powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].CwLimitSensorDetection ? 1 : 0;
                    }
                case "X_IN_LIMITMINUS":
                case "Y_IN_LIMITMINUS":
                case "Z1_IN_LIMITMINUS":
                case "Z2_IN_LIMITMINUS":
                case "Z3_IN_LIMITMINUS":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        result = true;
                        return emulators[axis].IsLimitPlus ? 1 : 0;
                    }
                    else
                    {
                        result = true;
                        return powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].CCwLimitSensorDetection ? 1 : 0;
                    }
                default:
                    return value;
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
            var axis = string.Empty;

            switch (command)
            {
                case "X_OUT_VELOCITY":
                case "Y_OUT_VELOCITY":
                case "Z1_OUT_VELOCITY":
                case "Z2_OUT_VELOCITY":
                case "Z3_OUT_VELOCITY":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].SetVelocity = value;
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].SetCurrentMovingVelocity(value);
                        result = true;
                    }
                    break;

                case "X_OUT_MOVEREL":
                case "Y_OUT_MOVEREL":
                case "T_OUT_MOVEREL":
                case "Z1_OUT_MOVEREL":
                case "Z2_OUT_MOVEREL":
                case "Z3_OUT_MOVEREL":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].MoveRelative(value);
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].MoveRel(value);
                        result = true;
                    }
                    break;
                case "X_OUT_MOVEABS":
                case "Y_OUT_MOVEABS":
                case "T_OUT_MOVEABS":
                case "Z1_OUT_MOVEABS":
                case "Z2_OUT_MOVEABS":
                case "Z3_OUT_MOVEABS":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].MoveAbsolute(value);
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].MoveAbs(value);
                        result = true;
                    }
                    break;
                case "Y_MOVE_POSITION":
                    powerPmacMotion[Convert.ToInt32(AXIS_INDEX.Y)].MoveRel(value, moveParameter.Velocity, moveParameter.AccelerationTime, moveParameter.S_CurveTime);
                    break;
                case "X_CONFIG_VELOCITY":
                    moveParameter.Velocity = value;
                    break;


            }
        }

        public void SET_INT_OUT(string command, int value, ref bool result)
        {
            var axis = string.Empty;

            switch (command)
            {
                case "X_OUT_ENABLE":
                case "Y_OUT_ENABLE":
                case "T_OUT_ENABLE":
                case "Z1_OUT_ENABLE":
                case "Z2_OUT_ENABLE":
                case "Z3_OUT_ENABLE":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].Enable(value > 0);
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].Enable(value > 0);
                        result = true;
                    }
                    break;
                case "X_OUT_STOP":
                case "Y_OUT_STOP":
                case "T_OUT_STOP":
                case "Z1_OUT_STOP":
                case "Z2_OUT_STOP":
                case "Z3_OUT_STOP":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].EStop();
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].StopEmergency();
                        result = true;
                    }
                    break;
                case "X_OUT_ESTOP":
                case "Y_OUT_ESTOP":
                case "T_OUT_ESTOP":
                case "Z1_OUT_ESTOP":
                case "Z2_OUT_ESTOP":
                case "Z3_OUT_ESTOP":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].EStop();
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].Stop();
                        result = true;
                    }
                    break;
                case "X_EXEC_HOME":
                case "Y_EXEC_HOME":
                case "T_EXEC_HOME":
                case "Z1_EXEC_HOME":
                case "Z2_EXEC_HOME":
                case "Z3_EXEC_HOME":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].ExecuteHome();
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].ExecuteHoming();
                        result = true;
                    }
                    break;

                // 250121_mh.yun
                case "X_OUT_JOGPLUS":
                case "Y_OUT_JOGPLUS":
                case "T_OUT_JOGPLUS":
                case "Z1_OUT_JOGPLUS":
                case "Z2_OUT_JOGPLUS":
                case "Z3_OUT_JOGPLUS":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].JogPlus();
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].MoveJog(value);
                        result = true;
                    }
                    break;

                case "X_OUT_JOGMINUS":
                case "Y_OUT_JOGMINUS":
                case "T_OUT_JOGMINUS":
                case "Z1_OUT_JOGMINUS":
                case "Z2_OUT_JOGMINUS":
                case "Z3_OUT_JOGMINUS":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].JogMinus();
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].MoveJog(value);
                        result = true;
                    }
                    break;

                case "X_OUT_JOGSTOP":
                case "Y_OUT_JOGSTOP":
                case "T_OUT_JOGSTOP":
                case "Z1_OUT_JOGSTOP":
                case "Z2_OUT_JOGSTOP":
                case "Z3_OUT_JOGSTOP":
                    axis = command.Split('_')[0];
                    if (simulationMode)
                    {
                        emulators[axis].JogStop();
                        result = true;
                    }
                    else
                    {
                        powerPmacMotion[(int)Enum.Parse(typeof(AXIS_INDEX), axis)].Stop();
                        result = true;
                    }
                    break;
            }
        }

        public void SET_STRING_OUT(string command, string value, ref bool result)
        {
            // value = "X_MOVE_REL,100.0"
            switch (command)
            {
                case "X_MOTION_COMMAND":
                    var cmd = value.Split(',');
                    if (cmd[0] == "X_MOVE_REL")
                    {
                        powerPmacMotion[Convert.ToInt32(AXIS_INDEX.X)].MoveRel(Convert.ToDouble(cmd[1]));
                    }
                    break;
            }
        }
    }
}
