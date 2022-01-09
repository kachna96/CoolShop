using CoolShop.WebApi.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.UnitTests.Base;

public class InMemoryTestBase
{
    public CoolShopContext Context { get; private set; } = CoolShopInMemoryContext.Create();

    [TestInitialize]
    protected void Initialize()
    {
        Context = CoolShopInMemoryContext.Create();
        Context.Database.EnsureCreated();
    }

    [TestCleanup]
    protected void Cleanup()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
