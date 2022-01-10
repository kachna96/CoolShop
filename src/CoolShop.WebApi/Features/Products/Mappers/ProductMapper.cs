using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using CoolShop.WebApi.Features.Products.Queries;

namespace CoolShop.WebApi.Features.Products.Mappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, GetProductById.Response>();
    }
}
