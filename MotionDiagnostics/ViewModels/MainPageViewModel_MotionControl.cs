using EPLE.Data;
using EPLE.Manager.Alarm;
using System;
using System.Windows;
using System.Windows.Threading;
using PropertyChanged;
using PrismCommands;
using EPLE.Manager;
using EPLE.Service;
using EPLE.ViewModel;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;


namespace MotionDiagnostics.ViewModels
{
    public partial class MainPageViewModel
    {

        
        public DelegateCommand ServoEnableCommand { get; private set; }
        public DelegateCommand ServoDisableCommand { get; private set; }
        public DelegateCommand ServoHomeCommand { get; private set; }
        public DelegateCommand ServoStopCommand { get; private set; }
        public DelegateCommand JogPlusCommand { get; private set; }
        public DelegateCommand JogMinusCommand { get; private set; }
        public DelegateCommand MoveAbsCommand { get; private set; }
        public DelegateCommand MoveRelCommand { get; private set; }

        public DelegateCommand UserInputVelocityCommand { get; private set; }
        public DelegateCommand VelocityCheckedCommand { get; private set; }
        public DelegateCommand ServoEStopCommand { get; private set; }
        public DelegateCommand MotionControl_LoadedCommand { get; private set; }
        public DelegateCommand MotionControl_UnloadedCommand { get; private set; }
        public DelegateCommand<SelectionChangedEventArgs> MotionAxisChangedCommand { get; private set; }

        public bool CanServoEnable { get; private set; } = true;
        public bool CanServoDisable { get; private set; } = true;
        public bool CanServoHome { get; private set; } = true;
        public bool CanServoStop { get; private set; } = true;
        public bool CanJogPlus { get; private set; } = true;
        public bool CanJogMinus { get; private set; } = true;
        public bool CanMoveAbs { get; private set; } = true;
        public bool CanMoveRel { get; private set; } = true;

        private DispatcherTimer statusMotionControlTimer = new DispatcherTimer();

        public void InitializeMotionAxisComboBox()
        {
            var devName = deviceVMList.Devices.FirstOrDefault((device) => device.DeviceType == "MOTION");

            MotionAxisList = new List<string>();

            MotionAxisList.Add("X");
            MotionAxisList.Add("Y");
            MotionAxisList.Add("T");
            MotionAxisList.Add("Z1");


            if (devName == null)
            {

            }
            else if (devName.DeviceName == "ACS")
            {
                //MotionAxisList.Add("Z2");
                //MotionAxisList.Add("Z3"); 
            }
            else if (devName.DeviceName == "PMAC")
            {
                MotionAxisList.Add("Z2");
                MotionAxisList.Add("Z3");
            }
            
        }

        private void InitializeMotionControl()
        {
            ServoEnableCommand = new DelegateCommand(ServoEnable).ObservesCanExecute(() => CanServoEnable);
            ServoDisableCommand = new DelegateCommand(ServoDisable).ObservesCanExecute(() => CanServoDisable);
            ServoHomeCommand = new DelegateCommand(ServoHome).ObservesCanExecute(() => CanServoHome);
            ServoStopCommand = new DelegateCommand(ServoStop).ObservesCanExecute(() => CanServoStop);
            JogPlusCommand = new DelegateCommand(JogPlus).ObservesCanExecute(() => CanJogPlus);
            JogMinusCommand = new DelegateCommand(JogMinus).ObservesCanExecute(() => CanJogMinus);
            MoveAbsCommand = new DelegateCommand(MoveAbs).ObservesCanExecute(() => CanMoveAbs);
            MoveRelCommand = new DelegateCommand(MoveRel).ObservesCanExecute(() => CanMoveRel);
            UserInputVelocityCommand = new DelegateCommand(UserInputVelocity);
            VelocityCheckedCommand = new DelegateCommand(VelocityChecked);
            ServoEStopCommand = new DelegateCommand(ServoEStop);
            MotionControl_LoadedCommand = new DelegateCommand(MotionControl_Loaded);
            MotionControl_UnloadedCommand = new DelegateCommand(MotionControl_Unloaded);
            MotionAxisChangedCommand = new DelegateCommand<SelectionChangedEventArgs>(MotionAxisChanged, CanMotionAxisChanged);
            // Initialize the motion control user control
            // ComboBox SelectedMotion 초기 설정

            InitializeMotionAxisComboBox();

            SelectedMotionAxis = "X";

            // Initialize the timer for monitoring the status of the motion device
            statusMotionControlTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = new System.TimeSpan(0, 0, 0, 0, 100)
            };
            statusMotionControlTimer.Tick += StatusMotionControlTimer_Tick; ;

