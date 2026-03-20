using System.ComponentModel;

namespace MDS.UI.UserControls.Validations
{
    internal class RepeatCountDataErrorInfo : IDataErrorInfo
    {
        private int repeatCount;

        public int RepeatCount
        {
            get { return repeatCount; }
            set { repeatCount = value; }
        }

        public string Error { get => ""; }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "RepeatCount")
                {
                    if (RepeatCount < 1)
                    {
                        return "Repeat Count must be greater than 0";
                    }
                }
                return "";
            }
        }
    }
}
