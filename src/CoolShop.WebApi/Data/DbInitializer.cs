using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoBogus;
using CoolShop.WebApi.Domain.Entities;

namespace CoolShop.WebApi.Data;

public static class DbInitializer
{
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
