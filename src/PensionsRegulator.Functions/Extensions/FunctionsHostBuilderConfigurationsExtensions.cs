using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PensionsRegulator.Functions
{
    internal static class FunctionsHostBuilderConfigurationsExtensions
    {

        public static IFunctionsHostBuilder AddConfiguration(this IFunctionsHostBuilder builder, Func<IConfigurationBuilder, IConfiguration> configBuilderFunc)
        {
            var configurationBuilder = builder.GetBaseConfigurationBuilder();

            var configuration = configBuilderFunc(configurationBuilder);

            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), configuration));

            return builder;
        }

        public static IConfiguration GetCurrentConfiguration(this IFunctionsHostBuilder builder)
        {
            var provider = builder.Services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();
            return configuration;
        }

        private static IConfigurationBuilder GetBaseConfigurationBuilder(this IFunctionsHostBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();

            var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (descriptor?.ImplementationInstance is IConfiguration configRoot)
            {
                configurationBuilder.AddConfiguration(configRoot);
            }

            var rootConfigurationBuilder = configurationBuilder.SetBasePath(GetCurrentDirectory());

            return rootConfigurationBuilder;
        }

        private static string GetCurrentDirectory()
        {
            var currentDirectory = "/home/site/wwwroot";
            var isLocal = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));
            if (isLocal)
            {
                currentDirectory = Environment.CurrentDirectory;
            }
            return currentDirectory;
        }
    }
}
