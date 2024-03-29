﻿using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PensionsRegulatorApi.StartupConfiguration
{
    public static class DatabaseExtensions
    {
        private const string AzureResource = "https://database.windows.net/";

        public static void AddDatabaseRegistration(this IServiceCollection services, string environment, string connectionString)
        {
            services.AddTransient<IDbConnection>(c => {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                return environment.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
                    ? new SqlConnection(connectionString)
                    : new SqlConnection
                    {
                        ConnectionString = connectionString,
                        AccessToken = azureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result
                    };
            });
        }
    }
}
