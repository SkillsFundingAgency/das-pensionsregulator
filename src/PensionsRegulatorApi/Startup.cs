using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using PensionsRegulatorApi.Application.Queries;
using PensionsRegulatorApi.Configuration;
using PensionsRegulatorApi.Data;
using PensionsRegulatorApi.Security;
using PensionsRegulatorApi.StartupConfiguration;

namespace PensionsRegulatorApi;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
        _environment = environment;
        _configuration = configuration.BuildDasConfiguration();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddActiveDirectoryAuthentication(_configuration);
        services.AddControllers(options =>
        {
            if (!_environment.IsDevelopment())
            {
                options.Filters.Add(new AuthorizeFilter("default"));
            }
        });
        
        services.AddLogging(builder =>
        {
            builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
            builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
        });

        services.AddDasSwagger();

        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetOrganisationsByPayeRef>());
        services.AddTransient<IOrganisationRepository, SqlOrganisationRepository>();

        services.AddDatabaseRegistration(_configuration["EnvironmentName"],
            _configuration
                .GetSection("ConnectionStrings")
                .Get<ConnectionStrings>().PensionsRegulatorSql);

        services.AddApplicationInsightsTelemetry();
    }

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