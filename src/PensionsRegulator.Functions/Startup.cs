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
using PensionsRegulator.Functions.Application.Services;
using PensionsRegulator.Functions.Domain.Data;
using PensionsRegulator.Functions.Domain.Services;
using PensionsRegulator.Functions.Infrastructure.Configuration;
using PensionsRegulator.Functions.Infrastructure.Data;
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
                var tempConfig = configBuilder
                    .Build();

                var configuration = configBuilder
                    //.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddAzureTableStorageConfiguration(
                        tempConfig["ConfigurationStorageConnectionString"],
                        tempConfig["ConfigNames"],
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

            services.AddTransient<IPensionRegulatorImportService, PensionRegulatorImportService>();
            services.AddTransient<IPensionRegulatorRepository>(s => new PensionRegulatorRepository(Configuration.GetValue<string>("PensionsRegulatorSQLConnectionString"),s.GetRequiredService<ILogger>()));

        }
    }
}