            this.deviceManager.DeviceAttachEvent += FireDeviceAttachEvent;
        }

        private void FireDeviceAttachEvent(object sender, EventArgs e)
        {
            InitializeMotionAxisComboBox();
        }

        private void StatusMotionControlTimer_Tick(object sender, EventArgs e)
        {
            var result = false;

            if(SelectedMotionAxis == null)
            {
                return;
            }
            else if(!deviceManager.IsDeviceAttached())
            {
                IsPlusLimitChecked = false;
                IsMinusLimitChecked = false;
                IsInPositionChecked = false;
                IsCalibrationChecked = false;
                IsErrorChecked = false;
                ActualPosition = 0.0;
                MoveRelDistance = 0.0;
                return;
            }

            switch (SelectedMotionAxis)
            {
                case "X":
                    IsPlusLimitChecked = dataManager.GET_BOOL(DataNameHelper.X_IN_LIMITPLUS, out result);
                    IsMinusLimitChecked = dataManager.GET_BOOL(DataNameHelper.X_IN_LIMITMINUS, out result);
                    IsEnableChecked = dataManager.GET_BOOL(DataNameHelper.X_IN_ENABLE, out result);
                    IsDisableChecked = !IsEnableChecked;
                    IsInPositionChecked = dataManager.GET_BOOL(DataNameHelper.X_IN_INPOS, out result);
                    IsCalibrationChecked = dataManager.GET_BOOL(DataNameHelper.X_IN_CALIBRATED, out result);
                    IsBusyChecked = dataManager.GET_BOOL(DataNameHelper.X_IN_BUSY, out result);
                    IsErrorChecked = dataManager.GET_BOOL(DataNameHelper.X_IN_ERROR, out result);
                    ActualVelocity = dataManager.GET_DOUBLE(DataNameHelper.X_IN_ACTVEL, out result);
                    CommandVelocity = dataManager.GET_DOUBLE(DataNameHelper.X_IN_CMDVEL, out result);
                    ActualPosition = dataManager.GET_DOUBLE(DataNameHelper.X_IN_ACTPOS, out result);

                    break;
                case "Y":
                    IsPlusLimitChecked = dataManager.GET_BOOL(DataNameHelper.Y_IN_LIMITPLUS, out result);
                    IsMinusLimitChecked = dataManager.GET_BOOL(DataNameHelper.Y_IN_LIMITMINUS, out result);
                    IsEnableChecked = dataManager.GET_BOOL(DataNameHelper.Y_IN_ENABLE, out result);
                    IsDisableChecked = !IsEnableChecked;
                    IsInPositionChecked = dataManager.GET_BOOL(DataNameHelper.Y_IN_INPOS, out result);
                    IsCalibrationChecked = dataManager.GET_BOOL(DataNameHelper.Y_IN_CALIBRATED, out result);
                    IsErrorChecked = dataManager.GET_BOOL(DataNameHelper.Y_IN_ERROR, out result);
                    ActualVelocity = dataManager.GET_DOUBLE(DataNameHelper.Y_IN_ACTVEL, out result);
                    CommandVelocity = dataManager.GET_DOUBLE(DataNameHelper.Y_IN_CMDVEL, out result);
                    ActualPosition = dataManager.GET_DOUBLE(DataNameHelper.Y_IN_ACTPOS, out result);
                    break;
                case "Z1":
                    IsPlusLimitChecked = dataManager.GET_BOOL(DataNameHelper.Z1_IN_LIMITPLUS, out result);
                    IsMinusLimitChecked = dataManager.GET_BOOL(DataNameHelper.Z1_IN_LIMITMINUS, out result);
                    IsEnableChecked = dataManager.GET_BOOL(DataNameHelper.Z1_IN_ENABLE, out result);
                    IsDisableChecked = !IsEnableChecked;
                    IsInPositionChecked = dataManager.GET_BOOL(DataNameHelper.Z1_IN_INPOS, out result);
                    IsCalibrationChecked = dataManager.GET_BOOL(DataNameHelper.Z1_IN_CALIBRATED, out result);
                    IsErrorChecked = dataManager.GET_BOOL(DataNameHelper.Z1_IN_ERROR, out result);
                    ActualVelocity = dataManager.GET_DOUBLE(DataNameHelper.Z1_IN_ACTVEL, out result);
                    CommandVelocity = dataManager.GET_DOUBLE(DataNameHelper.Z1_IN_CMDVEL, out result);
                    ActualPosition = dataManager.GET_DOUBLE(DataNameHelper.Z1_IN_ACTPOS, out result);
                    break;
                case "Z2":
                    IsPlusLimitChecked = dataManager.GET_BOOL(DataNameHelper.Z2_IN_LIMITPLUS, out result);
                    IsMinusLimitChecked = dataManager.GET_BOOL(DataNameHelper.Z2_IN_LIMITMINUS, out result);
                    IsEnableChecked = dataManager.GET_BOOL(DataNameHelper.Z2_IN_ENABLE, out result);
                    IsDisableChecked = !IsEnableChecked;
                    IsInPositionChecked = dataManager.GET_BOOL(DataNameHelper.Z2_IN_INPOS, out result);
                    IsCalibrationChecked = dataManager.GET_BOOL(DataNameHelper.Z2_IN_CALIBRATED, out result);
                    IsErrorChecked = dataManager.GET_BOOL(DataNameHelper.Z2_IN_ERROR, out result);
                    ActualVelocity = dataManager.GET_DOUBLE(DataNameHelper.Z2_IN_ACTVEL, out result);
                    CommandVelocity = dataManager.GET_DOUBLE(DataNameHelper.Z2_IN_CMDVEL, out result);
                    ActualPosition = dataManager.GET_DOUBLE(DataNameHelper.Z2_IN_ACTPOS, out result);
                    break;
                case "Z3":
                    IsPlusLimitChecked = dataManager.GET_BOOL(DataNameHelper.Z3_IN_LIMITPLUS, out result);
                    IsMinusLimitChecked = dataManager.GET_BOOL(DataNameHelper.Z3_IN_LIMITMINUS, out result);
                    IsEnableChecked = dataManager.GET_BOOL(DataNameHelper.Z3_IN_ENABLE, out result);
                    IsDisableChecked = !IsEnableChecked;
                    IsInPositionChecked = dataManager.GET_BOOL(DataNameHelper.Z3_IN_INPOS, out result);
                    IsCalibrationChecked = dataManager.GET_BOOL(DataNameHelper.Z3_IN_CALIBRATED, out result);
                    IsErrorChecked = dataManager.GET_BOOL(DataNameHelper.Z3_IN_ERROR, out result);
                    ActualVelocity = dataManager.GET_DOUBLE(DataNameHelper.Z3_IN_ACTVEL, out result);
                    CommandVelocity = dataManager.GET_DOUBLE(DataNameHelper.Z3_IN_CMDVEL, out result);
                    ActualPosition = dataManager.GET_DOUBLE(DataNameHelper.Z3_IN_ACTPOS, out result);
                    break;
                case "T":
                    IsEnableChecked = dataManager.GET_BOOL(DataNameHelper.T_IN_ENABLE, out result);
                    IsDisableChecked = !IsEnableChecked;
                    IsInPositionChecked = dataManager.GET_BOOL(DataNameHelper.T_IN_INPOS, out result);
                    IsCalibrationChecked = dataManager.GET_BOOL(DataNameHelper.T_IN_CALIBRATED, out result);
                    IsErrorChecked = dataManager.GET_BOOL(DataNameHelper.T_IN_ERROR, out result);
                    ActualVelocity = dataManager.GET_DOUBLE(DataNameHelper.T_IN_ACTVEL, out result);
                    CommandVelocity = dataManager.GET_DOUBLE(DataNameHelper.T_IN_CMDVEL, out result);
                    ActualPosition = dataManager.GET_DOUBLE(DataNameHelper.T_IN_ACTPOS, out result);
                    break;
            }
        }

