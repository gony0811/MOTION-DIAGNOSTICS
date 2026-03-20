using EPLE.Core.Device.Interface;
using EPLE.Data;
using EPLE.Manager;
using EPLE.Service;
using EPLE.ViewModel;
using MotionDiagnostics.UserControls.ViewModels;
using MotionDiagnostics.Model;
using PropertyChanged;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MediaBrush = System.Windows.Media.Brush;
using MediaBrushes = System.Windows.Media.Brushes;
using Prism.Commands;
using PrismCommands;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MotionDiagnostics.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class AccuracyViewModel : IDataErrorInfo
    {
        private readonly DataManager dataManager;
        private readonly DeviceManager deviceManager;
        private readonly DataVMList dataVMList;
        private readonly DeviceVMList deviceVMList;

        private readonly AccuracyMeasureService accuracyMeasureService = null;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private DispatcherTimer statusMonitoringTimer = new DispatcherTimer();

        public MediaBrush MotionStatusBackground { get; set; }
        public MediaBrush VisionStatusBackground { get; set; }
        public HorizontalSetting HorizontalSetting { get; set; }
        public string MotionType { get; set; }
        public MotionSetting MotionSetting{ get; set;}
        public ModelManagement ModelManagement { get; set; }

        public List<Tuple<int, int>> Selections { get; set; } = new List<Tuple<int, int>>();
        public int SelectedRow { get; set; }
        public int SelectedCol { get; set; }

        public BitmapImage ImageSource { get; set; }

        public int RepeatPointCol { get; set; } = 0;
        public int RepeatPointRow { get; set; } = 0;
        public int RepeatCount { get; set; } = 1;

        
        public bool IsPneumaticOptionChecked { get; set; } = false;

        public bool IsXGuideSolOptionChecked { get; set; } = false;

        public bool IsXSupportSolOptionChecked { get; set; } = false;

        public bool IsY1GuideSolOptionChecked { get; set; } = false;

        public bool IsY1SupportSolOptionChecked { get; set; } = false;

        public bool IsY2GuideSolOptionChecked { get; set; } = false;

        public bool IsY2SupportSolOptionChecked { get; set; } = false;

        public string Error => string.Empty;

        public int Diameter { get; set; }     // 지름 
        public int GridWidth { get; set; }
        public int GridSize { get; set; }   // diameter / gridWidth : 그려질 Grid의 갯수를 나타냄
        //private int buttonSize = 10;

        public WaferControlViewModel WaferControlViewModel { get; set; }
        public RepeatMeasureControlViewModel RepeatMeasureControlViewModel { get; set; }

        public MappingAndMeasureControlViewModel MappingAndMeasureControlViewModel { get; set; }

        public ObservableCollection<MeasureVMList.MeasureVM> MeasureResults { get; set; } = new ObservableCollection<MeasureVMList.MeasureVM>();

        public string this[string columnName]
        {
            get
            {
                if (columnName == "RepeatCount")
                {
                    if (RepeatCount <= 0)
                    {
                        return "Repeat must be greater than 0";
                    }
                }
                return "";
            }
        }
        public AccuracyViewModel(DataManager dataManager, DeviceManager deviceManager, DataVMList dataVMList, DeviceVMList deviceVMList, WaferControlViewModel waferControlViewModel, AccuracyMeasureService accuracyMeasureService, RepeatMeasureControlViewModel repeatMeasureControlViewModel, MappingAndMeasureControlViewModel mappingAndMeasureControlViewModel)
        {
            this.dataManager = dataManager;
            this.deviceManager = deviceManager;
            this.accuracyMeasureService = accuracyMeasureService;
            this.dataVMList = dataVMList;
            this.deviceVMList = deviceVMList;

            this.WaferControlViewModel = waferControlViewModel;
            this.RepeatMeasureControlViewModel = repeatMeasureControlViewModel;
            this.MappingAndMeasureControlViewModel = mappingAndMeasureControlViewModel;

            statusMonitoringTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = new System.TimeSpan(0, 0, 0, 0, 100)
            };

            statusMonitoringTimer.Tick += StatusMonitoringTimerCallback;

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

        private void StatusMonitoringTimerCallback(object sender, EventArgs e)
        {
            var devMotion = deviceVMList.Devices.FirstOrDefault((device) => device.DeviceType == "MOTION");
            if (devMotion == null) return;

            var x = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.X_IN_CALIBRATED);
            var y = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Y_IN_CALIBRATED);
            var t = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.T_IN_CALIBRATED);
            var z1 = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z1_IN_CALIBRATED);
            var z2 = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z2_IN_CALIBRATED);
            var z3 = dataVMList.DataList.FirstOrDefault((data) => data.Name == DataNameHelper.Z3_IN_CALIBRATED);

            if (deviceManager.IsDeviceMode(devMotion.DeviceName) != DevMode.CONNECT)
            {
                MotionStatusBackground = MediaBrushes.Gray;
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
            }
            else
            {
                MotionStatusBackground = MediaBrushes.Red;
                MotionType = devMotion.DeviceName;
            }
        }

        [DelegateCommand]
        public async void Loaded()
        {
            await Initialize();
            statusMonitoringTimer.Start();
        }

        [DelegateCommand]
        public void Unloaded()
        {
            statusMonitoringTimer.Stop();
        }

        [DelegateCommand]
        public async Task Initialize()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            var center_col_index = this.dataManager.GET_INT(DataNameHelper.X_CENTER_INDEX, out _);
            var center_row_index = this.dataManager.GET_INT(DataNameHelper.Y_CENTER_INDEX, out _);
            var grid_pitch = this.dataManager.GET_INT(DataNameHelper.SET_GRID_PITCH, out _);

            await accuracyMeasureService.Initialize(_cancellationTokenSource.Token, centerGridCol: center_col_index, centerGridRow: center_row_index, gridPitch: grid_pitch );
        }

        [DelegateCommand]
        public void CancelRepeatMeasure()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                this.dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, (int)STATUS.ON);
                this.dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, (int)STATUS.ON);
            }
        }

       

        [DelegateCommand]
        public void CancelMovePosition()
        {
            if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                this.dataManager.SET_DATA(DataNameHelper.X_OUT_STOP, (int)STATUS.ON);
                this.dataManager.SET_DATA(DataNameHelper.Y_OUT_STOP, (int)STATUS.ON);
            }
        }

        [DelegateCommand]
        public async Task MovePositionAsync()
        {
            // 팝업창 띄우기

            // 확인/취소

            // 확인 버튼 클릭 시

            // 취소면 아래 코드 실행 안함

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            this.dataManager.SET_DATA(DataNameHelper.X_OUT_ENABLE, (int)STATUS.ON);
            this.dataManager.SET_DATA(DataNameHelper.Y_OUT_ENABLE, (int)STATUS.ON);
            this.dataManager.SET_DATA(DataNameHelper.SET_X_INDEX, SelectedCol);
            this.dataManager.SET_DATA(DataNameHelper.SET_Y_INDEX, SelectedRow);

            await this.accuracyMeasureService.MovePosition(token, SelectedRow, SelectedCol);
        }

        [DelegateCommand]
        public void RepeatPointInput()
        {
            RepeatMeasureControlViewModel.SelectedColumn = SelectedCol;
            RepeatMeasureControlViewModel.SelectedRow = SelectedRow;
        }

        [DelegateCommand]
        public void SelectionChanged(object position)
        {
            //SelectedRow 
        }
    }
}
