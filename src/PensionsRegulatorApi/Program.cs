﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;

namespace PensionsRegulatorApi;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = LogManager.LogFactory.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

        try
        {
            logger.Info("Starting up host");
            CreateWebHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            logger.Fatal(ex, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseNLog()
            .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());
}