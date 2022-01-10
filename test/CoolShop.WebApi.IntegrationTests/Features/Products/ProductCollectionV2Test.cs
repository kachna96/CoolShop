using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoolShop.WebApi.Features.Products.Queries;
using CoolShop.WebApi.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.IntegrationTests.Features.Products;

[TestClass]
public class ProductCollectionV2Test : CustomWebApplicationFactory<Startup>
{
    [TestMethod]
    public async Task GetProductCollectionV2Test_WithEmptyDatabase()
    {
        var client = CreateCoolShopClient();

        var response = await client.GetFromJsonAsync<GetProductCollectionV2.ProductCollectionResponseV2>("/api/v2/products");

        Assert.IsNotNull(response);
        Assert.AreEqual(0, response.TotalCount);
        Assert.AreEqual(1, response.Page);
        Assert.IsFalse(response.ProductCollection.Any());
    }

    [TestMethod]
    public async Task GetProductCollectionV2Test_WithFilledDatabase()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var response = await client.GetFromJsonAsync<GetProductCollectionV2.ProductCollectionResponseV2>("/api/v2/products");

        Assert.IsNotNull(response);
        Assert.AreEqual(20, response.TotalCount);
        Assert.AreEqual(1, response.Page);
        Assert.AreEqual(10, response.ProductCollection.Count());
    }

    [TestMethod]
    public async Task GetProductCollectionV2Test_WithFilledDatabase_Paged()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var response = await client.GetFromJsonAsync<GetProductCollectionV2.ProductCollectionResponseV2>("/api/v2/products?page=3&take=5");

        Assert.IsNotNull(response);
        Assert.AreEqual(20, response.TotalCount);
        Assert.AreEqual(3, response.Page);
        Assert.AreEqual(5, response.ProductCollection.Count());
        Assert.AreEqual(Products.Skip(10).First().Name, response.ProductCollection.First().Name);
    }
}
