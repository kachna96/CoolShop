using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using CoolShop.WebApi.Features.Queries.Products.Responses;

namespace CoolShop.WebApi.Features.Queries.Products.Mappers;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductResponse>();
    }
}
