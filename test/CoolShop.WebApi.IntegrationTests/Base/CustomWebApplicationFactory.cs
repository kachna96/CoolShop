using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoBogus;
using CoolShop.WebApi.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.IntegrationTests.Base;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected IEnumerable<Product> Products { get; set; } = new List<Product>();

    [TestInitialize]
    public void Initialize()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CoolShopContext>();
        context.Database.EnsureCreated();
    }

    [TestCleanup]
    public void Cleanup()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CoolShopContext>();
        context.Database.EnsureDeleted();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<CoolShopContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<CoolShopContext>(options =>
            {
                options.UseInMemoryDatabase(nameof(CoolShopContext));
                // don't raise the error warning us that the in memory db doesn't support transactions
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        });
    }

    protected HttpClient CreateCoolShopClient()
    {
        var client = CreateClient();
        ConfigureClient(client);

        return client;
    }

    protected override void ConfigureClient(HttpClient? client)
    {
        if (client is null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        client.BaseAddress = new Uri("https://localhost");
    }

    protected async Task SeedDatabase()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CoolShopContext>();

        Products = new AutoFaker<Product>()
            .Ignore(x => x.Id)
            .Generate(count: 20);

        context.Products.AddRange(Products);

        await context.SaveChangesAsync();
    }
}
