using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web
{
    public class Program
    {
        private static string _env = "dev";
        public static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //       .MinimumLevel.Information()
            //        .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error).WriteTo.RollingFile(@"Logs\Error-{Date}.log"))
            //       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.RollingFile(@"Logs\Info-{Date}.log"))
            //       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug).WriteTo.RollingFile(@"Logs\Debug-{Date}.log"))
            //       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.RollingFile(@"Logs\Warning-{Date}.log"))

            //       .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal).WriteTo.RollingFile(@"Logs\Fatal-{Date}.log"))
            //       // .WriteTo.RollingFile(@"Logs\Verbose-{Date}.log")
            //       .CreateLogger();
            SetEnvironment();
            CreateHostBuilder(args).Build().Run();
        }


        private static void SetEnvironment()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false)
                    .Build();
                _env = config.GetSection("Environment").Value;
            }
            catch (Exception)
            {
                _env = "dev";
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json");
                config.AddJsonFile($"appsettings.{_env}.json",
                    optional: true,
                    reloadOnChange: true);

            })
            .ConfigureLogging((hostingContext, builder) =>
            {
                builder.AddFile("Logs/info-{Date}.txt", LogLevel.Information);
                builder.AddFile("Logs/error-{Date}.txt", LogLevel.Error);
               // builder.AddFile("Logs/warning-{Date}.txt", LogLevel.Warning);
                //builder.AddFile("Logs/info-{Date}.txt", LogLevel.Information, new Dictionary<string, LogLevel> { { "warning", LogLevel.Error }, { "error", LogLevel.Warning } });


            })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
