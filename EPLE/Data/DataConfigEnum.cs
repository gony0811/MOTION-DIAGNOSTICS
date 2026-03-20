using System.Runtime.Serialization;

namespace EPLE.Data
{
    public enum DataType
    {
        [EnumMember(Value = "INT")]
        INT,
        [EnumMember(Value = "DOUBLE")]
        DOUBLE,
        [EnumMember(Value = "STRING")]
        STRING,
        [EnumMember(Value = "OBJECT")]
        OBJECT,
    }

    public enum Direction
    {
        [EnumMember(Value = "IN")]
        IN,
        [EnumMember(Value = "OUT")]
        OUT,
        [EnumMember(Value = "BOTH")]
        BOTH,
    }

    /// <summary>
    /// Alarm Level Code Definitions
    /// </summary>
    public enum ALCD
    {
        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,
        [EnumMember(Value = "LIGHT")]
        LIGHT,
        [EnumMember(Value = "HEAVY")]
        HEAVY
    }

    /// <summary>
    /// Alarm Status Code Definitions
    /// </summary>
    public enum ALST
    {
        [EnumMember(Value = "UNKNOWN")]
        UNKNOWN,
        [EnumMember(Value = "SET")]
        SET,
        [EnumMember(Value = "RESET")]
        RESET
    }

    public enum ALED
    {
        [EnumMember(Value = "DISABLE")]
        DISABLE,
        [EnumMember(Value = "ENABLE")]
        ENABLE
    }

    public enum AXES : int
    {
        [EnumMember(Value = "X")]
        X,
        [EnumMember(Value = "Y")]
        Y,
        [EnumMember(Value = "T")]
        T,
        [EnumMember(Value = "Z1")]
        Z1,
        [EnumMember(Value = "Z2")]
        Z2,
        [EnumMember(Value = "Z3")]
        Z3,
    }

    public enum STATUS : int
    {
        [EnumMember(Value = "OFF")]
        OFF = 0,
        [EnumMember(Value = "ON")]
        ON = 1
    }

    public enum EXECUTE : int
    {
        [EnumMember(Value = "STOP")]
        STOP = 0,
        [EnumMember(Value = "START")]
        START = 1
    }

    public enum Interlock {
        [EnumMember(Value = "NONE")]
        NONE,
        [EnumMember(Value = "SETPOINT")]
        SETPOINT,
        [EnumMember(Value = "SETVALUE")]
        SETVALUE }

    public enum  MeasureResult 
    {
        [EnumMember(Value = "NONE")]
        NONE,
        [EnumMember(Value = "OK")]
        OK,
        [EnumMember(Value = "NG")]
        NG
    }
}