        public string SelectedMotionAxis { get; set; }

        public bool IsPlusLimitChecked { get; set; }

        public bool IsMinusLimitChecked { get; set; }

        public bool IsEnableChecked { get; set; }

        public bool IsDisableChecked { get; set; }

        public bool IsErrorChecked { get; set; }

        public bool IsHommingChecked { get; set; }
        public bool IsCalibrationChecked { get; set; }
        public bool IsInPositionChecked { get; set; }
        public bool IsBusyChecked { get; set; }
        public bool IsInputVelocityChecked { get; set; }

        public bool IsLowVelocityChecked { get; set; }

        public bool IsMidVelocityChecked { get; set; }

        public bool IsHighVelocityChecked { get; set; }

        public double InputVelocity { get; set; } = 0.0;
        public double LowVelocity { get; set; } = 10.0;
        public double MidVelocity { get; set; } = 30.0;
        public double HighVelocity { get; set; } = 50.0;
        public double MoveAbsPosition { get; set; } = 0.0;
        public double MoveRelDistance { get; set; } = 0.0;
        public double ActualPosition { get; set; } = 0.0;
        public double CommandPosition { get; set; } = 0.0;
        public string ServoErrorText { get; set; } = "";

        // 250121_mh.yun
        public double ActualVelocity { get; set; } = 0.0;
        public double CommandVelocity { get; set; } = 0.0;
        public List<string> MotionAxisList { get; set; }

