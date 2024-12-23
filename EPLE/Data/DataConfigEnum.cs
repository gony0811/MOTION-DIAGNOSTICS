namespace EPLE.Data
{
    public enum DataType 
    {
        INT,
        DOUBLE,
        STRING,
        OBJECT,
    }

    public enum Direction
    {
        IN,
        OUT,
        BOTH,
    }

    public enum Interlock { NONE, SETPOINT, SETVALUE }
}
