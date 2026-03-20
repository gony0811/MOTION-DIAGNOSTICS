
using EPLE.Data;
using EPLE.Manager;
using PropertyChanged;

namespace MotionDiagnostics.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class MotionModalVM
    {
        private DataManager dataManager;
        private DeviceManager deviceManager;

        public string Name { get; set; }

        public double CommandPosition { get; set; }

        public double FeedbackPosition { get; set; }

        public double Velocity { get; set; }

        public MotionModalVM(DataManager dataManager, DeviceManager deviceManager)
        {
            this.dataManager = dataManager;
            this.deviceManager = deviceManager;
        }

        public void SetData(string name)
        {
            bool result = false;
            switch (name)
            {
                case "LoadingPositionX":
                    CommandPosition = dataManager.GET_DOUBLE(DataNameHelper.X_LOADING_POSITION, out result);
                    Velocity = dataManager.GET_DOUBLE(DataNameHelper.X_LOADING_VELOCITY, out result);
                    break;
            }
        }
    }
}
