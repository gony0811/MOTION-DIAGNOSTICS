namespace MDS.UI.Model
{
    public class ModelManagement
    {
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }

        private static ModelManagement _instance;
        private static readonly object _lock = new object();

        private ModelManagement()
        {
            OffsetX = 0; OffsetY = 0;
        }

        public static ModelManagement GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
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
            OffsetY = 0; OffsetX = 0;
        }
    }
}
