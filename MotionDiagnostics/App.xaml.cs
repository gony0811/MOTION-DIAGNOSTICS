using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EPLE.Data.Entity;
using EPLE.Data.Repository;
using EPLE.ImageProcessing;
using EPLE.Manager;
using Microsoft.Extensions.DependencyInjection;
using MotionDiagnostics.Common;
using MotionDiagnostics.UserControls;
using MotionDiagnostics.UserControls.ViewModels;
using MotionDiagnostics.ViewModels;

namespace MotionDiagnostics
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        /// <summary>
        /// Gets the <see cref="IContainer"/> instance to resolve application services.
        /// </summary>
        public static IContainer Services { get; private set; }
        protected override async void OnStartup(StartupEventArgs e)
        {
            var builder = EPLE.Startup.Builder();

            var services = new ServiceCollection();

            /// SEAN.KIM
            /// 여기에 서비스 ViewModel들을 추가 하면 됩니다.
            services.AddSingleton<WaferControlViewModel>();
            services.AddSingleton<AccuracyViewModel>();
            services.AddSingleton<MainPageViewModel>();
            services.AddSingleton<ShellViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<MotionModalVM>();
            services.AddSingleton<DialogService>();
            services.AddSingleton<StandbyProgressControlViewModel>();
            services.AddSingleton<WaferLoadingControlViewModel>();
            services.AddSingleton<RepeatMeasureControlViewModel>();
            services.AddSingleton<HalconImageProcessing>();
            services.AddSingleton<VisionSettingControlViewModel>();
            services.AddSingleton<VisionSettingControl>();
            services.AddSingleton<HalconImageProcessingControlViewModel>();
            services.AddSingleton<HalconImageProcessingControl>();
            services.AddSingleton<MappingAndMeasureControlViewModel>();

            /// SEAN.KIM

            builder.Populate(services);

            var container = builder.Build();

            Services = container;


            using (var scope = container.BeginLifetimeScope())
            {
                var dataRepository = scope.Resolve<DataRepository>();
                
                // 데이터베이스 초기화
                //dataRepository.DeleteAll<DataConfigEntity>();
                //dataRepository.DeleteAll<DeviceConfigEntity>();
                //dataRepository.DeleteAll<AlarmConfigEntity>();

                //dataRepository.AddRange<DataConfigEntity>(DatabaseInitializeHelper.DataConfig);
                //dataRepository.AddRange<DeviceConfigEntity>(DatabaseInitializeHelper.DeviceConfig);
                //dataRepository.AddRange<AlarmConfigEntity>(DatabaseInitializeHelper.AlarmConfig);

                dataRepository.LoadData();
                dataRepository.ChangeMotionDevice("ACS");
            }

            await ResetMeasureData();
        }

        public async Task ResetMeasureData()
        {
            var measureManager = Services.Resolve<MeasureManager>();

            await Task.Run(() =>
            {
                measureManager.ResetAllMeasureData();
            });
          
        }

        // 전역적으로 서비스 가져오기
        public static T GetService<T>() => Services.Resolve<T>();
    }
}
