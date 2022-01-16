using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoolShop.WebApi.Features.Products.Queries;
using CoolShop.WebApi.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.IntegrationTests.Features.Products;

[TestClass]
public class GetProductByIdTest : CustomWebApplicationFactory<Startup>
{
    [TestMethod]
    public async Task GetProductByIdTest_WithMissingProduct()
    {
        var client = CreateCoolShopClient();

        var response = await client.GetAsync("/api/v1/products/9");

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task GetProductByIdTest_WithExistingProduct()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var response = await client.GetFromJsonAsync<GetProductById.Response>("/api/v1/products/9");

        Assert.IsNotNull(response);
        Assert.AreEqual(Products.Skip(8).First().Name, response.Name);
    }

    [TestMethod]
    public async Task GetProductByIdTest_WithNegativeId_ProducesError()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var ex = await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await client.GetFromJsonAsync<GetProductById.Response>("/api/v1/products/-1"));
        Assert.AreEqual(ex.Message, "Response status code does not indicate success: 400 (Bad Request).");
    }
}
