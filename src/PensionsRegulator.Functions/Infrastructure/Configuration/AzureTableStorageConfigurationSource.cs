using Microsoft.Extensions.Configuration;

namespace PensionsRegulator.Functions.Infrastructure.Configuration
{
    public class AzureTableStorageConfigurationSource : IConfigurationSource
    {
        private readonly string _connection;
        private readonly string[] _configNames;
        private readonly string _environment;
        private readonly string _version;

        public AzureTableStorageConfigurationSource(string connection, string[] configNames, string environment, string version)
        {
            _connection = connection;
            _configNames = configNames;
            _environment = environment;
            _version = version;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AzureTableStorageConfigurationProvider(_connection, _configNames, _environment, _version);
        }
    }
}