using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionDiagnostics.Model
{
    public class ModelManagement
    {
        public double OffsetX { get; set; }  // 측정 옵셋 값 X
        public double OffsetY { get; set; }  // 측정 옵셋 값 Y

        private static ModelManagement _instance;

        // Thread-safety를 위한 Lock 객체
        private static readonly object _lock = new object();

        private ModelManagement()
        {
            OffsetX = 0;
            OffsetY = 0;
        }
        public static ModelManagement GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock) // Thread-safe
                {
                    if (_instance == null)
                    {
                        _instance = new ModelManagement();
                    }
                }
            }
            return _instance;
        }

        public void Reset()
        {
            OffsetY = 0;
            OffsetX = 0;
        }

    }
}