        /*
        public bool CanServoEnable()
        {
            if (deviceManager.IsDeviceAttached())
            {
                return true;
            }
            else if (IsErrorChecked)
            {
                dataManager.ShowMessageBox("Error", "Servo is in error state.", ShowDialogOptions.Ok);
                //MessageBox.Show("Servo is in error state.");
                return false;
            }
            else if (IsEnableChecked)
            {
                dataManager.ShowMessageBox("Error", "Servo is already enabled.", ShowDialogOptions.Ok);
                //MessageBox.Show("Servo is already enabled.");
                return false;
            }
            else
            {
                dataManager.ShowMessageBox("Error", "Servo is disabled.", ShowDialogOptions.Ok);
                return false;
            }
        }
        */

        /*
        public bool CanExecuteServoDisable()
        {
            if (deviceManager.IsDeviceAttached() && IsEnableChecked)
            {
                return true;
            }
            else if (deviceManager.IsDeviceAttached()) 
            { 
                MessageBox.Show("Motion device is not attached.");
                return false; 
            }
            else if (!IsEnableChecked)
            {
                MessageBox.Show("Servo is already disabled.");
                return false;
            }
            else if (IsErrorChecked)
            {
                MessageBox.Show("Servo is in error state.");
                return false;
            }
            else
            {
                return false;
            }
        }
        */

        /*
        public bool CanExecuteServoHome()
        {
            if (deviceManager.IsDeviceAttached() && IsEnableChecked)
            {
                return true;
            }
            else if (!IsEnableChecked)
            {
                MessageBox.Show("Servo is not enabled.");
                return false;
            }
            else if (IsErrorChecked)
            {
                MessageBox.Show("Servo is in error state.");
                return false;
            }
            else if (IsHommingChecked)
            {
                MessageBox.Show("Servo is already homming.");
                return false;
            }
            else
            {
                return false;
            }
        }
        */

        /*
        public bool CanExecuteServoStop()
        {
            if (deviceManager.IsDeviceAttached())
                return true;
            else
                return false;
        }
        */

        /*
        public bool CanExecuteJogPlus()
        {
            if (deviceManager.IsDeviceAttached())
                return true;
            else
                return false;
        }
        */

        /*
        public bool CanExecuteJogMinus()
        {
            if (deviceManager.IsDeviceAttached())
                return true;
            else
                return false;
        }
        */

        /*
        public bool CanExecuteMoveAbs()
        {
            if (deviceManager.IsDeviceAttached())
                return true;
            else
                return false;
        }
        */

