using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using PensionsRegulator.Functions.Domain.Configuration;

namespace PensionsRegulator.Functions.Infrastructure.Configuration
{
    public class AzureTableStorageConfigurationProvider : ConfigurationProvider
    {
        private readonly string _environment;
        private readonly string _version;
        private readonly CloudStorageAccount _storageAccount;
        private readonly string _appName;

        public AzureTableStorageConfigurationProvider(string connection,string appName, string environment, string version)
        {
            _environment = environment;
            _version = version;
            _storageAccount = CloudStorageAccount.Parse(connection);
            _appName = appName;
        }
        public override void Load()
        {
            var table = GetTable();
            var operation = GetOperation(_appName, _environment, _version);
            var result = table.ExecuteAsync(operation).Result;

            var configItem = (ConfigurationItem)result.Result;

            using (var stream = configItem.Data.ToStream())
            {
                Data = JsonConfigurationParser.Parse(stream);
            }
        }

        private CloudTable GetTable()
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference("Configuration");
        }
        private TableOperation GetOperation(string serviceName, string environmentName, string version)
        {
            return TableOperation.Retrieve<ConfigurationItem>(environmentName, $"{serviceName}_{version}");
        }
    }
}