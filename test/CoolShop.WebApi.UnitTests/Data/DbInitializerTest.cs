using System.Threading.Tasks;
using CoolShop.WebApi.Data;
using CoolShop.WebApi.UnitTests.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.UnitTests.Data;

[TestClass]
public class DbInitializerTest : InMemoryTestBase
{
    [TestMethod]
    public async Task DbInitializer_WithInMemoryContext_InitializesData()
    {
        await DbInitializer.InitializeAsync(Context);

        Assert.AreNotEqual(0, await Context.Products.ToListAsync());
    }
}