        /*
        public bool CanExecuteMoveRel()
        {
            if (deviceManager.IsDeviceAttached())
                return true;
            else
                return false;
        }
        */

        public bool CanMotionAxisChanged(SelectionChangedEventArgs args)
        {
            return true;
        }

        public void ServoEnable()
        {
            switch(SelectedMotionAxis)
            {
                case "X":
                    dataManager.SET_DATA(DataNameHelper.X_OUT_ENABLE, true);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_ENABLE, true);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_ENABLE, true);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_ENABLE, true);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_ENABLE, true);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_ENABLE, true);
                    break;
            }
        }

        public void ServoDisable()
        {
            switch (SelectedMotionAxis)
            {
                case "X":
                    dataManager.SET_DATA(DataNameHelper.X_OUT_ENABLE, false);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_ENABLE, false);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_ENABLE, false);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_ENABLE, false);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_ENABLE, false);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_ENABLE, false);
                    break;
            }
        }

        public void MotionAxisChanged(SelectionChangedEventArgs args)
        {

        }
        public void ServoHome()
        {
            switch (SelectedMotionAxis)
            {
                case "X":
                    dataManager.SET_DATA(DataNameHelper.X_OUT_HOME, true);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_HOME, true);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_HOME, true);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_HOME, true);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_HOME, true);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_HOME, true);
                    break;
            }
            //dataManager.SET_DATA(EPLE.Data.DataNameHelper.X_OUT_HOME, true);
        }

        public void ServoStop()
        {
            switch (SelectedMotionAxis)
            {
                case "X":
                    dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, true);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, true);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_STOP, true);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_STOP, true);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_STOP, true);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_STOP, true);
                    break;
            }
        }

        public void JogPlus()
        {
            VelocityChecked();

            switch (SelectedMotionAxis)
            {
                case "X":

                    dataManager.SET_DATA(DataNameHelper.X_OUT_JOGPLUS, true);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_JOGPLUS, true);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_JOGPLUS, true);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_JOGPLUS, true);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_JOGPLUS, true);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_JOGPLUS, true);
                    break;
            }
        }

        public void JogMinus()
        {
            VelocityChecked();

            switch (SelectedMotionAxis)
            {
                case "X":
                    dataManager.SET_DATA(DataNameHelper.X_OUT_JOGMINUS, true);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_JOGMINUS, true);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_JOGMINUS, true);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_JOGMINUS, true);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_JOGMINUS, true);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_JOGMINUS, true);
                    break;
            }
        }

        public async void MoveAbs()
        {
            var result = false;

            switch (SelectedMotionAxis)
            {
                case "X":
                    result = dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEABS, MoveAbsPosition);
                    break;
                case "Y":
                    result = dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEABS, MoveAbsPosition);
                    break;
                case "Z1":
                    result = dataManager.SET_DATA(DataNameHelper.Z1_OUT_MOVEABS, MoveAbsPosition);
                    break;
                case "Z2":
                    result = dataManager.SET_DATA(DataNameHelper.Z2_OUT_MOVEABS, MoveAbsPosition);
                    break;
                case "Z3":
                    result = dataManager.SET_DATA(DataNameHelper.Z3_OUT_MOVEABS, MoveAbsPosition);
                    break;
                case "T":
                    result = dataManager.SET_DATA(DataNameHelper.T_OUT_MOVEABS, MoveAbsPosition);
                    break;
            }

            await Task.Run(() =>
            {
                while (result)
                {

                    var isInPosition = false;

                    switch (SelectedMotionAxis)
                    {
                        case "X":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.X_IN_INPOS, out result);
                            break;
                        case "Y":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.Y_IN_INPOS, out result);
                            break;
                        case "Z1":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.Z1_IN_INPOS, out result);
                            break;
                        case "Z2":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.Z2_IN_INPOS, out result);
                            break;
                        case "Z3":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.X_IN_INPOS, out result);
                            break;
                        case "T":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.T_OUT_STOP, out result);
                            break;
                    }

                    if (isInPosition)
                    {
                        return;
                    }
                }

            });
        }


        public async void MoveRel()
        {
            var result = false;

            switch (SelectedMotionAxis)
            {
                case "X":
                    result = dataManager.SET_DATA(DataNameHelper.X_OUT_MOVEREL, MoveRelDistance);
                    break;
                case "Y":
                    result = dataManager.SET_DATA(DataNameHelper.Y_OUT_MOVEREL, MoveRelDistance);
                    break;
                case "Z1":
                    result = dataManager.SET_DATA(DataNameHelper.Z1_OUT_MOVEREL, MoveRelDistance);
                    break;
                case "Z2":
                    result = dataManager.SET_DATA(DataNameHelper.Z2_OUT_MOVEREL, MoveRelDistance);
                    break;
                case "Z3":
                    result = dataManager.SET_DATA(DataNameHelper.Z3_OUT_MOVEREL, MoveRelDistance);
                    break;
                case "T":
                    result = dataManager.SET_DATA(DataNameHelper.T_OUT_MOVEREL, MoveRelDistance);
                    break;
            }

            await Task.Run(() =>
            {
                while (result)
                {

                    var isInPosition = false;

                    switch (SelectedMotionAxis)
                    {
                        case "X":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.X_IN_INPOS, out result);
                            break;
                        case "Y":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.Y_IN_INPOS, out result);
                            break;
                        case "Z1":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.Z1_IN_INPOS, out result);
                            break;
                        case "Z2":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.Z2_IN_INPOS, out result);
                            break;
                        case "Z3":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.X_IN_INPOS, out result);
                            break;
                        case "T":
                            isInPosition = dataManager.GET_BOOL(DataNameHelper.T_OUT_STOP, out result);
                            break;
                    }

                    if (isInPosition)
                    {
                        return;
                    }
                }

            });

        }

        [DelegateCommand]
        public void UserInputVelocity()
        {
            switch (SelectedMotionAxis)
            {
                case "X":
                    dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, InputVelocity);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, InputVelocity);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_VELOCITY, InputVelocity);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_VELOCITY, InputVelocity);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_VELOCITY, InputVelocity);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_VELOCITY, InputVelocity);
                    break;
            }
        }

        [DelegateCommand]
        public void ServoEStop()
        {
            switch (SelectedMotionAxis)
            {
                case "X":
                    dataManager.SET_DATA(DataNameHelper.X_OUT_ESTOP, true);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_ESTOP, true);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_ESTOP, true);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_ESTOP, true);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_ESTOP, true);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_ESTOP, true);
                    break;
            }
        }
        [DelegateCommand]
        public void MotionControl_Loaded()
        {
            statusMotionControlTimer.Start();
        }

        [DelegateCommand]
        public void MotionControl_Unloaded()
        {
            statusMotionControlTimer.Stop();
        }

        [DelegateCommand]
        public void VelocityChecked()
        {
            if (IsInputVelocityChecked)
            {
                SetVelocity(InputVelocity);
            }
            else if (IsLowVelocityChecked)
            {
                SetVelocity(LowVelocity);
            }
            else if (IsMidVelocityChecked)
            {
                SetVelocity(MidVelocity);
            }
            else if (IsHighVelocityChecked)
            {
                SetVelocity(HighVelocity);
            }
        }

        private void SetVelocity(double velocity)
        {
            switch (SelectedMotionAxis)
            {
                case "X":
                    dataManager.SET_DATA(DataNameHelper.X_OUT_VELOCITY, velocity);
                    break;
                case "Y":
                    dataManager.SET_DATA(DataNameHelper.Y_OUT_VELOCITY, velocity);
                    break;
                case "Z1":
                    dataManager.SET_DATA(DataNameHelper.Z1_OUT_VELOCITY, velocity);
                    break;
                case "Z2":
                    dataManager.SET_DATA(DataNameHelper.Z2_OUT_VELOCITY, velocity);
                    break;
                case "Z3":
                    dataManager.SET_DATA(DataNameHelper.Z3_OUT_VELOCITY, velocity);
                    break;
                case "T":
                    dataManager.SET_DATA(DataNameHelper.T_OUT_VELOCITY, velocity);
                    break;
            }
        }
    }
}
