using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using static CoolShop.WebApi.Features.Queries.Products.GetProductCollection;

namespace CoolShop.WebApi.Features.Queries.Products.Mappers;

public class InnerProductMapper : Profile
{
    public InnerProductMapper()
    {
        CreateMap<Product, InnerProduct>();
    }
}
