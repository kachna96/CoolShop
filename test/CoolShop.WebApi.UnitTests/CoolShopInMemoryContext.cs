using CoolShop.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoolShop.WebApi.UnitTests;

internal static class CoolShopInMemoryContext
{
    public static CoolShopContext Create()
    {
        var options = new DbContextOptionsBuilder<CoolShopContext>()
            .UseInMemoryDatabase(nameof(CoolShopContext))
            .Options;

        return new CoolShopContext(options);
    }
}
