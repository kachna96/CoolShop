using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using static CoolShop.WebApi.Features.Queries.Products.GetProductCollection;

namespace CoolShop.WebApi.Features.Queries.Products.Mappers;

public class ProductCollectionMapper : Profile
{
    public ProductCollectionMapper()
    {
        CreateMap<IEnumerable<Product>, ProductCollectionResponse>()
            .ForMember(x => x.Count, x => x.MapFrom(source => source.Count()))
            .ForMember(x => x.ProductCollection, x => x.MapFrom(source => source));
    }
}
