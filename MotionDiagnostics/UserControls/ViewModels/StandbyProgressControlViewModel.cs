using EPLE.Data;
using EPLE.Manager;
using EPLE.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.Windows.Threading;
using MediaBrush = System.Windows.Media.Brush;
using MediaBrushes = System.Windows.Media.Brushes;
using PropertyChanged;
using PrismCommands;
using Prism.Commands;
using EPLE.ImageProcessing;
using System.Windows;
using MotionDiagnostics.Properties;
using EPLE.Service;

namespace MotionDiagnostics.UserControls.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class StandbyProgressControlViewModel
    {
        public DelegateCommand LoadedCommand { get; private set; }
        public DelegateCommand UnloadedCommand { get; private set; }

        public enum StandbyProgress
        {
            Init,
            DeviceAttach,
            MotionInit,
            WaferLoading,
            VisionRegist,
            WaferAlign,
            MeasureStandby
        }

        public EventHandler<StandbyProgress> StandbyProgressEventHandler;

        private StandbyProgress standbyProgress = StandbyProgress.Init;
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;
        private readonly HalconImageProcessing imageProcessing;
        private readonly WaferAlignService waferAlignService;
        private DispatcherTimer standbyTimer;

        public StandbyProgressControlViewModel(DataManager dataManager, DeviceManager deviceManager, HalconImageProcessing imageProcessing, WaferAlignService waferAlignService)
        {
            LoadedCommand = new DelegateCommand(Loaded);
            UnloadedCommand = new DelegateCommand(Unloaded);

            this.dataManager = dataManager;
            this.deviceManager = deviceManager;
            this.imageProcessing = imageProcessing;
            this.waferAlignService = waferAlignService;
            standbyTimer = new DispatcherTimer(new System.TimeSpan(0, 0, 0, 0, 200), DispatcherPriority.Background, StandbyTimer_Tick, App.Current.Dispatcher);
        }

        public string CurrentStepStatus { get; set; } = "Init";

        public MediaBrush DeviceAttachStatusBackground { get; set; } = MediaBrushes.LightGray;

        public MediaBrush MotionInitStatusBackground { get; set; } = MediaBrushes.LightGray;

        public MediaBrush VisionStatusBackground { get; set; } = MediaBrushes.LightGray;
        public MediaBrush WaferLoadingStatusBackground { get; set; } = MediaBrushes.LightGray;
        public MediaBrush WaferAlignStatusBackground { get; set; } = MediaBrushes.LightGray;

        public MediaBrush MeasureStandbyStatusBackground { get; set; } = MediaBrushes.LightGray;

        public string StatusText { get; set; } = "Init";

        public void Loaded()
        {
            this.DeviceAttachStatusBackground = MediaBrushes.LightGray;
            this.MotionInitStatusBackground = MediaBrushes.LightGray;
            this.VisionStatusBackground = MediaBrushes.LightGray;
            this.WaferLoadingStatusBackground = MediaBrushes.LightGray;
            this.WaferAlignStatusBackground = MediaBrushes.LightGray;
            this.MeasureStandbyStatusBackground = MediaBrushes.LightGray;
            dataManager.DataChangedEvent += DataManager_DataChangedEvent;
            standbyTimer.Start();
        }

        public void Unloaded()
        {
            dataManager.DataChangedEvent -= DataManager_DataChangedEvent;
            standbyTimer.Stop();
        }



        private void StandbyTimer_Tick(object sender, EventArgs e)
        {
            switch (standbyProgress)
            {
                case StandbyProgress.Init:
                    //this.DeviceAttachStatusBackground = MediaBrushes.LightGray;
                    //this.MotionInitStatusBackground = MediaBrushes.LightGray;
                    //this.VisionStatusBackground = MediaBrushes.LightGray;
                    //this.WaferLoadingStatusBackground = MediaBrushes.LightGray;
                    //this.WaferAlignStatusBackground = MediaBrushes.LightGray;
                    //this.MeasureStandbyStatusBackground = MediaBrushes.LightGray;
                    //CurrentStepStatus = "Init";
                    standbyProgress = StandbyProgress.DeviceAttach;
                    break;
                case StandbyProgress.DeviceAttach:
                    if (deviceManager.IsDeviceAttached())
                    {
                        standbyProgress = StandbyProgress.MotionInit;
                        DeviceAttachStatusBackground = MediaBrushes.LightGreen;
                    }
                    else
                    {
                        this.DeviceAttachStatusBackground = MediaBrushes.LightGray;
                        this.MotionInitStatusBackground = MediaBrushes.LightGray;
                        this.VisionStatusBackground = MediaBrushes.LightGray;
                        this.WaferLoadingStatusBackground = MediaBrushes.LightGray;
                        this.WaferAlignStatusBackground = MediaBrushes.LightGray;
                        this.MeasureStandbyStatusBackground = MediaBrushes.LightGray;

                        CurrentStepStatus = "Device Attach";
                        StatusText = Resources.ResourceManager.GetString("MSG_STANDBY_DEVICE_ATTACH");
                        standbyProgress = StandbyProgress.Init;
                    }
                    break;
                case StandbyProgress.MotionInit:
                    var result = false;
                    var xEnableValue = dataManager.GET_BOOL(DataNameHelper.X_IN_ENABLE, out result);
                    var yEnableValue = dataManager.GET_BOOL(DataNameHelper.Y_IN_ENABLE, out result);
                    var tEnableValue = dataManager.GET_BOOL(DataNameHelper.T_IN_ENABLE, out result);
                    var z1EnableValue = dataManager.GET_BOOL(DataNameHelper.Z1_IN_ENABLE, out result);
                    var z2EnableValue = dataManager.GET_BOOL(DataNameHelper.Z2_IN_ENABLE, out result);
                    var z3EnableValue = dataManager.GET_BOOL(DataNameHelper.Z3_IN_ENABLE, out result);

                    var xCalibrated = dataManager.GET_BOOL(DataNameHelper.X_IN_CALIBRATED, out result);
                    var yCalibrated = dataManager.GET_BOOL(DataNameHelper.Y_IN_CALIBRATED, out result);
                    var tCalibrated = dataManager.GET_BOOL(DataNameHelper.T_IN_CALIBRATED, out result);
                    var z1Calibrated = dataManager.GET_BOOL(DataNameHelper.Z1_IN_CALIBRATED, out result);
                    var z2Calibrated = dataManager.GET_BOOL(DataNameHelper.Z2_IN_CALIBRATED, out result);
                    var z3Calibrated = dataManager.GET_BOOL(DataNameHelper.Z3_IN_CALIBRATED, out result);

                    Dictionary<string, bool> axisEnables;
                    Dictionary<string, bool> axisCalibrates;

                    if (this.deviceManager.GetDeviceName("MOTION") == "PMAC")
                    {
                        axisEnables = new Dictionary<string, bool> { { "X", xEnableValue }, { "Y", yEnableValue }, { "T", tEnableValue }, { "Z1", z1EnableValue }, { "Z2", z2EnableValue }, { "Z3", z3EnableValue } };

                        axisCalibrates = new Dictionary<string, bool> { { "X", xCalibrated }, { "Y", yCalibrated }, { "T", tCalibrated }, { "Z1", z1Calibrated }, { "Z2", z2Calibrated }, { "Z3", z3Calibrated } };
                    }
                    else
                    {
                        axisEnables = new Dictionary<string, bool> { { "X", xEnableValue }, { "Y", yEnableValue }, { "T", tEnableValue }, { "Z1", z1EnableValue } };

                        axisCalibrates = new Dictionary<string, bool> { { "X", xCalibrated }, { "Y", yCalibrated }, { "T", tCalibrated }, { "Z1", z1Calibrated } };
                    }

                    foreach (var axisEnable in axisEnables)
                    {
                        if (!axisEnable.Value)
                        {
                            var message = Resources.ResourceManager.GetString("MSG_STANDBY_MOTION_INIT");
                            CurrentStepStatus = "Motion Init";
                            StatusText = $"{axisEnable.Key} axis : ${message}";
                            standbyProgress = StandbyProgress.Init;
                            this.MotionInitStatusBackground = MediaBrushes.LightGray;
                            this.VisionStatusBackground = MediaBrushes.LightGray;
                            this.WaferLoadingStatusBackground = MediaBrushes.LightGray;
                            this.WaferAlignStatusBackground = MediaBrushes.LightGray;
                            this.MeasureStandbyStatusBackground = MediaBrushes.LightGray;
                            return;
                        }
                    }

                    //foreach (var axisCalibrate in axisCalibrates)
                    //{
                    //    if (!axisCalibrate.Value)
                    //    {
                    //        StatusText = $"{axisCalibrate.Key} is not calibrated.";
                    //        standbyProgress = StandbyProgress.Init;
                    //        return;
                    //    }
                    //}

                    MotionInitStatusBackground = MediaBrushes.LightGreen;
                    standbyProgress = StandbyProgress.WaferLoading;
                    break;
                case StandbyProgress.WaferLoading:
                    
                    var waferLoading = dataManager.GET_BOOL(DataNameHelper.WAFER_LOADING_COMPLETED, out result);

                    if (waferLoading)
                    {
                        WaferLoadingStatusBackground = MediaBrushes.LightGreen;
                        standbyProgress = StandbyProgress.VisionRegist;
                        break;
                    }
                    else
                    {
                        this.WaferLoadingStatusBackground = MediaBrushes.LightGray;
                        this.VisionStatusBackground = MediaBrushes.LightGray;
                        this.WaferAlignStatusBackground = MediaBrushes.LightGray;
                        this.MeasureStandbyStatusBackground = MediaBrushes.LightGray;
                        CurrentStepStatus = "Wafer Loading";
                        StatusText = Resources.ResourceManager.GetString("MSG_STANDBY_WAFER_LOADING_NOT_COMPLETED");
                        standbyProgress = StandbyProgress.Init;
                        break;
                    }
                case StandbyProgress.VisionRegist:

                    if (imageProcessing.IsGrabStart)
                    {
                        VisionStatusBackground = MediaBrushes.LightGreen;
                        standbyProgress = StandbyProgress.WaferAlign;
                        break;
                    }
                    else
                    {
                        this.VisionStatusBackground = MediaBrushes.LightGray;
                        this.WaferAlignStatusBackground = MediaBrushes.LightGray;
                        this.MeasureStandbyStatusBackground = MediaBrushes.LightGray;
                        CurrentStepStatus = "Vision Ready";
                        StatusText = Resources.ResourceManager.GetString("MSG_STANDBY_VISION_NOT_READY");
                        standbyProgress = StandbyProgress.Init;
                        break;
                    }
                case StandbyProgress.WaferAlign:

                    if (this.waferAlignService.IsAlignCompleted)
                    {
                        this.WaferAlignStatusBackground = MediaBrushes.LightGreen;
                        standbyProgress = StandbyProgress.MeasureStandby;
                    }
                    else
                    {
                        this.WaferAlignStatusBackground = MediaBrushes.LightGray;
                        this.MeasureStandbyStatusBackground = MediaBrushes.LightGray;
                        CurrentStepStatus = "Wafer Align";
                        StatusText = Resources.ResourceManager.GetString("MSG_STANDBY_WAFER_ALIGN_NOT_COMPLETED");
                        standbyProgress = StandbyProgress.Init;
                    }
                    break;
                case StandbyProgress.MeasureStandby:
                    this.MeasureStandbyStatusBackground = MediaBrushes.LightGreen;
                    standbyProgress = StandbyProgress.Init;
                    CurrentStepStatus = "Measure Standby";
                    StatusText = Resources.ResourceManager.GetString("MSG_STANDBY_MEASURE_READY");
                    break;
            }
        }

        private void BlankProcessButton(StandbyProgress standbyProgress)
        {
            switch (standbyProgress)
            {
                case StandbyProgress.Init:
                    return;
                case StandbyProgress.DeviceAttach:
                    return;
            }
        }

        private void DataManager_DataChangedEvent(object sender, DataVMList.DataVM e)
        {

        }
    }
}
