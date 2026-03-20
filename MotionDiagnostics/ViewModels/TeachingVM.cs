using MotionDiagnostics.Modal;
using MotionDiagnostics.Model;
using System.Windows;
using PropertyChanged;
using PrismCommands;
using System.Threading;
using System.Threading.Tasks;
using System;
using Prism.Commands;

namespace MotionDiagnostics.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public partial class TeachingVM
    {
        public string LabelContent { get; set; }

        public string Name { get; set; }

        public double CommandPosition { get; set; }

        public double FeedbackPosition { get; set; }

        public double Velocity { get; set; }

        public double Pitch { get; set; }

        public bool AlarmStatus { get; set; }

        public bool EnableStatus { get; set; }


        public bool BusyStatus { get; set; }


        private CancellationTokenSource _cancellationTokenSource;

        public TeachingVM(string name)
        {
            this.Name = name;
            this.LabelContent = this.Name.Split(new char[] { ' ' })[1] + " POS";

            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(async () => { await ValueUpdate(_cancellationTokenSource.Token); });

            TeachCommand = new DelegateCommand<object>(Teach);
            SetCommand = new DelegateCommand<object>(Set);
            MoveCommand = new DelegateCommand<object>(Move);
        }

        /// <summary>
        /// 주기적으로 센서 데이터를 읽어옵니다.
        /// </summary>
        private async Task ValueUpdate(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    try
                    {
                        // 센서 데이터를 읽어옵니다.
                        var data = await GetInformationByName(Name);

                        // 센서별 데이터를 독립적으로 업데이트
                        UpdateDataSafely(() => CommandPosition = data.CommandPosition, "CommandPosition");
                        UpdateDataSafely(() => FeedbackPosition = data.FeedbackPosition, "FeedbackPosition");
                        UpdateDataSafely(() => AlarmStatus = data.AlarmStatus, "AlarmStatus");
                        UpdateDataSafely(() => EnableStatus = data.EnableStatus, "EnableStatus");
                        UpdateDataSafely(() => BusyStatus = data.BusyStatus, "BusyStatus");
                    }
                    catch (Exception ex)
                    {
                        // 전체 데이터 읽기 실패 시 처리
                        LogError($"Failed to read sensor data: {ex.Message}");
                    }
                }

                // 데이터를 1초마다 갱신
                await Task.Delay(1000, cancellationToken);
            }
        }

        /// <summary>
        /// 개별 데이터를 업데이트하며 예외를 처리합니다.
        /// </summary>
        private void UpdateDataSafely(Action updateAction, string sensorName)
        {
            try
            {
                updateAction.Invoke();
            }
            catch (Exception ex)
            {
                LogError($"Error updating {sensorName}: {ex.Message}");
            }
        }

        /// <summary>
        /// 이름 기반으로 데이터를 읽어옵니다.
        /// </summary>
        private Task<TeachingData> GetInformationByName(string name)
        {
            // 가상의 데이터를 반환하는 예제 (센서 중 일부는 실패 시뮬레이션)
            var random = new Random();
            var data = new TeachingData
            {
                CommandPosition = random.NextDouble() * 100,
                FeedbackPosition = random.NextDouble() * 100,
                AlarmStatus = random.Next(0, 2) == 0,
                EnableStatus = true,
                BusyStatus = false
            };

            // 특정 센서에서 에러 발생 시뮬레이션

            return Task.FromResult(data);
        }

        /// <summary>
        /// 로그를 기록합니다.
        /// </summary>
        private void LogError(string message)
        {
            // 로그를 파일 또는 콘솔에 기록
            Console.WriteLine(message);
        }

        /// <summary>
        /// 주기적인 업데이트를 시작합니다.
        /// </summary>
        public void StartUpdating()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource = new CancellationTokenSource();
            }

            _ = ValueUpdate(_cancellationTokenSource.Token);
        }

        /// <summary>
        /// 업데이트 작업을 중단합니다.
        /// </summary>
        public void StopUpdating()
        {
            _cancellationTokenSource.Cancel();
        }

        public DelegateCommand<object> TeachCommand { get; private set; }

        public void Teach(object parameter)
        {
            var modalWindow = MotionModal.Instance;
            if (!modalWindow.IsVisible)
            {
                modalWindow.Owner = Application.Current.MainWindow;
                modalWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                modalWindow.SetAndDisplayMotion("");
            }
            else
            {
                modalWindow.Activate();
            }
        }

        public DelegateCommand<object> SetCommand { get; private set; }

        public void Set(object parameter)
        {
            // 설정 작업 수행
        }

        public DelegateCommand<object> MoveCommand { get; private set; }

        public void Move(object parameter)
        {
            // 설정 작업 수행
        }
    }

}
