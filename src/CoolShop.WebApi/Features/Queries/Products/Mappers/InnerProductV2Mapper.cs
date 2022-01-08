using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using static CoolShop.WebApi.Features.Queries.Products.GetProductCollectionV2;

namespace CoolShop.WebApi.Features.Queries.Products.Mappers;

public class InnerProductV2Mapper : Profile
{
    public InnerProductV2Mapper()
    {
        CreateMap<Product, InnerProductV2>();
    }
}
