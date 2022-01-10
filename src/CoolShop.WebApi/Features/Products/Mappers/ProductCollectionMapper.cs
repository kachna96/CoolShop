using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using static CoolShop.WebApi.Features.Products.Queries.GetProductCollectionV1;

namespace CoolShop.WebApi.Features.Products.Mappers;

public class ProductCollectionMapper : Profile
{
    public ProductCollectionMapper()
    {
        CreateMap<IEnumerable<Product>, ProductCollectionResponseV1>()
            .ForMember(x => x.Count, x => x.MapFrom(source => source.Count()))
            .ForMember(x => x.ProductCollection, x => x.MapFrom(source => source));
    }
}
