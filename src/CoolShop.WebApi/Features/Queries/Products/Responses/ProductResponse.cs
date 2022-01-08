using System;

namespace CoolShop.WebApi.Features.Queries.Products.Responses;

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Uri ImageUri { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}
