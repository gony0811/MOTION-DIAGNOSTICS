using EPLE.Data;
using EPLE.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionDiagnostics.Common
{
    internal partial class DatabaseInitializeHelper
    {
        public static DeviceConfigEntity[] DeviceConfig = new[]
        {
        new DeviceConfigEntity { DeviceName = "Dummy", DeviceType = "Dummy", InstanceName = "Device.Dummy", FileName = "Device.Dummy.dll", IsUse = true, Args = "TEST_DEVICE", Description = "Dummy Device" },
        new DeviceConfigEntity { DeviceName = "Camera", DeviceType = "VISION", InstanceName = "Device.CAM", FileName = "Device.CAM.dll", IsUse = false, Args = "0", Description = "PC CAM Device"},
        new DeviceConfigEntity { DeviceName = "PMAC", DeviceType = "MOTION", InstanceName = "Device.PowerPMAC", FileName = "Device.PowerPMAC.dll", IsUse = false, Args = "SIMULATION_MODE", Description = "Power PMAC Motion Controller" },
        new DeviceConfigEntity { DeviceName = "ACS", DeviceType = "MOTION", InstanceName = "Device.ACSMotionControl", FileName = "Device.ACSMotionControl.dll", IsUse = true, Args = "", Description = "ACS Motion Controller" },
        };


        public static DataConfigEntity[] DataConfig = new[]
        {
            new DataConfigEntity { Name = "CAM.IN.PICTURE", Module = "CAM", Type = DataType.OBJECT, Direction = Direction.IN, DeviceName = "VIRTUAL", Command = "GET_PICTURE", PollingTime = 30, IsUse = true, Description = "Camera Image Data"},

            new DataConfigEntity { Name = "X.IN.CALIBRATED", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IS_CALIBRATED", PollingTime = 100, IsUse = true, Description = "X Axis Home Status"},
            new DataConfigEntity { Name = "Y.IN.CALIBRATED", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IS_CALIBRATED", PollingTime = 100, IsUse = true, Description = "Y Axis Home Status"},
            new DataConfigEntity { Name = "T.IN.CALIBRATED", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IS_CALIBRATED", PollingTime = 100, IsUse = true, Description = "T Axis Home Status"},
            new DataConfigEntity { Name = "Z1.IN.CALIBRATED", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IS_CALIBRATED", PollingTime = 100, IsUse = true, Description = "Z1 Axis Home Status"},
            new DataConfigEntity { Name = "Z2.IN.CALIBRATED", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IS_CALIBRATED", PollingTime = 100, IsUse = true, Description = "Z2 Axis Home Status"},
            new DataConfigEntity { Name = "Z3.IN.CALIBRATED", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IS_CALIBRATED", PollingTime = 100, IsUse = true, Description = "Z3 Axis Home Status"},

            new DataConfigEntity { Name = "X.OUT.HOME", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_EXEC_HOME", PollingTime = 0, IsUse = true, Description = "X Axis Home Execute"},
            new DataConfigEntity { Name = "Y.OUT.HOME", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_EXEC_HOME", PollingTime = 0, IsUse = true, Description = "Y Axis Home Execute"},
            new DataConfigEntity { Name = "T.OUT.HOME", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_EXEC_HOME", PollingTime = 0, IsUse = true, Description = "T Axis Home Execute"},
            new DataConfigEntity { Name = "Z1.OUT.HOME", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_EXEC_HOME", PollingTime = 0, IsUse = true, Description = "Z1 Axis Home Execute"},
            new DataConfigEntity { Name = "Z2.OUT.HOME", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_EXEC_HOME", PollingTime = 0, IsUse = true, Description = "Z2 Axis Home Execute"},
            new DataConfigEntity { Name = "Z3.OUT.HOME", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_EXEC_HOME", PollingTime = 0, IsUse = true, Description = "Z3 Axis Home Execute"},

            new DataConfigEntity { Name = "X.IN.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_ENABLE", PollingTime = 100, IsUse = true, Description = "X Axis Servo Enable Status"},
            new DataConfigEntity { Name = "Y.IN.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_ENABLE", PollingTime = 100, IsUse = true, Description = "Y Axis Servo Enable Status"},
            new DataConfigEntity { Name = "T.IN.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_ENABLE", PollingTime = 100, IsUse = true, Description = "T Axis Servo Enable Status"},
            new DataConfigEntity { Name = "Z1.IN.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_ENABLE", PollingTime = 100, IsUse = true, Description = "Z1 Axis Servo Enable Status"},
            new DataConfigEntity { Name = "Z2.IN.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_ENABLE", PollingTime = 100, IsUse = true, Description = "Z2 Axis Servo Enable Status"},
            new DataConfigEntity { Name = "Z3.IN.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_ENABLE", PollingTime = 100, IsUse = true, Description = "Z3 Axis Servo Enable Status"},

            new DataConfigEntity { Name = "X.OUT.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_ENABLE", PollingTime = 0, IsUse = true, Description = "X Axis Servo Enable/Disable Set"},
            new DataConfigEntity { Name = "Y.OUT.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_ENABLE", PollingTime = 0, IsUse = true, Description = "Y Axis Servo Enable/Disable Set"},
            new DataConfigEntity { Name = "T.OUT.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_ENABLE", PollingTime = 0, IsUse = true, Description = "T Axis Servo Enable/Disable Set"},
            new DataConfigEntity { Name = "Z1.OUT.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_ENABLE", PollingTime = 0, IsUse = true, Description = "Z1 Axis Servo Enable/Disable Set"},
            new DataConfigEntity { Name = "Z2.OUT.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_ENABLE", PollingTime = 0, IsUse = true, Description = "Z2 Axis Servo Enable/Disable Set"},
            new DataConfigEntity { Name = "Z3.OUT.ENABLE", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_ENABLE", PollingTime = 0, IsUse = true, Description = "Z3 Axis Servo Enable/Disable Set"},

            new DataConfigEntity { Name = "X.OUT.MOVEREL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_MOVEREL", PollingTime = 0, IsUse = true, Description = "X Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "Y.OUT.MOVEREL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_MOVEREL", PollingTime = 0, IsUse = true, Description = "Y Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "T.OUT.MOVEREL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_MOVEREL", PollingTime = 0, IsUse = true, Description = "T Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "Z1.OUT.MOVEREL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_MOVEREL", PollingTime = 0, IsUse = true, Description = "Z1 Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "Z2.OUT.MOVEREL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_MOVEREL", PollingTime = 0, IsUse = true, Description = "Z2 Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "Z3.OUT.MOVEREL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_MOVEREL", PollingTime = 0, IsUse = true, Description = "Z3 Axis Servo Move Relative Set"},

            new DataConfigEntity { Name = "X.OUT.MOVEABS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_MOVEABS", PollingTime = 0, IsUse = true, Description = "X Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "Y.OUT.MOVEABS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_MOVEABS", PollingTime = 0, IsUse = true, Description = "Y Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "T.OUT.MOVEABS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_MOVEABS", PollingTime = 0, IsUse = true, Description = "T Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "Z1.OUT.MOVEABS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_MOVEABS", PollingTime = 0, IsUse = true, Description = "Z1 Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "Z2.OUT.MOVEABS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_MOVEABS", PollingTime = 0, IsUse = true, Description = "Z2 Axis Servo Move Relative Set"},
            new DataConfigEntity { Name = "Z3.OUT.MOVEABS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_MOVEABS", PollingTime = 0, IsUse = true, Description = "Z3 Axis Servo Move Relative Set"},

            new DataConfigEntity { Name = "X.OUT.STOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_STOP", PollingTime = 0, IsUse = true, DataResetTimeout=1000, Description = "X Axis Servo Stop"},
            new DataConfigEntity { Name = "Y.OUT.STOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_STOP", PollingTime = 0, IsUse = true, DataResetTimeout=1000, Description = "Y Axis Servo Stop"},
            new DataConfigEntity { Name = "T.OUT.STOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_STOP", PollingTime = 0, IsUse = true, DataResetTimeout=1000, Description = "T Axis Servo Stop"},
            new DataConfigEntity { Name = "Z1.OUT.STOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_STOP", PollingTime = 0, IsUse = true, DataResetTimeout = 1000, Description = "Z1 Axis Servo Stop"},
            new DataConfigEntity { Name = "Z2.OUT.STOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_STOP", PollingTime = 0, IsUse = true, DataResetTimeout = 1000, Description = "Z2 Axis Servo Stop"},
            new DataConfigEntity { Name = "Z3.OUT.STOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_STOP", PollingTime = 0, IsUse = true, DataResetTimeout = 1000, Description = "Z3 Axis Servo Stop"},

            new DataConfigEntity { Name = "X.OUT.ESTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_ESTOP", PollingTime = 0, IsUse = true, DataResetTimeout=1000, Description = "X Axis Servo EStop"},
            new DataConfigEntity { Name = "Y.OUT.ESTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_ESTOP", PollingTime = 0, IsUse = true, DataResetTimeout = 1000, Description = "Y Axis Servo EStop"},
            new DataConfigEntity { Name = "T.OUT.ESTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_ESTOP", PollingTime = 0, IsUse = true, DataResetTimeout = 1000, Description = "T Axis Servo EStop"},
            new DataConfigEntity { Name = "Z1.OUT.ESTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_ESTOP", PollingTime = 0, IsUse = true, DataResetTimeout = 1000, Description = "Z1 Axis Servo EStop"},
            new DataConfigEntity { Name = "Z2.OUT.ESTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_ESTOP", PollingTime = 0, IsUse = true, DataResetTimeout = 1000, Description = "Z2 Axis Servo EStop"},
            new DataConfigEntity { Name = "Z3.OUT.ESTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_ESTOP", PollingTime = 0, IsUse = true, DataResetTimeout = 1000, Description = "Z3 Axis Servo EStop"},

            new DataConfigEntity { Name = "X.IN.INPOS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_INPOS", PollingTime = 100, IsUse = true, Description = "X Axis Servo In-Position Status"},
            new DataConfigEntity { Name = "Y.IN.INPOS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_INPOS", PollingTime = 100, IsUse = true, Description = "Y Axis Servo In-Position Status"},
            new DataConfigEntity { Name = "T.IN.INPOS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_INPOS", PollingTime = 100, IsUse = true, Description = "T Axis Servo In-Position Status"},
            new DataConfigEntity { Name = "Z1.IN.INPOS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_INPOS", PollingTime = 100, IsUse = true, Description = "Z1 Axis Servo In-Position Status"},
            new DataConfigEntity { Name = "Z2.IN.INPOS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_INPOS", PollingTime = 100, IsUse = true, Description = "Z2 Axis Servo In-Position Status"},
            new DataConfigEntity { Name = "Z3.IN.INPOS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_INPOS", PollingTime = 100, IsUse = true, Description = "Z3 Axis Servo In-Position Status"},

            new DataConfigEntity { Name = "X.IN.ERROR", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_ERROR", PollingTime = 100, IsUse = true, Description = "X Axis Servo Error Status"},
            new DataConfigEntity { Name = "Y.IN.ERROR", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_ERROR", PollingTime = 100, IsUse = true, Description = "Y Axis Servo Error Status"},
            new DataConfigEntity { Name = "T.IN.ERROR", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_ERROR", PollingTime = 100, IsUse = true, Description = "T Axis Servo Error Status"},
            new DataConfigEntity { Name = "Z1.IN.ERROR", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_ERROR", PollingTime = 100, IsUse = true, Description = "Z1 Axis Servo Error Status"},
            new DataConfigEntity { Name = "Z2.IN.ERROR", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_ERROR", PollingTime = 100, IsUse = true, Description = "Z2 Axis Servo Error Status"},
            new DataConfigEntity { Name = "Z3.IN.ERROR", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_ERROR", PollingTime = 100, IsUse = true, Description = "Z3 Axis Servo Error Status"},

            new DataConfigEntity { Name = "X.IN.LIMITPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_LIMITPLUS", PollingTime = 100, IsUse = true, Description = "X Axis Servo Plus Limit Sensor Status"},
            new DataConfigEntity { Name = "Y.IN.LIMITPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_LIMITPLUS", PollingTime = 100, IsUse = true, Description = "Y Axis Servo Plus Limit Sensor Status"},
            new DataConfigEntity { Name = "T.IN.LIMITPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_LIMITPLUS", PollingTime = 100, IsUse = true, Description = "T Axis Servo Plus Limit Sensor Status"},
            new DataConfigEntity { Name = "Z1.IN.LIMITPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_LIMITPLUS", PollingTime = 100, IsUse = true, Description = "Z1 Axis Servo Plus Limit Sensor Status"},
            new DataConfigEntity { Name = "Z2.IN.LIMITPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_LIMITPLUS", PollingTime = 100, IsUse = true, Description = "Z2 Axis Servo Plus Limit Sensor Status"},
            new DataConfigEntity { Name = "Z3.IN.LIMITPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_LIMITPLUS", PollingTime = 100, IsUse = true, Description = "Z3 Axis Servo Plus Limit Sensor Status"},

            new DataConfigEntity { Name = "X.IN.LIMITMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_LIMITMINUS", PollingTime = 100, IsUse = true, Description = "X Axis Servo Minus Limit Sensor Status"},
            new DataConfigEntity { Name = "Y.IN.LIMITMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_LIMITMINUS", PollingTime = 100, IsUse = true, Description = "Y Axis Servo Minus Limit Sensor Status"},
            new DataConfigEntity { Name = "T.IN.LIMITMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_LIMITMINUS", PollingTime = 100, IsUse = true, Description = "T Axis Servo Minus Limit Sensor Status"},
            new DataConfigEntity { Name = "Z1.IN.LIMITMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_LIMITMINUS", PollingTime = 100, IsUse = true, Description = "Z1 Axis Servo Minus Limit Sensor Status"},
            new DataConfigEntity { Name = "Z2.IN.LIMITMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_LIMITMINUS", PollingTime = 100, IsUse = true, Description = "Z2 Axis Servo Minus Limit Sensor Status"},
            new DataConfigEntity { Name = "Z3.IN.LIMITMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_LIMITMINUS", PollingTime = 100, IsUse = true, Description = "Z3 Axis Servo Minus Limit Sensor Status"},

            new DataConfigEntity { Name = "X.IN.ACTPOS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_ACTPOS", PollingTime = 100, IsUse = true, Description = "X Axis Servo actual position"},
            new DataConfigEntity { Name = "Y.IN.ACTPOS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_ACTPOS", PollingTime = 100, IsUse = true, Description = "Y Axis Servo actual position"},
            new DataConfigEntity { Name = "T.IN.ACTPOS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_ACTPOS", PollingTime = 100, IsUse = true, Description = "T Axis Servo actual position"},
            new DataConfigEntity { Name = "Z1.IN.ACTPOS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_ACTPOS", PollingTime = 100, IsUse = true, Description = "Z1 Axis Servo actual position"},
            new DataConfigEntity { Name = "Z2.IN.ACTPOS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_ACTPOS", PollingTime = 100, IsUse = true, Description = "Z2 Axis Servo actual position"},
            new DataConfigEntity { Name = "Z3.IN.ACTPOS", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_ACTPOS", PollingTime = 100, IsUse = true, Description = "Z3 Axis Servo actual position"},

            new DataConfigEntity { Name = "X.OUT.VELOCITY", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_VELOCITY", PollingTime = 0, IsUse = true, DefaultValue="0.0", Description = "X Axis Servo target velocity"},
            new DataConfigEntity { Name = "Y.OUT.VELOCITY", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_VELOCITY", PollingTime = 0, IsUse = true, DefaultValue="0.0", Description = "Y Axis Servo target velocity"},
            new DataConfigEntity { Name = "T.OUT.VELOCITY", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_VELOCITY", PollingTime = 0, IsUse = true, DefaultValue="0.0", Description = "T Axis Servo target velocity"},
            new DataConfigEntity { Name = "Z1.OUT.VELOCITY", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_VELOCITY", PollingTime = 0, IsUse = true, DefaultValue="0.0", Description = "Z1 Axis Servo target velocity"},
            new DataConfigEntity { Name = "Z2.OUT.VELOCITY", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_VELOCITY", PollingTime = 0, IsUse = true, DefaultValue="0.0", Description = "Z2 Axis Servo target velocity"},
            new DataConfigEntity { Name = "Z3.OUT.VELOCITY", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_VELOCITY", PollingTime = 0, IsUse = true, DefaultValue="0.0", Description = "Z3 Axis Servo target velocity"},

            new DataConfigEntity { Name = "X.IN.ACTVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_ACTVEL", PollingTime = 0, IsUse = true, Description = "X Axis Servo current velocity"},
            new DataConfigEntity { Name = "Y.IN.ACTVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_ACTVEL", PollingTime = 0, IsUse = true, Description = "Y Axis Servo current velocity"},
            new DataConfigEntity { Name = "T.IN.ACTVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_ACTVEL", PollingTime = 0, IsUse = true, Description = "T Axis Servo current velocity"},
            new DataConfigEntity { Name = "Z1.IN.ACTVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_ACTVEL", PollingTime = 0, IsUse = true, Description = "Z1 Axis Servo current velocity"},
            new DataConfigEntity { Name = "Z2.IN.ACTVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_ACTVEL", PollingTime = 0, IsUse = true, Description = "Z2 Axis Servo current velocity"},
            new DataConfigEntity { Name = "Z3.IN.ACTVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_ACTVEL", PollingTime = 0, IsUse = true, Description = "Z3 Axis Servo current velocity"},


            new DataConfigEntity { Name = "X.IN.CMDVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_CMDVEL", PollingTime = 0, IsUse = true, Description = "X Axis Servo command velocity"},
            new DataConfigEntity { Name = "Y.IN.CMDVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_CMDVEL", PollingTime = 0, IsUse = true, Description = "Y Axis Servo command velocity"},
            new DataConfigEntity { Name = "T.IN.CMDVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_CMDVEL", PollingTime = 0, IsUse = true, Description = "T Axis Servo command velocity"},
            new DataConfigEntity { Name = "Z1.IN.CMDVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_CMDVEL", PollingTime = 0, IsUse = true, Description = "Z1 Axis Servo command velocity"},
            new DataConfigEntity { Name = "Z2.IN.CMDVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_CMDVEL", PollingTime = 0, IsUse = true, Description = "Z2 Axis Servo command velocity"},
            new DataConfigEntity { Name = "Z3.IN.CMDVEL", Module = "MOTION", Type = DataType.DOUBLE, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_CMDVEL", PollingTime = 0, IsUse = true, Description = "Z3 Axis Servo command velocity"},

            // 250121_mh.yun
            new DataConfigEntity { Name = "X.OUT.JOGPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_JOGPLUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "X Axis Servo JogMove Plus"},
            new DataConfigEntity { Name = "Y.OUT.JOGPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_JOGPLUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Y Axis Servo JogMove Plus"},
            new DataConfigEntity { Name = "T.OUT.JOGPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_JOGPLUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "T Axis Servo JogMove Plus"},
            new DataConfigEntity { Name = "Z1.OUT.JOGPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_JOGPLUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z1 Axis Servo JogMove Plus"},
            new DataConfigEntity { Name = "Z2.OUT.JOGPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_JOGPLUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z2 Axis Servo JogMove Plus"},
            new DataConfigEntity { Name = "Z3.OUT.JOGPLUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_JOGPLUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z3 Axis Servo JogMove Plus"},

            new DataConfigEntity { Name = "X.OUT.JOGMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_JOGMINUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "X Axis Servo JogMove Minus"},
            new DataConfigEntity { Name = "Y.OUT.JOGMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_JOGMINUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Y Axis Servo JogMove Minus"},
            new DataConfigEntity { Name = "T.OUT.JOGMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_JOGMINUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "T Axis Servo JogMove Minus"},
            new DataConfigEntity { Name = "Z1.OUT.JOGMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_JOGMINUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z1 Axis Servo JogMove Minus"},
            new DataConfigEntity { Name = "Z2.OUT.JOGMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_JOGMINUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z2 Axis Servo JogMove Minus"},
            new DataConfigEntity { Name = "Z3.OUT.JOGMINUS", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_JOGMINUS", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z3 Axis Servo JogMove Minus"},

            new DataConfigEntity { Name = "X.OUT.JOGSTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "X_OUT_JOGSTOP", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "X Axis Servo JogMove Stop"},
            new DataConfigEntity { Name = "Y.OUT.JOGSTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Y_OUT_JOGSTOP", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Y Axis Servo JogMove Stop"},
            new DataConfigEntity { Name = "T.OUT.JOGSTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "T_OUT_JOGSTOP", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "T Axis Servo JogMove Stop"},
            new DataConfigEntity { Name = "Z1.OUT.JOGSTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z1_OUT_JOGSTOP", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z1 Axis Servo JogMove Stop"},
            new DataConfigEntity { Name = "Z2.OUT.JOGSTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z2_OUT_JOGSTOP", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z2 Axis Servo JogMove Stop"},
            new DataConfigEntity { Name = "Z3.OUT.JOGSTOP", Module = "MOTION", Type = DataType.INT, Direction = Direction.OUT, DeviceName = "ACS", Command = "Z3_OUT_JOGSTOP", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Z3 Axis Servo JogMove Stop"},


            new DataConfigEntity { Name = "X.IN.BUSY", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "X_IN_BUSY", PollingTime = 100, IsUse = true, Description = "X Axis Servo Busy Status"},
            new DataConfigEntity { Name = "Y.IN.BUSY", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Y_IN_BUSY", PollingTime = 100, IsUse = true, Description = "Y Axis Servo Busy Status"},
            new DataConfigEntity { Name = "T.IN.BUSY", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "T_IN_BUSY", PollingTime = 100, IsUse = true, Description = "T Axis Servo Busy Status"},
            new DataConfigEntity { Name = "Z1.IN.BUSY", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z1_IN_BUSY", PollingTime = 100, IsUse = true, Description = "Z1 Axis Servo Busy Status"},
            new DataConfigEntity { Name = "Z2.IN.BUSY", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z2_IN_BUSY", PollingTime = 100, IsUse = true, Description = "Z2 Axis Servo Busy Status"},
            new DataConfigEntity { Name = "Z3.IN.BUSY", Module = "MOTION", Type = DataType.INT, Direction = Direction.IN, DeviceName = "ACS", Command = "Z3_IN_BUSY", PollingTime = 100, IsUse = true, Description = "Z3 Axis Servo Busy Status"},


            /// <summary>
            /// VIRTUAL
            /// </summary>
            /// 
            
            /// <summary>
            /// X.LOADING.POSITION
            /// Wafer Loading X 좌표
            /// </summary>
            new DataConfigEntity { Name = "X.LOADING.POSITION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer Loading X Postion"},

              /// <summary>
            /// Y.LOADING.VELOCITY
            /// Wafer Loading X VELOCITY
            /// </summary>
            new DataConfigEntity { Name = "X.LOADING.VELOCITY", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, DefaultValue="100", IsUse = true, Description = "Wafer Loading X VELOCITY"},

              /// <summary>
            /// X.MENUAL.VELOCITY
            /// Wafer Manual X VELOCITY
            /// </summary>
            new DataConfigEntity { Name = "X.MANUAL.VELOCITY", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, DefaultValue="100", IsUse = true, Description = "Wafer Manual X VELOCITY"},


            /// <summary>
            /// Y.LOADING.POSITION
            /// Wafer Loading Y 좌표
            /// </summary>
            new DataConfigEntity { Name = "Y.LOADING.POSITION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer Loading Y Postion"},

            /// <summary>
            /// Y.LOADING.VELOCITY
            /// Wafer Loading X VELOCITY
            /// </summary>
            new DataConfigEntity { Name = "Y.LOADING.VELOCITY", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, DefaultValue="100", IsUse = true, Description = "Wafer Loading Y VELOCITY"},

            /// <summary>
            /// Y.MANUAL.VELOCITY
            /// Wafer Manual Y VELOCITY
            /// </summary>
            new DataConfigEntity { Name = "Y.MANUAL.VELOCITY", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, DefaultValue="100", IsUse = true, Description = "Wafer Manual Y VELOCITY"},


            /// <summary>
            /// X.CENTER.POSITION
            /// Wafer의 중심 X 좌표
            /// </summary>
            new DataConfigEntity { Name = "X.CENTER.POSITION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer Center X Postion"},

            /// <summary>
            /// Y.CENTER.POSITION
            /// Wafer의 중심 Y 좌표
            /// </summary>
            new DataConfigEntity { Name = "Y.CENTER.POSITION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer Center Y Postion"},

            /// <summary>
            /// X.LEFTEDGE.POSITION
            /// Wafer Loading X 좌표
            /// </summary>
            new DataConfigEntity { Name = "X.LEFTEDGE.POSITION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer LeftEdge X Postion"},

            /// <summary>
            /// Y.LEFTEDGE.POSITION
            /// Wafer Left Edge X 좌표
            /// </summary>
            new DataConfigEntity { Name = "Y.LEFTEDGE.POSITION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer LeftEdge Y Postion"},

            /// <summary>
            /// X.RIGHTEDGE.POSITION
            /// Wafer Left Edge X 좌표
            /// </summary>
            new DataConfigEntity { Name = "X.RIGHTEDGE.POSITION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer LeftEdge X Postion"},

            /// <summary>
            /// Y.RIGHTEDGE.POSITION
            /// Wafer Loading X 좌표
            /// </summary>
            new DataConfigEntity { Name = "Y.RIGHTEDGE.POSITION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer RightEdge Y Postion"},



            /// <summary>
            /// X.CENTER.INDEX
            /// Wafer의 중심 X INDEX
            /// </summary>
            new DataConfigEntity { Name = "X.CENTER.INDEX", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="29", Description = "Wafer Center X Index"},

            /// <summary>
            /// Y.CENTER.INDEX
            /// Wafer의 중심 Y INDEX
            /// </summary>
            new DataConfigEntity { Name = "Y.CENTER.INDEX", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="29", Description = "Wafer Center Y Index"},

            /// <summary>
            /// MARK.X.OFFSET
            /// VISION으로 찾은 MARK의 X Offset
            /// </summary>
            new DataConfigEntity { Name = "MARK.X.OFFSET", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="29", Description = "Vision Mark X Offset"},

            /// <summary>
            /// MARK.Y.OFFSET
            /// VISION으로 찾은 MARK의 Y Offset
            /// </summary>
            new DataConfigEntity { Name = "MARK.Y.OFFSET", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="29", Description = "Vision Mark Y Offset"},

            /// <summary>
            /// SET.X.INDEX
            /// Wafer를 Pitch로 나눈 Column의 Index
            /// </summary>
            new DataConfigEntity { Name = "SET.X.INDEX", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer Center Column Index"},

            /// <summary>
            /// SET.Y.INDEX
            /// Wafer를 Pitch로 나눈 Row의 Index
            /// </summary>
            new DataConfigEntity { Name = "SET.Y.INDEX", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Wafer Center Row Index"},


            /// <summary>
            /// SET.GRID.ROW
            /// Wafer Grid의 X축 방향 Cell 개수
            /// </summary>
            new DataConfigEntity { Name = "SET.GRID.ROW", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="60", Description = "Wafer Grid Row Count"},

            /// <summary>
            /// SET.GRID.COL
            /// Wafer Grid의 Y축 방향 Cell 개수
            /// </summary>
            new DataConfigEntity { Name = "SET.GRID.COL", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="60", Description = "Wafer Grid Column Count"},

            /// <summary>
            /// SET.GRID.PITCH
            /// Wafer Grid의 Y축 방향 Cell 개수
            /// </summary>
            new DataConfigEntity { Name = "SET.GRID.PITCH", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="5", Description = "Wafer Grid Column Count"},

            /// <summary>
            /// SET.OK.COUNT
            /// 측정에 대한 정상 OK Count
            /// </summary>
            new DataConfigEntity { Name = "SET.OK.COUNT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Inspection OK Count"},

            /// <summary>
            /// SET.NG.COUNT
            /// 측정에 대한 정상 NG Count
            /// </summary>
            new DataConfigEntity { Name = "SET.NG.COUNT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Inspection NG Count"},

            /// <summary>
            /// SET.REPEAT.COUNT
            /// 측정에 대한 정상 REPEAT Count
            /// </summary>
            new DataConfigEntity { Name = "SET.REPEAT.COUNT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, Description = "Inspection Repeat Count"},

            /// <summary>
            /// SET.MOTION.TIMEOUT
            /// MOTION 이동에 대한 Timeout
            /// 모든 축에 대해 동일한 Timeout을 사용, 모든 모션 이동은 Timeout 시간 이내에 이루어져야함
            /// default 10000ms (10초)
            /// </summary>
            new DataConfigEntity { Name = "SET.MOTION.TIMEOUT", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="10000.0", Description = "Motion Timeout"},

            /// <summary>
            /// CAM.X.RESOLUTION
            /// Camera Resolution X(mm/pixel)
            /// X 방향 카메라 해상도
            /// default 1.0 mm
            /// </summary>
            new DataConfigEntity { Name = "CAM.X.RESOLUTION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="0.0025", Description = "Camera Resolution X(mm/pixel)"},

            /// <summary>
            /// CAM.Y.RESOLUTION
            /// Camera Resolution Y(mm/pixel)
            /// X 방향 카메라 해상도
            /// default 1.0 mm
            /// </summary>
            new DataConfigEntity { Name = "CAM.Y.RESOLUTION", Module = "VIRTUAL", Type = DataType.DOUBLE, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="0.0025", Description = "Camera Resolution Y(mm/pixel)"},

            /// <summary>
            /// X.DIRECTION.INVERT
            /// X 축 이동 방향 Invert
            /// 카메라 좌표 기준으로 X축 이동 방향을 Invert
            /// JOG 방향을 원하는 방향으로 설정하기 위함
            /// -1 : 반대방향, 1 : 설정된 모터 이동방향과 동일
            /// </summary>
            new DataConfigEntity { Name = "X.DIRECTION.INVERT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="1", Description = "X 축 이동 방향 Invert"},

            /// <summary>
            /// Y.DIRECTION.INVERT
            /// Y 축 이동 방향 Invert
            /// 카메라 좌표 기준으로 Y축 이동 방향을 Invert
            /// JOG 방향을 원하는 방향으로 설정하기 위함
            /// -1 : 반대방향, 1 : 설정된 모터 이동방향과 동일
            /// </summary>
            new DataConfigEntity { Name = "Y.DIRECTION.INVERT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="1", Description = "Y 축 이동 방향 Invert"},

            /// <summary>
            /// T.DIRECTION.INVERT
            /// T 축 이동 방향 Invert
            /// 카메라 좌표 기준으로 T축 이동 방향을 Invert
            /// JOG 방향을 원하는 방향으로 설정하기 위함
            /// -1 : 반대방향, 1 : 설정된 모터 이동방향과 동일
            /// </summary>
            new DataConfigEntity { Name = "T.DIRECTION.INVERT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="1", Description = "T 축 이동 방향 Invert"},

            /// <summary>
            /// Z1.DIRECTION.INVERT
            /// Z1 축 이동 방향 Invert
            /// 카메라 좌표 기준으로 Z1축 이동 방향을 Invert
            /// JOG 방향을 원하는 방향으로 설정하기 위함
             /// -1 : 반대방향, 1 : 설정된 모터 이동방향과 동일
            /// </summary>
            new DataConfigEntity { Name = "Z1.DIRECTION.INVERT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="1", Description = "Z1 축 이동 방향 Invert"},

            /// <summary>
            /// Z2.DIRECTION.INVERT
            /// Z2 축 이동 방향 Invert
            /// 카메라 좌표 기준으로 Z2축 이동 방향을 Invert
            /// JOG 방향을 원하는 방향으로 설정하기 위함
             /// -1 : 반대방향, 1 : 설정된 모터 이동방향과 동일
            /// </summary>
            new DataConfigEntity { Name = "Z2.DIRECTION.INVERT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="1", Description = "Z2 축 이동 방향 Invert"},

            /// <summary>
            /// Z3.DIRECTION.INVERT
            /// Z3 축 이동 방향 Invert
            /// 카메라 좌표 기준으로 Z3축 이동 방향을 Invert
            /// JOG 방향을 원하는 방향으로 설정하기 위함
             /// -1 : 반대방향, 1 : 설정된 모터 이동방향과 동일
            /// </summary>
            new DataConfigEntity { Name = "Z3.DIRECTION.INVERT", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="1", Description = "Z3 축 이동 방향 Invert"},

            /// <summary>
            /// WAFER.LOADING.COMPLETED
            /// Wafer Loading 완료 여부
            /// wafer를 로딩했을 때 UI에서 사람이 수동으로 완료 체크
             /// 0 : 로딩 않됨, 1 : 로딩완료
            /// </summary>
            new DataConfigEntity { Name = "WAFER.LOADING.COMPLETED", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "Wafer Loading 완료 여부"},

            /// <summary>
            /// SYS.MODE.SIMULATION
            /// System이 Device와 Offline 환경에서 구동되도록 설정
             /// 0 : Online, 1 : Offline (Simulation)
            /// </summary>
            new DataConfigEntity { Name = "SYS.MODE.SIMULATION", Module = "VIRTUAL", Type = DataType.INT, Direction = Direction.BOTH, DeviceName = "VIRTUAL", Command = "", PollingTime = 0, IsUse = true, DefaultValue="0", Description = "시뮬레이션 구동 여부"},
        };
    }
}
