using EPLE.Data.Entity;
using EPLE.Manager;
using MotionDiagnostics.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using PropertyChanged;
using PrismCommands;
using Prism.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;



namespace MotionDiagnostics.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class SettingsViewModel
    {
        private readonly DataManager dataManager;
        public PositionStatus PositionStatus { get; set; }

        public double Speed { get; set; } = 1; // 초기값: 1

        private CancellationTokenSource statusCancellationTokenSource;

        // 참조 된 데이터 View
        public ObservableCollection<TableViewModel> TableViewModels { get; set; } = new ObservableCollection<TableViewModel>();

        private readonly DeviceManager deviceManager;

        public Dictionary<string, TeachingVM> TeachingVMMap { get; set; }
        public SettingsViewModel(DataManager dataManager, DeviceManager deviceManager)
        {
            this.deviceManager = deviceManager;
            this.dataManager = dataManager;
            PositionStatus = new PositionStatus();
            InitDeviceDetails();

            var positions = new List<string>
            {
                "LOADING_T_POS", "LOADING_X_POS", "LOADING_Y_POS",
                "INSPECTION_T_POS",
                "INSPECTION_X_POS_TOP", "INSPECTION_Y_POS_TOP",
                "INSPECTION_X_POS_LEFT", "INSPECTION_Y_POS_LEFT",
                "INSPECTION_X_POS_CENTER", "INSPECTION_Y_POS_CENTER",
                "INSPECTION_X_POS_RIGHT", "INSPECTION_Y_POS_RIGHT",
                "INSPECTION_X_POS_BOTTOM", "INSPECTION_Y_POS_BOTTOM"
            };

            // TeachingVMMap 초기화
            TeachingVMMap = positions.ToDictionary(
                position => position,
                position => new TeachingVM(position.Replace("_POS", "").Replace("_", " ")));
            //positionStatus.PropertyChanged += PositionStatus_PropertyChanged;
        }

        private void InitDeviceDetails()
        {
            TableViewModels.Clear();

            foreach (var device in deviceManager.GetAllDevice())
            {
                TableViewModels.Add(new TableViewModel(device, deviceManager.Update, DeleteTableView));
            }

        }

        [DelegateCommand]
        public void CreateDevice()
        {
            var newDevice = new DeviceConfigEntity();
            TableViewModels.Add(new TableViewModel(newDevice, deviceManager.Update, DeleteTableView));
        }

        public void DeleteTableView(TableViewModel viewModel)
        {
            if (viewModel.DeviceConfig != null)
            {
                deviceManager.Delete(viewModel.DeviceConfig.Id);
            }
            TableViewModels.Remove(viewModel);
        }

        // 테스트 함수
        public async Task StartRandomStatusUpdate()
        {
            Random random = new Random();
            string[] names = { "X", "Y", "Z1", "Z2", "Z3", "T" }; // 테스트 PositionName 값들

            while (true)
            {
                // 3초 대기
                await Task.Delay(3000);

                // 랜덤 PositionName 값 설정
                PositionStatus.PositionName = names[random.Next(names.Length)];

                // 랜덤 상태값 설정
                PositionStatus.PlusLimit = random.Next(2) == 1;
                PositionStatus.HomeStat = random.Next(2) == 1;
                PositionStatus.MinusLimit = random.Next(2) == 1;
                PositionStatus.Enable = random.Next(2) == 1;
                PositionStatus.InPosition = random.Next(2) == 1;
                PositionStatus.Alarm = random.Next(2) == 1;
                PositionStatus.Busy = random.Next(2) == 1;
                PositionStatus.HomeDone = random.Next(2) == 1;

                // 출력 (디버깅용)
                Debug.WriteLine($"PositionName: {PositionStatus.PositionName}, PlusLimit: {PositionStatus.PlusLimit}, Alarm: {PositionStatus.Alarm}");
            }
        }

        private void PositionStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PositionStatus.PositionName))
            {
                ExecuteOnPositionNameChanged(PositionStatus.PositionName);
            }
        }

        //axis Name이 변경될때 사용될 함수
         private void ExecuteOnPositionNameChanged(string newName)
        {
            StartMonitoring(newName);
        }

        // Status 정보들 실시간 모니터링 
        private void StartMonitoring(string positionName)
        {
            statusCancellationTokenSource = new CancellationTokenSource();

            var token = statusCancellationTokenSource.Token;

            Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        // 모니터링 로직

                        // 1초마다 업데이트
                        await Task.Delay(1000, token);
                        //UpdateStatusInfo(positionName);
                    }
                } catch (TaskCanceledException)
                {
                    Debug.WriteLine($"Monitoring for axis {positionName} stopped.");
                }
            }, token);
        }
    }
}
