
using EPLE.Manager;
using EPLE.ViewModel;
using System.Drawing;
using System.IO;
using System.Windows;
using MediaBrush = System.Windows.Media.Brush;
using MediaBrushes = System.Windows.Media.Brushes;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using EPLE.Core.Device.Interface;
using EPLE.Data;
using EPLE.Service;
using PropertyChanged;
using System;
using System.Linq;
using MotionDiagnostics.UserControls.ViewModels;
using PrismCommands;
using Prism.Commands;
using EPLE.ImageProcessing;
using HalconDotNet;


namespace MotionDiagnostics.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class MainPageViewModel
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;   
        private readonly DataVMList dataVMList;
        private readonly DeviceVMList deviceVMList;
        private readonly AccuracyMeasureService accuracyMeasureService;
        private readonly DispatcherTimer grabTimer;
        private readonly HalconImageProcessing imageProcessing;
        private readonly SequenceService sequenceService;
        private readonly WaferAlignService waferAlignService;
        private DispatcherTimer statusMonitoringTimer = new DispatcherTimer();

        public StandbyProgressControlViewModel StandbyProgressControlViewModel { get; set; }

        public WaferLoadingControlViewModel WaferLoadingControlViewModel { get; set; }

        public VisionSettingControlViewModel VisionSettingControlViewModel { get; set; }

        public WaferAlignControlViewModel WaferAlignControlViewModel { get; set; }

        public HalconImageProcessingControlViewModel HalconImageProcessingControlViewModel { get; set; }

        public MediaBrush MotionStatusBackground { get; set; }
        public MediaBrush VisionStatusBackground { get; set; }

        public string MotionType { get; set; }

        public string VisionType { get; set; }

        private BitmapImage ImageSource { get; set; }


        public MainPageViewModel(DataManager dataManager, DeviceManager deviceManager, DataVMList dataVMList, DeviceVMList deviceVMList, AccuracyMeasureService accuracyMeasureService, SequenceService sequenceService, HalconImageProcessing imageProcessing, WaferAlignService waferAlignService, WaferLoadingControlViewModel waferLoadingControlViewModel, StandbyProgressControlViewModel standbyProgressControlViewModel)
        {
            this.dataManager = dataManager;
            this.deviceManager = deviceManager;
            this.accuracyMeasureService = accuracyMeasureService;
            this.dataVMList = dataVMList;
            this.deviceVMList = deviceVMList;
            this.imageProcessing = imageProcessing;
            this.sequenceService = sequenceService;
            this.waferAlignService = waferAlignService;
            this.WaferLoadingControlViewModel = waferLoadingControlViewModel;
            this.StandbyProgressControlViewModel = standbyProgressControlViewModel;
            //this.WaferLoadingControlViewModel = new WaferLoadingControlViewModel(dataManager, deviceManager, sequenceService, dialogCoordinator);
            //this.VisionSettingControlViewModel = new VisionSettingControlViewModel(dataManager, deviceManager, imageProcessing, dialogCoordinator);
            this.WaferAlignControlViewModel = new WaferAlignControlViewModel(dataManager, deviceManager, imageProcessing, waferAlignService);
            //this.HalconImageProcessingControlViewModel = new HalconImageProcessingControlViewModel(dataManager, deviceManager, imageProcessing);

            // Initialize the timer for monitoring the status of the motion device
            statusMonitoringTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = new System.TimeSpan(0, 0, 0, 0, 100)
            };

            statusMonitoringTimer.Tick += MotionStatusTimerCallback;
            statusMonitoringTimer.Tick += VisionStatusTimerCallback;

            // Initialize the motion control user control
            InitializeMotionControl();

            Bitmap bitmap = LoadBitmapFromResource("Resources/cam_default.png");
            ImageSource = Convert(bitmap);
            grabTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = new System.TimeSpan(0, 0, 0, 0, 20)
            };

            grabTimer.Tick += GrabTimerCallback;
        }

        private void VisionStatusTimerCallback(object sender, EventArgs e)
        {
            var devVision = deviceVMList.Devices.FirstOrDefault((device) => device.DeviceType == "VISION");
            if (devVision == null)
            {
                VisionStatusBackground = MediaBrushes.Gray;
                VisionType = "NO VISION";
                return;
            }
            if (deviceManager.IsDeviceMode(devVision.DeviceName) != DevMode.CONNECT)
            {
                VisionStatusBackground = MediaBrushes.Red;
                VisionType = devVision.DeviceName;
            }
            else
            {
                VisionStatusBackground = MediaBrushes.Green;
                VisionType = devVision.DeviceName;
            }
        }

        private void MotionStatusTimerCallback(object sender, EventArgs e)
        {
            var devMotion = deviceVMList.Devices.FirstOrDefault((device) => device.DeviceType == "MOTION");
            if (devMotion == null)
            {
                MotionStatusBackground = MediaBrushes.Gray;
                MotionType = "NO MOTION";
                return;
            }

            var xEnable = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.X_IN_ENABLE);
            var yEnable = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Y_IN_ENABLE);
            var tEnable = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.T_IN_ENABLE);
            var z1Enable = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z1_IN_ENABLE);
            var z2Enable = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z2_IN_ENABLE);
            var z3Enable = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z3_IN_ENABLE);

            var xActPos = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.X_IN_ACTPOS);

            var x = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.X_IN_CALIBRATED);
            var y = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Y_IN_CALIBRATED);
            var t = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.T_IN_CALIBRATED);
            var z1 = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z1_IN_CALIBRATED);
            var z2 = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z2_IN_CALIBRATED);
            var z3 = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z3_IN_CALIBRATED);

            if (deviceManager.IsDeviceMode(devMotion.DeviceName) != DevMode.CONNECT)
            {
                MotionStatusBackground = MediaBrushes.Red;
                MotionType = devMotion.DeviceName;
            }
            else if ((x.Value is int x_calibrated && x_calibrated == (int)STATUS.ON) ||
                     (y.Value is int y_calibrated && y_calibrated == (int)STATUS.ON) ||
                     (t.Value is int t_calibrated && t_calibrated == (int)STATUS.ON) ||
                     (z1.Value is int z1_calibrated && z1_calibrated == (int)STATUS.ON) ||
                     (z2.Value is int z2_calibrated && z2_calibrated == (int)STATUS.ON) ||
                     (z3.Value is int z3_calibrated && z3_calibrated == (int)STATUS.ON))
            {
                MotionStatusBackground = MediaBrushes.Green;
                MotionType = devMotion.DeviceName;
            }
            else
            {
                MotionStatusBackground = MediaBrushes.Red;
                MotionType = devMotion.DeviceName;
            }
        }


        private Bitmap LoadBitmapFromResource(string resourcePath)
        {
            var uri = new Uri($"pack://application:,,,/{resourcePath}");
            using (var stream = Application.GetResourceStream(uri).Stream)
            {
                return new Bitmap(stream);
            }
        }

        public void Start()
        {
            if(dataManager.IsDeviceMode("Camera") == EPLE.Core.Device.Interface.DevMode.CONNECT)
                grabTimer.Start();
        }

        private void GrabTimerCallback(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                Bitmap bitmap;
                dataManager.GET_DATA("CAM.IN.PICTURE", out object value);
 
                if (value == null || !(value is Bitmap))
                    bitmap = LoadBitmapFromResource("Resources/cam_default.png");
                else
                    bitmap = (Bitmap)value;

                ImageSource = Convert(bitmap);
            }));
        }

        public void Stop()
        {
            grabTimer.Stop();
            Bitmap bitmap = LoadBitmapFromResource("Resources/cam_default.png");
            ImageSource = Convert(bitmap);
        }

        private readonly object _bitmapLock = new object();

        public BitmapImage Convert(Bitmap src)
        {
            lock (_bitmapLock)
            {
                MemoryStream ms = new MemoryStream();
                ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        [DelegateCommand]
        public void Loaded()
        {
            statusMonitoringTimer.Start();
        }

        [DelegateCommand]
        public void Unloaded()
        {
            statusMonitoringTimer.Stop();
        }


    }
}
