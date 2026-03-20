using System.Windows;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MDS.UI.ViewModels;
using MDS.UI.UserControls.ViewModels;

namespace MDS.UI
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public static IContainer Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            var services = new ServiceCollection();

            // ViewModels
            services.AddSingleton<ShellViewModel>();
            services.AddSingleton<MainPageViewModel>();
            services.AddSingleton<AccuracyViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<MotionModalVM>();
            services.AddSingleton<WaferControlViewModel>();
            services.AddSingleton<WaferLoadingControlViewModel>();
            services.AddSingleton<StandbyProgressControlViewModel>();
            services.AddSingleton<RepeatMeasureControlViewModel>();
            services.AddSingleton<MappingAndMeasureControlViewModel>();
            services.AddSingleton<WaferAlignControlViewModel>();
            services.AddSingleton<DialogService>();

            builder.Populate(services);
            Services = builder.Build();

            base.OnStartup(e);
        }

        public static T GetService<T>() where T : class
            => Services.Resolve<T>();
    }
}
