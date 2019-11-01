using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using PensionsRegulator.Functions.Infrastructure.Configuration;
using PensionsRegulator.Functions.Infrastructure.Logging;

[assembly: FunctionsStartup(typeof(PensionsRegulator.Functions.Startup))]

namespace PensionsRegulator.Functions
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup() { }

         public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var executionContextOptions = builder.Services.BuildServiceProvider()
                .GetService<IOptions<ExecutionContextOptions>>().Value;

            builder.AddConfiguration((configBuilder) =>
            {
                var tempConfig = new ConfigurationBuilder()
                    .SetBasePath(executionContextOptions.AppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                var configuration = configBuilder
                    .SetBasePath(executionContextOptions.AppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddAzureTableStorageConfiguration(
                        tempConfig["ConfigurationStorageConnectionString"],
                        tempConfig["ConfigNames"].Split(','),
                        tempConfig["EnvironmentName"],
                        tempConfig["ConfigurationVersion"])
                    .Build();

                return configuration;
            });

            Configuration = builder.GetCurrentConfiguration();

            ConfigureServices(builder.Services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Configuration.ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            
            services.AddLogging(loggingBuilder =>
            {
                var nLogConfiguration = new NLogConfiguration();

                loggingBuilder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });

                nLogConfiguration.ConfigureNLog(Configuration);
            });
        }
    }
}
