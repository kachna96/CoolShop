using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoolShop.WebApi.Features.Commands.Commands;
using CoolShop.WebApi.Features.Queries.Products.Responses;
using CoolShop.WebApi.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.IntegrationTests.Features.Products;

[TestClass]
public class ProductTest : CustomWebApplicationFactory<Startup>
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

        var response = await client.GetFromJsonAsync<ProductResponse>("/api/v1/products/9");

        Assert.IsNotNull(response);
        Assert.AreEqual(Products.Skip(8).First().Name, response.Name);
    }

    [TestMethod]
    public async Task PutProductByIdTest_WithMissingProduct()
    {
        var client = CreateCoolShopClient();

        var data = new PutProductById.RequestCommand("New product desciption");
        var response = await client.PutAsJsonAsync("/api/v1/products/9", data);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task PutProductByIdTest_WithShortDesciption()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var response = await client.PutAsJsonAsync("/api/v1/products/9", new PutProductById.RequestCommand("New"));

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task PutProductByIdTest_WithDesciption()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var description = "New product description";
        var response = await client.PutAsJsonAsync("/api/v1/products/9", new PutProductById.RequestCommand(description));

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ProductResponse>();

        Assert.IsNotNull(content);
        Assert.AreEqual(description, content.Description);
    }
}
