using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoolShop.WebApi.Features.Products.Queries;
using CoolShop.WebApi.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.IntegrationTests.Features.Products;

[TestClass]
public class ProductCollectionTest : CustomWebApplicationFactory<Startup>
{
    [TestMethod]
    public async Task GetProductCollectionTest_WithEmptyDatabase()
    {
        var client = CreateCoolShopClient();

        var response = await client.GetFromJsonAsync<GetProductCollectionV1.ProductCollectionResponseV1>("/api/v1/products");

        Assert.IsNotNull(response);
        Assert.AreEqual(0, response.Count);
        Assert.IsFalse(response.ProductCollection.Any());
    }

    [TestMethod]
    public async Task GetProductCollectionTest_WithFilledDatabase()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var response = await client.GetFromJsonAsync<GetProductCollectionV1.ProductCollectionResponseV1>("/api/v1/products");

        Assert.IsNotNull(response);
        Assert.AreEqual(20, response.Count);
        Assert.AreEqual(20, response.ProductCollection.Count());
    }
}
