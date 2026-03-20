
using EPLE.Data;
using EPLE.Manager;
using PropertyChanged;
using PrismCommands;
using Prism.Commands;

using System.Threading.Tasks;
using EPLE.Service;
using System;
using MotionDiagnostics.Properties;
using System.ComponentModel;
using MotionDiagnostics.UserControls.Validations;
using EPLE.Core.Service;

namespace MotionDiagnostics.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class WaferLoadingControlViewModel : IDataErrorInfo
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;
        private readonly MeasureManager measureManager;
        private readonly SequenceService sequenceService;
        //private readonly MotionModalVM motionModalVM;

        public WaferLoadingControlViewModel(DataManager dataManager, DeviceManager deviceManager, MeasureManager measureManager, SequenceService sequenceService)
        {
            this.dataManager = dataManager;
            this.deviceManager = deviceManager;
            this.measureManager = measureManager;
            this.sequenceService = sequenceService;

            LoadedCommand = new DelegateCommand(Loaded);
            UnloadedCommand = new DelegateCommand(Unloaded);
            LoadingPositionDataSetCommand = new DelegateCommand(LoadingPositionDataSet);
            MoveToLoadingPositionCommand = new DelegateCommand(MoveToLoadingPosition, CanExecuteMove);
            LeftEdgePositionDataSetCommand = new DelegateCommand(LeftEdgePositionDataSet);
            RightEdgePositionDataSetCommand = new DelegateCommand(RightEdgePositionDataSet);
            MoveToLeftEdgePositionCommand = new DelegateCommand(MoveToLeftEdgePosition, CanExecuteMove);
            MoveToRightEdgePositionCommand = new DelegateCommand(MoveToRightEdgePosition, CanExecuteMove);
            CenterPositionDataSetCommand = new DelegateCommand(CenterPositionDataSet);
            MoveToCenterPositionCommand = new DelegateCommand(MoveToCenterPosition, CanExecuteMove);

            WaferGridRowCount = dataManager.GET_INT(DataNameHelper.SET_GRID_ROW, out _);
            WaferGridColumnCount = dataManager.GET_INT(DataNameHelper.SET_GRID_COL, out _);
            WaferGridPitch = dataManager.GET_INT(DataNameHelper.SET_GRID_PITCH, out _);
            LoadingPositionX = dataManager.GET_DOUBLE(DataNameHelper.X_LOADING_POSITION, out _);
            LoadingPositionY = dataManager.GET_DOUBLE(DataNameHelper.Y_LOADING_POSITION, out _);
            LeftEdgePositionX = dataManager.GET_DOUBLE(DataNameHelper.X_LEFTEDGE_POSITION, out _);
            LeftEdgePositionY = dataManager.GET_DOUBLE(DataNameHelper.Y_LEFTEDGE_POSITION, out _);
            RightEdgePositionX = dataManager.GET_DOUBLE(DataNameHelper.X_RIGHTEDGE_POSITION, out _);
            RightEdgePositionY = dataManager.GET_DOUBLE(DataNameHelper.Y_RIGHTEDGE_POSITION, out _);
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "WaferGridRowCount")
                {
                    if (WaferGridRowCount <= 0)
                    {
                        return "Wafer grid row must be greater than 0";
                    }
                }
                else if (columnName == "WaferGridColumnCount")
                {
                    if (WaferGridColumnCount <= 0)
                    {
                        return "Wafer grid column must be greater than 0";
                    }
                }
                else if (columnName == "WaferGridPitch")
                {
                    if (WaferGridPitch <= 0)
                    {
                        return "Wafer grid pitch must be greater than 0";
                    }
                }

                return "";
            }
        }

        public double LoadingPositionX { get; set; }

        public double LoadingPositionY { get; set; }
        public double CenterPositionX { get; set; }
        public double CenterPositionY { get; set; }
        public double LeftEdgePositionX { get; set; }
        public double LeftEdgePositionY { get; set; }
        public double RightEdgePositionX { get; set; }
        public double RightEdgePositionY { get; set; }

        public int WaferGridRowCount { get; set; }
        public int WaferGridColumnCount { get; set; }

        public int WaferGridPitch { get; set; }

        public string ButtonContent_LoadingStatus { get; set; }

        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand UnloadedCommand { get; private set; }

        public DelegateCommand MoveToLoadingPositionCommand { get; private set; }
        public DelegateCommand LoadingPositionDataSetCommand { get; private set; }
        public DelegateCommand MoveToLeftEdgePositionCommand { get; private set; }
        public DelegateCommand LeftEdgePositionDataSetCommand { get; private set; }
        public DelegateCommand MoveToRightEdgePositionCommand { get; private set; }
        public DelegateCommand RightEdgePositionDataSetCommand { get; private set; }
        public DelegateCommand CenterPositionDataSetCommand { get; private set; }
        public DelegateCommand MoveToCenterPositionCommand { get; private set; }

        public string Error { get => ""; }


        [DelegateCommand]
        private void Loaded()
        {
            var waferLoadingCompleted = this.dataManager.GET_BOOL(DataNameHelper.WAFER_LOADING_COMPLETED, out _);

            if (waferLoadingCompleted)
                ButtonContent_LoadingStatus = "Loaded";
            else
                ButtonContent_LoadingStatus = "Unloaded";
        }

        [DelegateCommand]
        private void Unloaded()
        {

        }

        private bool CanExecuteMove()
        {
            if (deviceManager.IsDeviceAttached())
            {
                return true;
            }
            else
            {
                var title = Resources.ResourceManager.GetString("MB_TITLE_WARNING");
                var message = Resources.ResourceManager.GetString("MB_MSG_DEVICE_IS_NOT_RUNNING");
                System.Windows.MessageBox.Show(message, title);
                return false;
            }
        }


        [DelegateCommand]
        public async void MoveToLoadingPosition()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_PROCESSING");
            var message = Resources.ResourceManager.GetString("MB_MSG_MOVE_TO_LOADING_POSITION");

            var move_x_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.X_LOADING_VELOCITY, out _);
            var move_y_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.X_LOADING_VELOCITY, out _);

            await MoveToPosition(title, message, LoadingPositionX, LoadingPositionY, move_x_velocity, move_y_velocity);
        }

        public async void MoveToCenterPosition()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_PROCESSING");
            var message = Resources.ResourceManager.GetString("MB_MSG_MOVE_TO_CENTER_POSITION");

            var move_x_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var move_y_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, move_x_velocity);
            dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, move_y_velocity);

            await MoveToPosition(title, message, CenterPositionX, CenterPositionY, move_x_velocity, move_y_velocity);
        }



        [DelegateCommand]
        public void LoadingCompleted()
        {
            var waferLoadingCompleted = this.dataManager.GET_BOOL(DataNameHelper.WAFER_LOADING_COMPLETED, out _);

            var title = Resources.ResourceManager.GetString("MB_TITLE_LOADING_COMPLETED");
           
            if (waferLoadingCompleted)
            {
                var message = Resources.ResourceManager.GetString("MB_MSG_UNLOADING_COMPLETED");

                if (System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                {
                    this.dataManager.SET_DATA(DataNameHelper.WAFER_LOADING_COMPLETED, false);
                }
            }
            else
            {
                var message = Resources.ResourceManager.GetString("MB_MSG_LOADING_COMPLETED");

                if (System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                {
                    this.dataManager.SET_DATA(DataNameHelper.WAFER_LOADING_COMPLETED, true);
                }
            }
        }

        public void CenterPositionDataSet()
        {
            dataManager.SET_DATA(DataNameHelper.X_CENTER_POSITION, CenterPositionX, setDefaultValue: true);
            dataManager.SET_DATA(DataNameHelper.Y_CENTER_POSITION, CenterPositionY, setDefaultValue: true);
        }

        [DelegateCommand]
        public async void MoveToLeftEdgePosition()
        {

            var title = Resources.ResourceManager.GetString("MB_TITLE_PROCESSING");
            var message = Resources.ResourceManager.GetString("MB_MSG_MOVE_TO_LEFT_EDGE_POSITION");

            var move_x_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var move_y_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            await MoveToPosition(title, message, LeftEdgePositionX, LeftEdgePositionY, move_x_velocity, move_y_velocity);
        }

        [DelegateCommand]
        public async void MoveToRightEdgePosition()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_PROCESSING");
            var message = Resources.ResourceManager.GetString("MB_MSG_MOVE_TO_RIGHT_EDGE_POSITION");

            var move_x_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var move_y_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            await MoveToPosition(title, message, RightEdgePositionX, RightEdgePositionY, move_x_velocity, move_y_velocity);
        }

        [DelegateCommand]
        public void WaferGridDataSet()
        {
            dataManager.SET_DATA(DataNameHelper.SET_GRID_ROW, WaferGridRowCount, setDefaultValue: true);
            dataManager.SET_DATA(DataNameHelper.SET_GRID_COL, WaferGridColumnCount, setDefaultValue: true);
            dataManager.SET_DATA(DataNameHelper.SET_GRID_PITCH, WaferGridPitch, setDefaultValue: true);

            measureManager.CreateMeasureData(WaferGridRowCount, WaferGridColumnCount);
        }

        [DelegateCommand]
        public void LoadingPositionDataSet()
        {
            dataManager.SET_DATA(DataNameHelper.X_LOADING_POSITION, LoadingPositionX, setDefaultValue: true);
            dataManager.SET_DATA(DataNameHelper.Y_LOADING_POSITION, LoadingPositionY, setDefaultValue: true);
        }

        [DelegateCommand]
        public void LeftEdgePositionDataSet()
        {
            dataManager.SET_DATA(DataNameHelper.X_LEFTEDGE_POSITION, LeftEdgePositionX, setDefaultValue: true);
            dataManager.SET_DATA(DataNameHelper.Y_LEFTEDGE_POSITION, LeftEdgePositionY, setDefaultValue: true);
        }

        [DelegateCommand]
        public void RightEdgePositionDataSet()
        {
            dataManager.SET_DATA(DataNameHelper.X_RIGHTEDGE_POSITION, RightEdgePositionX, setDefaultValue: true);
            dataManager.SET_DATA(DataNameHelper.Y_RIGHTEDGE_POSITION, RightEdgePositionY, setDefaultValue: true);
        }

        private async Task MoveToPosition(string mb_title, string mb_message, double position_x, double position_y, double velocity_x, double velocity_y)
        {
            dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, velocity_x);
            dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, velocity_y);

            dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEABS, position_x);
            dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEABS, position_y);

            DateTime st = DateTime.Now;

            await Task.Run(async () =>
            {
                while (true)
                {
                    TimeSpan elipsed = DateTime.Now - st;

                    await Task.Delay(100);

                    var xBusy = dataManager.GET_INT(DataNameHelper.X_IN_BUSY, out _);
                    var yBusy = dataManager.GET_INT(DataNameHelper.Y_IN_BUSY, out _);

                    if (xBusy == 0 && yBusy == 0)
                    {
                        break;
                    }
                    else if (elipsed.TotalSeconds >= 60)
                    {
                        break;
                    }
                }

            });
        }

    }
}
