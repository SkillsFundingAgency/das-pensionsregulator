using Microsoft.Extensions.Configuration;

namespace PensionsRegulator.Functions.Infrastructure.Configuration
{
    public static class AzureTableStorageConfigurationExtensions
    {
        public static IConfigurationBuilder AddAzureTableStorageConfiguration(this IConfigurationBuilder builder, string connection, string[] configNames, string environment, string version)
        {
            return builder.Add(new AzureTableStorageConfigurationSource(connection, configNames, environment, version));
        }
    }
}