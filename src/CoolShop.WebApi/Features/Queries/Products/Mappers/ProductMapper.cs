using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using static CoolShop.WebApi.Features.Queries.Products.GetProductById;

namespace CoolShop.WebApi.Features.Queries.Products.Mappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductResponse>();
    }
}
