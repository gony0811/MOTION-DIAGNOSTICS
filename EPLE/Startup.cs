using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutofacSerilogIntegration;
using EPLE.Data.Repository;
using EPLE.Data.Entity;
using EPLE.Interface;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;
using System.IO;
using Serilog;
using EPLE.Options;
using EPLE.ViewModel;
using EPLE.Service;
using EPLE.Manager;
using System.IO.Pipelines;

namespace EPLE
{
    public static class Startup
    {

        public static ContainerBuilder Builder()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();


            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

            var services = new ServiceCollection();
            //serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger));
            services.AddSingleton(configuration);
            services.Configure<DataOptions>((option) => configuration.GetSection(DataOptions.Data));

            services.AddSingleton<ISessionFactory>(ISessionFactory =>
            {
                var cfg = new Configuration();
                cfg.Configure(); // hibernate.cfg.xml 로드
                cfg.AddAssembly(Assembly.GetExecutingAssembly()); // 매핑 로드
                new SchemaExport(cfg).Create(true, true); // Database create
                return cfg.BuildSessionFactory();
            });

            services.AddScoped<ISession>(c => c.GetService<ISessionFactory>().OpenSession());

            services.AddSingleton<DataRepository>();

            services.AddSingleton<DataVMList>();
            services.AddSingleton<DeviceVMList>();
            services.AddSingleton<AlarmVMList>();
            services.AddSingleton<MeasureVMList>();

            services.AddSingleton<DataManager>();
            services.AddSingleton<DeviceManager>();
            services.AddSingleton<AlarmManager>();
            services.AddSingleton<MeasureManager>();

            services.AddSingleton<SequenceService>();
            services.AddSingleton<AccuracyMeasureService>();
            services.AddSingleton<WaferAlignService>();

            // Serilog을 Microsoft.Extensions.Logging.ILogger로 등록
            //services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, dispose: true));

            var builder = new ContainerBuilder();

            builder.RegisterLogger(logger);

            builder.Populate(services);

            // NHibernate SessionFactory 등록
            //builder.Register(c =>
            //{
            //    var cfg = new Configuration();
            //    cfg.Configure(); // hibernate.cfg.xml 로드
            //    cfg.AddAssembly(Assembly.GetExecutingAssembly()); // 매핑 로드
            //    new SchemaUpdate(cfg).Execute(true, true); // Database create
            //    return cfg.BuildSessionFactory();
            //}).As<ISessionFactory>().SingleInstance();

            // NHibernate Session 등록 (요청마다 새 세션)
            //builder.Register(c => c.Resolve<ISessionFactory>().OpenSession())
            //       .As<ISession>()
            //       .InstancePerLifetimeScope();

            // Repository 및 서비스 등록
            //builder.RegisterType<AlarmRepository>().As<AlarmRepository>();
            //builder.RegisterType<DataRepository>().As<DataRepository>();
            //builder.RegisterType<DeviceRepository>().As<DeviceRepository>();

            return builder;
        }
    }
}
