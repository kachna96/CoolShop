using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoolShop.WebApi.Features.Products.Commands;
using CoolShop.WebApi.Features.Products.Queries;
using CoolShop.WebApi.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.IntegrationTests.Features.Products;

[TestClass]
public class PutProductByIdTest : CustomWebApplicationFactory<Startup>
{
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

        var content = await response.Content.ReadFromJsonAsync<GetProductById.Response>();

        Assert.IsNotNull(content);
        Assert.AreEqual(description, content.Description);
    }
}
