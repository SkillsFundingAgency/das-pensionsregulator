using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;

namespace PensionsRegulatorApi.StartupConfiguration;

public static class DatabaseExtensions
{
    private const string AzureResource = "https://database.windows.net/";
    // Take advantage of the builtin caching provided by AzureServiceTokenProvider. 
    private static readonly AzureServiceTokenProvider AzureServiceTokenProvider = new();

    public static void AddDatabaseRegistration(this IServiceCollection services, string environment, string connectionString)
    {
        services.AddTransient<IDbConnection>(_ => environment.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
            ? new SqlConnection(connectionString)
            : new SqlConnection
            {
                ConnectionString = connectionString,
                AccessToken = AzureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result
            });
    }
}