using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Configuration;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Security;
using SFA.DAS.Configuration.AzureTableStorage;

namespace PensionsRegulatorApi.StartupConfiguration;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        _environment = environment;

        var config = new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddUserSecrets<Startup>();

        config.AddAzureTableStorage(options =>
        {
            options.ConfigurationKeys = new[] { "SFA.DAS.PensionsRegulatorApi" };
            options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
            options.EnvironmentName = configuration["EnvironmentName"];
            options.PreFixConfigurationKeys = false;
        });

        _configuration = config.Build();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddADAuthentication(_configuration);
        services.AddControllers(options =>
        {
            if (!_environment.IsDevelopment())
            {
                options.Filters.Add(new AuthorizeFilter("default"));
            }
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pensions-Regulator-Api", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetOrganisationsByPayeRef>());
        services.AddTransient<IOrganisationRepository, SqlOrganisationRepository>();

        services.AddDatabaseRegistration(_configuration["EnvironmentName"],
            _configuration
                .GetSection("ConnectionStrings")
                .Get<ConnectionStrings>().PensionsRegulatorSql);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
                
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pensions Regulator Api V1");
            c.RoutePrefix = string.Empty;
        });
    }
}