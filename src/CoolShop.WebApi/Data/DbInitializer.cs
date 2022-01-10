using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoBogus;
using CoolShop.WebApi.Domain.Entities;

namespace CoolShop.WebApi.Data;

/// <summary>
/// Db static data related methods
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Create random data and insert them into database
    /// </summary>
    /// <param name="context">DbContext to use</param>
    /// <returns></returns>
    public static async Task InitializeAsync(CoolShopContext context)
    {
        Guard.Against.Null(context, nameof(context));

        context.Database.EnsureCreated();

        if (context.Products.Any())
        {
            return;
        }

        var products = new AutoFaker<Product>()
            .Ignore(x => x.Id)
            .Generate(count: 20);

        context.Products.AddRange(products);

        await context.SaveChangesAsync();
    }
}
