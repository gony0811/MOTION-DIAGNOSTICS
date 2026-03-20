using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPLE.Data
{
    public class DataNameHelper
    {
        enum SERVO
        {
            ENABLE = 1,
            DISABLE = 0
        }

        enum RunStop
        {
            RUN = 1,
            STOP = 0
        }

        enum ONOFF
        {
            ON = 1,
            OFF = 0
        }

        public static string X_IN_ENABLE = "X.IN.ENABLE";
        public static string X_IN_CALIBRATED = "X.IN.CALIBRATED";
        public static string X_IN_ACTPOS = "X.IN.ACTPOS";
        public static string X_IN_BUSY = "X.IN.BUSY";
        public static string X_OUT_HOME = "X.OUT.HOME";
        public static string X_OUT_ENABLE = "X.OUT.ENABLE";
        public static string X_OUT_MOVEREL = "X.OUT.MOVEREL";
        public static string X_OUT_MOVEABS = "X.OUT.MOVEABS";

        public static string X_OUT_STOP = "X.OUT.STOP";
        public static string X_OUT_ESTOP = "X.OUT.ESTOP";
        public static string X_IN_INPOS = "X.IN.INPOS";
        public static string X_OUT_VELOCITY = "X.OUT.VELOCITY";
        public static string X_IN_ACTVEL = "X.IN.ACTVEL";
        public static string X_IN_CMDVEL = "X.IN.CMDVEL";
        public static string X_IN_LIMITPLUS = "X.IN.LIMITPLUS";
        public static string X_IN_LIMITMINUS = "X.IN.LIMITMINUS";
        public static string X_IN_ERROR = "X.IN.ERROR";

        /// <summary>
        /// X축 JOG PLUS
        /// SET_DATA_TYPE : DOUBLE
        /// </summary>
        public static string X_OUT_JOGPLUS = "X.OUT.JOGPLUS";
        public static string X_OUT_JOGMINUS = "X.OUT.JOGMINUS";

        public static string Y_IN_ENABLE = "Y.IN.ENABLE";
        public static string Y_IN_CALIBRATED = "Y.IN.CALIBRATED";
        public static string Y_IN_ACTPOS = "Y.IN.ACTPOS";
        public static string Y_IN_BUSY = "Y.IN.BUSY";
        public static string Y_OUT_HOME = "Y.OUT.HOME";
        public static string Y_OUT_ENABLE = "Y.OUT.ENABLE";
        public static string Y_OUT_MOVEREL = "Y.OUT.MOVEREL";
        public static string Y_OUT_MOVEABS = "Y.OUT.MOVEABS";
        public static string Y_OUT_STOP = "Y.OUT.STOP";
        public static string Y_OUT_ESTOP = "Y.OUT.ESTOP";
        public static string Y_IN_INPOS = "Y.IN.INPOS";
        public static string Y_OUT_VELOCITY = "Y.OUT.VELOCITY";
        public static string Y_IN_ACTVEL = "Y.IN.ACTVEL";
        public static string Y_IN_CMDVEL = "Y.IN.CMDVEL";
        public static string Y_IN_LIMITPLUS = "Y.IN.LIMITPLUS";
        public static string Y_IN_LIMITMINUS = "Y.IN.LIMITMINUS";
        public static string Y_IN_ERROR = "Y.IN.ERROR";
        public static string Y_OUT_JOGPLUS = "Y.OUT.JOGPLUS";
        public static string Y_OUT_JOGMINUS = "Y.OUT.JOGMINUS";

        public static string Z1_IN_ENABLE = "Z1.IN.ENABLE";
        public static string Z1_IN_CALIBRATED = "Z1.IN.CALIBRATED";
        public static string Z1_IN_ACTPOS = "Z1.IN.ACTPOS";
        public static string Z1_IN_BUSY = "Z1.IN.BUSY";
        public static string Z1_OUT_HOME = "Z1.OUT.HOME";
        public static string Z1_OUT_ENABLE = "Z1.OUT.ENABLE";
        public static string Z1_OUT_MOVEREL = "Z1.OUT.MOVEREL";
        public static string Z1_OUT_MOVEABS = "Z1.OUT.MOVEABS";
        public static string Z1_OUT_STOP = "Z1.OUT.STOP";
        public static string Z1_OUT_ESTOP = "Z1.OUT.ESTOP";
        public static string Z1_IN_INPOS = "Z1.IN.INPOS";
        public static string Z1_OUT_VELOCITY = "Z1.OUT.VELOCITY";
        public static string Z1_IN_ACTVEL = "Z1.IN.ACTVEL";
        public static string Z1_IN_CMDVEL = "Z1.IN.CMDVEL";
        public static string Z1_IN_LIMITPLUS = "Z1.IN.LIMITPLUS";
        public static string Z1_IN_LIMITMINUS = "Z1.IN.LIMITMINUS";
        public static string Z1_IN_ERROR = "Z1.IN.ERROR";
        public static string Z1_OUT_JOGPLUS = "Z1.OUT.JOGPLUS";
        public static string Z1_OUT_JOGMINUS = "Z1.OUT.JOGMINUS";

        public static string Z2_IN_ENABLE = "Z2.IN.ENABLE";
        public static string Z2_IN_CALIBRATED = "Z2.IN.CALIBRATED";
        public static string Z2_IN_ACTPOS = "Z2.IN.ACTPOS";
        public static string Z2_IN_BUSY = "Z2.IN.BUSY";
        public static string Z2_OUT_HOME = "Z2.OUT.HOME";
        public static string Z2_OUT_ENABLE = "Z2.OUT.ENABLE";
        public static string Z2_OUT_MOVEREL = "Z2.OUT.MOVEREL";
        public static string Z2_OUT_MOVEABS = "Z2.OUT.MOVEABS";
        public static string Z2_OUT_STOP = "Z2.OUT.STOP";
        public static string Z2_OUT_ESTOP = "Z2.OUT.ESTOP";
        public static string Z2_IN_INPOS = "Z2.IN.INPOS";
        public static string Z2_OUT_VELOCITY = "Z2.OUT.VELOCITY";
        public static string Z2_IN_ACTVEL = "Z2.IN.ACTVEL";
        public static string Z2_IN_CMDVEL = "Z2.IN.CMDVEL";
        public static string Z2_IN_LIMITPLUS = "Z2.IN.LIMITPLUS";
        public static string Z2_IN_LIMITMINUS = "Z2.IN.LIMITMINUS";
        public static string Z2_IN_ERROR = "Z2.IN.ERROR";
        public static string Z2_OUT_JOGPLUS = "Z2.OUT.JOGPLUS";
        public static string Z2_OUT_JOGMINUS = "Z2.OUT.JOGMINUS";

        public static string Z3_IN_ENABLE = "Z3.IN.ENABLE";
        public static string Z3_IN_CALIBRATED = "Z3.IN.CALIBRATED";
        public static string Z3_IN_ACTPOS = "Z3.IN.ACTPOS";
        public static string Z3_IN_BUSY = "Z3.IN.BUSY";
        public static string Z3_OUT_HOME = "Z3.OUT.HOME";
        public static string Z3_OUT_ENABLE = "Z3.OUT.ENABLE";
        public static string Z3_OUT_MOVEREL = "Z3.OUT.MOVEREL";
        public static string Z3_OUT_MOVEABS = "Z3.OUT.MOVEABS";
        public static string Z3_OUT_STOP = "Z3.OUT.STOP";
        public static string Z3_OUT_ESTOP = "Z3.OUT.ESTOP";
        public static string Z3_IN_INPOS = "Z3.IN.INPOS";
        public static string Z3_OUT_VELOCITY = "Z3.OUT.VELOCITY";
        public static string Z3_IN_ACTVEL = "Z3.IN.ACTVEL";
        public static string Z3_IN_CMDVEL = "Z3.IN.CMDVEL";
        public static string Z3_IN_LIMITPLUS = "Z3.IN.LIMITPLUS";
        public static string Z3_IN_LIMITMINUS = "Z3.IN.LIMITMINUS";
        public static string Z3_IN_ERROR = "Z3.IN.ERROR";
        public static string Z3_OUT_JOGPLUS = "Z3.OUT.JOGPLUS";
        public static string Z3_OUT_JOGMINUS = "Z3.OUT.JOGMINUS";

        public static string T_IN_ENABLE = "T.IN.ENABLE";
        public static string T_IN_CALIBRATED = "T.IN.CALIBRATED";
        public static string T_IN_ACTPOS = "T.IN.ACTPOS";
        public static string T_IN_BUSY = "T.IN.BUSY";
        public static string T_OUT_HOME = "T.OUT.HOME";
        public static string T_OUT_ENABLE = "T.OUT.ENABLE";
        public static string T_OUT_MOVEREL = "T.OUT.MOVEREL";
        public static string T_OUT_MOVEABS = "T.OUT.MOVEABS";
        public static string T_OUT_STOP = "T.OUT.STOP";
        public static string T_OUT_ESTOP = "T.OUT.ESTOP";
        public static string T_IN_INPOS = "T.IN.INPOS";
        public static string T_OUT_VELOCITY = "T.OUT.VELOCITY";
        public static string T_IN_ACTVEL = "T.IN.ACTVEL";
        public static string T_IN_CMDVEL = "T.IN.CMDVEL";
        public static string T_IN_ERROR = "T.IN.ERROR";
        public static string T_OUT_JOGPLUS = "T.OUT.JOGPLUS";
        public static string T_OUT_JOGMINUS = "T.OUT.JOGMINUS";

        public static string X_CENTER_INDEX = "X.CENTER.INDEX";
        public static string Y_CENTER_INDEX = "Y.CENTER.INDEX";
        public static string X_CENTER_POSITION = "X.CENTER.POSITION";
        public static string Y_CENTER_POSITION = "Y.CENTER.POSITION";

        public static string X_LOADING_POSITION = "X.LOADING.POSITION";
        public static string Y_LOADING_POSITION = "Y.LOADING.POSITION";
        public static string X_LEFTEDGE_POSITION = "X.LEFTEDGE.POSITION";
        public static string Y_LEFTEDGE_POSITION = "Y.LEFTEDGE.POSITION";
        public static string X_RIGHTEDGE_POSITION = "X.RIGHTEDGE.POSITION";
        public static string Y_RIGHTEDGE_POSITION = "Y.RIGHTEDGE.POSITION";

        public static string MARK_X_OFFSET = "MARK.X.OFFSET";
        public static string MARK_Y_OFFSET = "MARK.Y.OFFSET";

        public static string SET_X_INDEX = "SET.X.INDEX";
        public static string SET_Y_INDEX = "SET.Y.INDEX";


        public static string SET_GRID_ROW = "SET.GRID.ROW";
        public static string SET_GRID_COL = "SET.GRID.COL";
        public static string SET_GRID_PITCH = "SET.GRID.PITCH";

        public static string SET_OK_COUNT = "SET.OK.COUNT";
        public static string SET_NG_COUNT = "SET.NG.COUNT";
        public static string SET_REPEAT_COUNT = "SET.REPEAT.COUNT";
        public static string SET_MOTION_TIMEOUT = "SET.MOTION.TIMEOUT";

        public static string X_LOADING_VELOCITY = "X.LOADING.VELOCITY";
        public static string Y_LOADING_VELOCITY = "Y.LOADING.VELOCITY";

        public static string X_MANUAL_VELOCITY = "X.MANUAL.VELOCITY";
        public static string Y_MANUAL_VELOCITY = "Y.MANUAL.VELOCITY";

        public static string CAMERA_RESOLUTION_X = "CAM.X.RESOLUTION";
        public static string CAMERA_RESOLUTION_Y = "CAM.Y.RESOLUTION";
        public static string X_DIRECTION_INVERT = "X.DIRECTION.INVERT";
        public static string Y_DIRECTION_INVERT = "Y.DIRECTION.INVERT";
        public static string Z1_DIRECTION_INVERT = "Z1.DIRECTION.INVERT";
        public static string Z2_DIRECTION_INVERT = "Z2.DIRECTION.INVERT";
        public static string Z3_DIRECTION_INVERT = "Z3.DIRECTION.INVERT";
        public static string T_DIRECTION_INVERT = "T.DIRECTION.INVERT";

        public static string WAFER_LOADING_COMPLETED = "WAFER.LOADING.COMPLETED";

        public static string SYS_MODE_SIMULATION = "SYS.MODE.SIMULATION";


        public static string GetField(string axis, string io, string fieldType)
        {
            axis = axis.ToUpper();

            if (string.IsNullOrWhiteSpace(axis))
            {
                throw new ArgumentException("Axis cannot be null or empty", nameof(axis));
            }

            if (string.IsNullOrWhiteSpace(io))
            {
                throw new ArgumentException("Result cannot be null or empty", nameof(io));
            }

            if (string.IsNullOrWhiteSpace(fieldType))
            {
                throw new ArgumentException("FieldType cannot be null or empty", nameof(fieldType));
            }

            if (io.Equals("IN", StringComparison.OrdinalIgnoreCase))
            {
                return $"{axis}.IN.{fieldType.ToUpper()}";
            }
            else if (io.Equals("OUT", StringComparison.OrdinalIgnoreCase))
            {
                return $"{axis}.OUT.{fieldType.ToUpper()}";
            }
            else
            {
                throw new ArgumentException($"Invalid result value: {io}. Must be 'IN' or 'OUT'.", nameof(io));
            }
        }
    }
}
