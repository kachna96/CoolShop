using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using CoolShop.WebApi.Features.Products.Queries;

namespace CoolShop.WebApi.Features.Products.Mappers;

/// <summary>
/// Mapper for a single Product
/// </summary>
public class ProductMapper : Profile
{
    /// <summary>
    /// Create map
    /// </summary>
    public ProductMapper()
    {
        CreateMap<Product, GetProductById.Response>();
    }
}
