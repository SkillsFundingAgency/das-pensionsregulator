﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PensionsRegulatorApi.Security;

public static class SecurityServicesCollectionExtensions
{
    public static void AddActiveDirectoryAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var activeDirectoryConfig = configuration.GetSection("ActiveDirectory").Get<ActiveDirectoryConfiguration>();

        services.AddAuthorizationBuilder()
            .AddPolicy("default", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Default");
            });
        
        services.AddAuthentication(auth =>
        {
            auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(auth =>
        {
            auth.Authority = $"https://login.microsoftonline.com/{activeDirectoryConfig.Tenant}";
            auth.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidAudiences = activeDirectoryConfig.IdentifierUri.Split(",")
            };
        });
    }
}