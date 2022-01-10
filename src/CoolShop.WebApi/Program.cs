using System;
using System.Threading.Tasks;
using CoolShop.WebApi.Data;
using CoolShop.WebApi.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoolShop.WebApi;

/// <summary>
/// Program entry
/// </summary>
public sealed class Program
{
    /// <summary>
    /// Main
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        await CreateDbIfNotExists(host);

        await host.RunAsync();
    }

    /// <summary>
    /// Create host builder
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

    private static async Task CreateDbIfNotExists(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<CoolShopContext>();
            await DbInitializer.InitializeAsync(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}
