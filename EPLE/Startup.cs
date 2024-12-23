global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
using EPLE.Data;
using EPLE.Manager;
using EPLE.Options;
using EPLE.Service;
using EPLE.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EPLE
{
    public static class Startup
    {
        public static IHostBuilder CreateHostBuilder(Action<LoggerConfiguration>? loggerConfigure = null)
        {
            var builder = Host.CreateDefaultBuilder();

            builder.ConfigureLogging(logging => logging.ClearProviders())
                .UseSerilog((context, services, loggerConfiguration) =>
                {
                    var config = loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                    loggerConfigure?.Invoke(config);
                });

            builder.ConfigureServices((context, services) =>
            {
                services.Configure<DataOptions>(context.Configuration.GetSection(DataOptions.Data));
            });

            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<DataVMList>();

            });

            builder.ConfigureServices((context, services) =>
            {
                var dataOptions = context.Configuration.GetSection(DataOptions.Data).Get<DataOptions>();
                var dbPath = (dataOptions?.Db) ?? throw new Exception("DB가 설정되어있지 않습니다");

                services.AddDbContext<DataContext>(options =>
                {
                    options
                        .EnableSensitiveDataLogging(dataOptions?.EnableSensitiveDataLogging ?? false)
                        .UseJetOleDb(@$"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};");
                }, ServiceLifetime.Singleton);
                services.AddSingleton<DataRepository>();

                services.AddSingleton<DataManager>();
                services.AddSingleton<DeviceManager>();

                services.AddSingleton<SequenceService>();
                services.AddHostedService<SequenceService>(provider => provider.GetRequiredService<SequenceService>());

            });

            return builder;
        }
    }
}
