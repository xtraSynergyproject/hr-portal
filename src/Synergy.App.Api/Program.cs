using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api
{
    public class Program
    {

        private static string _env = "dev";
        public static void Main(string[] args)
        {
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
            }).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
