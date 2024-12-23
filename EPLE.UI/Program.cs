using EPLE;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Formatting.Display;
using Serilog.Sinks.WinForms.Base;

namespace EPLE.UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledException;

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var builder = Startup.CreateHostBuilder(loggerConfigure: (config) =>
            {
                config.WriteToSimpleAndRichTextBox(new MessageTemplateTextFormatter("[{Timestamp:yyMMddTHH:mm:ss.fffffffz}][{Level}] {Message:lj}{NewLine}{Exception}"));
            }).ConfigureServices(ConfigureServices);

            var host = builder.Build();
            host.Start();

            ApplicationConfiguration.Initialize();
            Application.Run(host.Services.GetRequiredService<Form1>());

            host.StopAsync();
            host.WaitForShutdown();
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<Form1>();
        }

        private static void GlobalUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is not Exception ex) return;
            MessageBox.Show($"Please contact developer with below msg.\n\n\"{ex.Message}\"\n\nException: {ex.GetType()}\n{ex.TargetSite} in {ex.Source}", "Program Error");
            Application.Exit();
        }
    }
}