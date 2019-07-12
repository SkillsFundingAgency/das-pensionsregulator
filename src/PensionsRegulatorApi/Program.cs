using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using SFA.DAS.Configuration.AzureTableStorage;

namespace PensionsRegulatorApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Starting up host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appSettings.json", optional: false, reloadOnChange: false);
                    config.AddAzureTableStorage(options => {
                        options.ConfigurationKeys = new[] { "SFA.DAS.PensionsRegulatorApi" };
                        options.EnvironmentNameEnvironmentVariableName = "APPSETTING_EnvironmentName";
                        options.StorageConnectionStringEnvironmentVariableName = "APPSETTING_ConfigurationStorageConnectionString";
                        options.PreFixConfigurationKeys = false;
                    });
                    config.AddJsonFile($"appSettings.{environmentName}.json", optional: true, reloadOnChange: false);
                    config.AddEnvironmentVariables();
                    config.AddUserSecrets<Startup>();
                })
                .UseUrls("https://localhost:5051")
                .UseNLog();
    }
}
