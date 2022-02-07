using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace TgCloud
{
    /// <summary>
    /// EntryPoint Class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// EntryPoint
        /// </summary>
        /// <param name="args">аргументы командной строки</param>
        /// <returns></returns>
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        /// <summary>
        /// Создает хост для авторизации пользователей
        /// </summary>
        /// <param name="args">аргументы команднйо строки</param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", false)
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup((t) => new Startup(config));
                    webBuilder.UseUrls(new[] { config["LocalHost"] });
                });
        }
    }
}