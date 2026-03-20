using EPLE.Manager;
using PropertyChanged;
using PrismCommands;
using Prism.Commands;

using MotionDiagnostics.Properties;
using EPLE.Data;
using System.Threading.Tasks;
using System;
using EPLE.ImageProcessing;
using EPLE.ViewModel;
using EPLE.Core.Utility;
using EPLE.Service;
using System.Threading;

namespace MotionDiagnostics.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class WaferAlignControlViewModel
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;
        private readonly HalconImageProcessing imageProcessing;
        private readonly WaferAlignService waferAlignService;

        private const double inPositionValue = 10.0;

        public WaferAlignControlViewModel(DataManager dataManager, DeviceManager deviceManager, HalconImageProcessing imageProcessing, WaferAlignService waferAlignService)
        {
            this.dataManager = dataManager;
            this.deviceManager = deviceManager;
            this.imageProcessing = imageProcessing;
            this.waferAlignService = waferAlignService;

            MoveLeftEdgeButtonClickCommand = new DelegateCommand(MoveLeftEdgeButtonClick, CanExecuteMove);
            MoveRightEdgeButtonClickCommand = new DelegateCommand(MoveRightEdgeButtonClick, CanExecuteMove);
            MoveCenterButtonClickCommand = new DelegateCommand(MoveCenterButtonClick, CanExecuteMove);
            CalcAngleOffsetButtonClickCommand = new DelegateCommand(CalcAngleOffsetButtonClick);
            ApplyAngleOffsetButtonClickCommand = new DelegateCommand(ApplyAngleOffsetButtonClick);
            RotationThetaAngleButtonClickCommand = new DelegateCommand(RotationThetaAngleButtonClick, CanExecuteMove);
            MoveLeftEdgeMarkButtonClickCommand = new DelegateCommand(MoveLeftEdgeMarkButtonClick, CanExecuteMove);
            MoveCenterMarkButtonClickCommand = new DelegateCommand(MoveCenterMarkButtonClick, CanExecuteMove);
            MoveRightEdgeMarkButtonClickCommand = new DelegateCommand(MoveRightEdgeMarkButtonClick, CanExecuteMove);
            SetLeftEdgeMarkButtonClickCommand = new DelegateCommand(SetLeftEdgeMarkButtonClick);
            SetCenterMarkButtonClickCommand = new DelegateCommand(SetCenterMarkButtonClick);
            SetRightEdgeMarkButtonClickCommand = new DelegateCommand(SetRightEdgeMarkButtonClick);
            FindLeftEdgeButtonClickCommand = new DelegateCommand(FindLeftEdgeButtonClick);
            FindRightEdgeButtonClickCommand = new DelegateCommand(FindRightEdgeButtonClick);
            FindCenterButtonClickCommand = new DelegateCommand(FindCenterButtonClick);

        }

        public double ThetaAlignAngleOffset { get; set; }
        public double RotationAngle { get; set; }

        public double ThetaAlignAngle { get; set; }

        public double MarkCenterX { get; set; }
        public double MarkCenterY { get; set; }
        public double MarkLeftEdgeCenterX { get; set; }
        public double MarkLeftEdgeCenterY { get; set; } 
        public double MarkRightEdgeCenterX { get; set; }
        public double MarkRightEdgeCenterY { get; set; }
        public double MarkLeftEdgeX { get; set; }
        public double MarkLeftEdgeY { get; set; }
        public double MarkRightEdgeX { get; set; }
        public double MarkRightEdgeY { get; set; }

        public DelegateCommand MoveLeftEdgeButtonClickCommand { get; private set; }
        public DelegateCommand MoveRightEdgeButtonClickCommand { get; private set; }
        public DelegateCommand MoveCenterButtonClickCommand { get; private set; }
        public DelegateCommand CalcAngleOffsetButtonClickCommand { get; private set; }
        public DelegateCommand ApplyAngleOffsetButtonClickCommand { get; private set; }
        public DelegateCommand RotationThetaAngleButtonClickCommand { get; private set; }
        public DelegateCommand MoveLeftEdgeMarkButtonClickCommand { get; private set; }
        public DelegateCommand MoveCenterMarkButtonClickCommand { get; private set; }
        public DelegateCommand MoveRightEdgeMarkButtonClickCommand { get; private set; }
        public DelegateCommand SetLeftEdgeMarkButtonClickCommand { get; private set; }
        public DelegateCommand SetCenterMarkButtonClickCommand { get; private set; }
        public DelegateCommand SetRightEdgeMarkButtonClickCommand { get; private set; }
        public DelegateCommand FindLeftEdgeButtonClickCommand { get; private set; }
        public DelegateCommand FindRightEdgeButtonClickCommand { get; private set; }
        public DelegateCommand FindCenterButtonClickCommand { get; private set; }

        public bool CanExecuteMove()
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
        public void Loaded()
        {
            MarkLeftEdgeX = this.dataManager.GET_DOUBLE(DataNameHelper.X_LEFTEDGE_POSITION, out _);
            MarkLeftEdgeY = this.dataManager.GET_DOUBLE(DataNameHelper.Y_LEFTEDGE_POSITION, out _);
            MarkCenterX = this.dataManager.GET_DOUBLE(DataNameHelper.X_CENTER_POSITION, out _);
            MarkCenterY = this.dataManager.GET_DOUBLE(DataNameHelper.Y_CENTER_POSITION, out _);
            MarkRightEdgeX = this.dataManager.GET_DOUBLE(DataNameHelper.X_RIGHTEDGE_POSITION, out _);
            MarkRightEdgeY = this.dataManager.GET_DOUBLE(DataNameHelper.Y_RIGHTEDGE_POSITION, out _);

            this.dataManager.DataChangedEvent += FireDataChangedEvent;
        }

        private void FireDataChangedEvent(object sender, DataVMList.DataVM e)
        {
            if (e.Name == DataNameHelper.X_LEFTEDGE_POSITION)
            {
                MarkLeftEdgeX = (double)e.Value;
            }
            else if (e.Name == DataNameHelper.Y_LEFTEDGE_POSITION)
            {
                MarkLeftEdgeY = (double)e.Value;
            }
            else if (e.Name == DataNameHelper.X_CENTER_POSITION)
            {
                MarkCenterX = (double)e.Value;
            }
            else if (e.Name == DataNameHelper.Y_CENTER_POSITION)
            {
                MarkCenterY = (double)e.Value;
            }
            else if (e.Name == DataNameHelper.X_RIGHTEDGE_POSITION)
            {
                MarkRightEdgeX = (double)e.Value;
            }
            else if (e.Name == DataNameHelper.Y_RIGHTEDGE_POSITION)
            {
                MarkRightEdgeY = (double)e.Value;
            }
        }

        [DelegateCommand]
        public void Unloaded()
        {
            this.dataManager.DataChangedEvent -= FireDataChangedEvent;
        }

        [DelegateCommand]
        public async void MoveLeftEdgeButtonClick()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_PROCESSING");
            var message = Resources.ResourceManager.GetString("MB_MSG_MOVE_TO_LEFT_EDGE_POSITION");


            var move_x_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var move_y_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            var move_x_position = this.dataManager.GET_DOUBLE(DataNameHelper.X_LEFTEDGE_POSITION, out _);
            var move_y_position = this.dataManager.GET_DOUBLE(DataNameHelper.Y_LEFTEDGE_POSITION, out _);

            await MoveToPosition(title, message, move_x_position, move_y_position, move_x_velocity, move_y_velocity);    
        }

        [DelegateCommand]
        public async void MoveRightEdgeButtonClick()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_PROCESSING");
            var message = Resources.ResourceManager.GetString("MB_MSG_MOVE_TO_RIGHT_EDGE_POSITION");

            var move_x_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var move_y_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            await MoveToPosition(title, message, MarkRightEdgeX, MarkRightEdgeY, move_x_velocity, move_y_velocity);
        }

        [DelegateCommand]
        public async void MoveCenterButtonClick()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_PROCESSING");
            var message = Resources.ResourceManager.GetString("MB_MSG_MOVE_TO_CENTER_POSITION");

            var move_x_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var move_y_velocity = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            var move_x_position = this.dataManager.GET_DOUBLE(DataNameHelper.X_CENTER_POSITION, out _);
            var move_y_position = this.dataManager.GET_DOUBLE(DataNameHelper.Y_CENTER_POSITION, out _);

            await MoveToPosition(title, message, move_x_position, move_y_position, move_x_velocity, move_y_velocity);
        }

        [DelegateCommand]
        public void CalcAngleOffsetButtonClick()
        {
            var left_x = this.dataManager.GET_DOUBLE(DataNameHelper.X_LEFTEDGE_POSITION, out _);
            var left_y = this.dataManager.GET_DOUBLE(DataNameHelper.Y_LEFTEDGE_POSITION, out _);
            var right_x = this.dataManager.GET_DOUBLE(DataNameHelper.X_RIGHTEDGE_POSITION, out _);
            var right_y = this.dataManager.GET_DOUBLE(DataNameHelper.Y_RIGHTEDGE_POSITION, out _);

            var direction_invert_x = this.dataManager.GET_DOUBLE(DataNameHelper.X_DIRECTION_INVERT, out _);
            var direction_invert_y = this.dataManager.GET_DOUBLE(DataNameHelper.Y_DIRECTION_INVERT, out _);

            var y = Math.Abs(left_y - right_y) * direction_invert_y;
            var x = (left_x - right_x) * direction_invert_x;
            var radian = Math.Atan2(y, x);

            var degree = MathLib.RadToDeg(radian);

            ThetaAlignAngle = degree;
        }

        [DelegateCommand]
        private async void ApplyAngleOffsetButtonClick()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_WARNING");
            var message = Resources.ResourceManager.GetString("MB_MSG_NEED_TO_GRAB_IMAGE");

            if (!this.imageProcessing.IsGrabStart)
            {
                System.Windows.MessageBox.Show(message, title);
                return;
            }

            title = Resources.ResourceManager.GetString("MB_TITLE_CONFIRM");
            message = Resources.ResourceManager.GetString("MB_MSG_CONFIRM_START_ALIGN");

            if (System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo) != System.Windows.MessageBoxResult.Yes) return;

            CancellationTokenSource cancelationTokenSource = new CancellationTokenSource();

            try
            {
                await this.waferAlignService.StartAsync(cancelationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation exception if needed
            }
        }

        [DelegateCommand]
        public void RotationThetaAngleButtonClick()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_CONFIRM");
            var message = Resources.ResourceManager.GetString("MB_MSG_ROTATION_ANGLE");

            if (System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo) != System.Windows.MessageBoxResult.Yes) return;

            this.dataManager.SET_DATA(DataNameHelper.T_OUT_VELOCITY, 1);
            this.dataManager.SET_DATA(DataNameHelper.T_OUT_MOVEREL, RotationAngle);
        }

        [DelegateCommand]
        public void MoveLeftEdgeMarkButtonClick()
        {
            var xActPos = this.dataManager.GET_DOUBLE(DataNameHelper.X_IN_ACTPOS, out _);
            var yActPos = this.dataManager.GET_DOUBLE(DataNameHelper.Y_IN_ACTPOS, out _);

            var xTargetPos = MarkLeftEdgeX;
            var yTargetPos = MarkLeftEdgeY;

            if (Math.Abs(xActPos - xTargetPos) > inPositionValue || Math.Abs(yActPos - yTargetPos) > inPositionValue)
            {
                var title = Resources.ResourceManager.GetString("MB_TITLE_WARNING");
                var message = Resources.ResourceManager.GetString("MB_MSG_POS_IS_NOT_LEFTEDGE");

                System.Windows.MessageBox.Show(message, title);
                return;
            }

            var mark_x_offset = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_X_OFFSET, out _);
            var mark_y_offset = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_Y_OFFSET, out _);

            var velocity_x = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var velocity_y = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            var x_direction_invert = this.dataManager.GET_INT(DataNameHelper.X_DIRECTION_INVERT, out _);
            var y_direction_invert = this.dataManager.GET_INT(DataNameHelper.Y_DIRECTION_INVERT, out _);

            this.dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, velocity_x);
            this.dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, velocity_y);

            this.dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEREL, mark_x_offset * x_direction_invert);
            this.dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEREL, mark_y_offset * y_direction_invert);
        }

        [DelegateCommand]
        public void MoveCenterMarkButtonClick()
        {
            var xActPos = this.dataManager.GET_DOUBLE(DataNameHelper.X_IN_ACTPOS, out _);
            var yActPos = this.dataManager.GET_DOUBLE(DataNameHelper.Y_IN_ACTPOS, out _);

            var xTargetPos = MarkCenterX;
            var yTargetPos = MarkCenterY;

            if (Math.Abs(xActPos - xTargetPos) > inPositionValue || Math.Abs(yActPos - yTargetPos) > inPositionValue)
            {
                var title = Resources.ResourceManager.GetString("MB_TITLE_WARNING");
                var message = Resources.ResourceManager.GetString("MB_MSG_POS_IS_NOT_CENTER");

                System.Windows.MessageBox.Show(message, title);
                return;
            }

            var mark_x_offset = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_X_OFFSET, out _);
            var mark_y_offset = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_Y_OFFSET, out _);

            var velocity_x = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var velocity_y = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            this.dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, velocity_x);
            this.dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, velocity_y);

            var x_direction_invert = this.dataManager.GET_INT(DataNameHelper.X_DIRECTION_INVERT, out _);
            var y_direction_invert = this.dataManager.GET_INT(DataNameHelper.Y_DIRECTION_INVERT, out _);

            this.dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEREL, mark_x_offset * x_direction_invert);
            this.dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEREL, mark_y_offset * y_direction_invert);
        }

        [DelegateCommand]
        public void MoveRightEdgeMarkButtonClick()
        {
            var xActPos = this.dataManager.GET_DOUBLE(DataNameHelper.X_IN_ACTPOS, out _);
            var yActPos = this.dataManager.GET_DOUBLE(DataNameHelper.Y_IN_ACTPOS, out _);

            var xTargetPos = MarkRightEdgeX;
            var yTargetPos = MarkRightEdgeY;

            if (Math.Abs(xActPos - xTargetPos) > inPositionValue || Math.Abs(yActPos - yTargetPos) > inPositionValue)
            {
                var title = Resources.ResourceManager.GetString("MB_TITLE_WARNING");
                var message = Resources.ResourceManager.GetString("MB_MSG_POS_IS_NOT_RIGHTEDGE");

                System.Windows.MessageBox.Show(message, title);
                return;
            }

            var mark_x_offset = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_X_OFFSET, out _);
            var mark_y_offset = this.dataManager.GET_DOUBLE(DataNameHelper.MARK_Y_OFFSET, out _);

            var velocity_x = this.dataManager.GET_DOUBLE(DataNameHelper.X_MANUAL_VELOCITY, out _);
            var velocity_y = this.dataManager.GET_DOUBLE(DataNameHelper.Y_MANUAL_VELOCITY, out _);

            this.dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, velocity_x);
            this.dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, velocity_y);

            var x_direction_invert = this.dataManager.GET_INT(DataNameHelper.X_DIRECTION_INVERT, out _);
            var y_direction_invert = this.dataManager.GET_INT(DataNameHelper.Y_DIRECTION_INVERT, out _);

            this.dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEREL, mark_x_offset * x_direction_invert);
            this.dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEREL, mark_y_offset * y_direction_invert);
        }

        [DelegateCommand]
        public void SetLeftEdgeMarkButtonClick()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_CONFIRM");
            var message = Resources.ResourceManager.GetString("MB_MSG_SAVE_LEFTEDGE_MARK");

            if (System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo) != System.Windows.MessageBoxResult.Yes) return;

            var xActPos = this.dataManager.GET_DOUBLE(DataNameHelper.X_IN_ACTPOS, out _);
            var yActPos = this.dataManager.GET_DOUBLE(DataNameHelper.Y_IN_ACTPOS, out _);

            this.dataManager.SET_DATA(DataNameHelper.X_LEFTEDGE_POSITION, xActPos);
            this.dataManager.SET_DATA(DataNameHelper.Y_LEFTEDGE_POSITION, yActPos);

            MarkLeftEdgeX = xActPos;
            MarkLeftEdgeY = yActPos;
        }

        [DelegateCommand]
        public void SetCenterMarkButtonClick()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_CONFIRM");
            var message = Resources.ResourceManager.GetString("MB_MSG_SAVE_CENTER_MARK");

            if (System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo) != System.Windows.MessageBoxResult.Yes) return;

            var xActPos = this.dataManager.GET_DOUBLE(DataNameHelper.X_IN_ACTPOS, out _);
            var yActPos = this.dataManager.GET_DOUBLE(DataNameHelper.Y_IN_ACTPOS, out _);

            this.dataManager.SET_DATA(DataNameHelper.X_CENTER_POSITION, xActPos);
            this.dataManager.SET_DATA(DataNameHelper.Y_CENTER_POSITION, yActPos);

            MarkCenterX = xActPos;
            MarkCenterY = yActPos;
        }
        [DelegateCommand]
        public void SetRightEdgeMarkButtonClick()
        {
            var title = Resources.ResourceManager.GetString("MB_TITLE_CONFIRM");
            var message = Resources.ResourceManager.GetString("MB_MSG_SAVE_RIGHTEDGE_MARK");

            if (System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.YesNo) != System.Windows.MessageBoxResult.Yes) return;

            var xActPos = this.dataManager.GET_DOUBLE(DataNameHelper.X_IN_ACTPOS, out _);
            var yActPos = this.dataManager.GET_DOUBLE(DataNameHelper.Y_IN_ACTPOS, out _);

            this.dataManager.SET_DATA(DataNameHelper.X_RIGHTEDGE_POSITION, xActPos);
            this.dataManager.SET_DATA(DataNameHelper.Y_RIGHTEDGE_POSITION, yActPos);

            MarkRightEdgeX = xActPos;
            MarkRightEdgeY = yActPos;
        }
        [DelegateCommand]
        public void FindLeftEdgeButtonClick()
        {
            (double x, double y) = imageProcessing.Find(out bool result);

            this.dataManager.SET_DATA(DataNameHelper.MARK_X_OFFSET, x);
            this.dataManager.SET_DATA(DataNameHelper.MARK_Y_OFFSET, y);
        }
        [DelegateCommand]
        public void FindRightEdgeButtonClick()
        {
            (double x, double y) = imageProcessing.Find(out bool result);

            this.dataManager.SET_DATA(DataNameHelper.MARK_X_OFFSET, x);
            this.dataManager.SET_DATA(DataNameHelper.MARK_Y_OFFSET, y);
        }
        [DelegateCommand]
        public void FindCenterButtonClick()
        {
            (double x, double y) = imageProcessing.Find(out bool result);

            this.dataManager.SET_DATA(DataNameHelper.MARK_X_OFFSET, x);
            this.dataManager.SET_DATA(DataNameHelper.MARK_Y_OFFSET, y);
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
