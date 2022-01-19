using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CoolShop.WebApi.Features.Products.Commands;
using CoolShop.WebApi.Features.Products.Queries;
using CoolShop.WebApi.IntegrationTests.Base;
using CoolShop.WebApi.IntegrationTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoolShop.WebApi.IntegrationTests.Features.Products;

[TestClass]
public class PatchProductByIdTest : CustomWebApplicationFactory<Startup>
{
    [TestMethod]
    public async Task PatchProductByIdTest_WithMissingProduct()
    {
        var client = CreateCoolShopClient();

        var data = new PatchProductById.RequestCommand("New product desciption");
        var response = await client.PatchAsJsonAsync("/api/v1/products/9", data);

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task PatchProductByIdTest_WithShortDesciption()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var response = await client.PatchAsJsonAsync("/api/v1/products/9", new PatchProductById.RequestCommand("New"));

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task PatchProductByIdTest_WithDesciption()
    {
        await SeedDatabase();
        var client = CreateCoolShopClient();

        var description = "New product description";
        var response = await client.PatchAsJsonAsync("/api/v1/products/9", new PatchProductById.RequestCommand(description));

        Assert.IsNotNull(response);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<GetProductById.Response>();

        Assert.IsNotNull(content);
        Assert.AreEqual(description, content.Description);
    }
}
