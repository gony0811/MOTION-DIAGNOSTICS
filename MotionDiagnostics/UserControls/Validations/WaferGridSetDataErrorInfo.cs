using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;


namespace MotionDiagnostics.UserControls.Validations
{
    [AddINotifyPropertyChangedInterface]
    public class WaferGridSetDataErrorInfo : IDataErrorInfo
    {
        private int pitch;
        private int row;
        private int col;

        public int Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }

        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Col
        {
            get { return col; }
            set { col = value; }
        }

        public string Error { get => ""; }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Pitch")
                {
                    if (pitch <= 0)
                    {
                        return "Wafer grid pitch must be greater than 0";
                    }
                }
                else if (columnName == "Row")
                {
                    if (row <= 0)
                    {
                        return "Wafer grid row must be greater than 0";
                    }
                }
                else if (columnName == "Col")
                {
                    if (col <= 0)
                    {
                        return "Wafer grid column must be greater than 0";
                    }
                }

                return "";
            }
        }
    }
}
