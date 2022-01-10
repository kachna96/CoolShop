using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using static CoolShop.WebApi.Features.Products.Queries.GetProductCollectionV1;

namespace CoolShop.WebApi.Features.Products.Mappers;

/// <summary>
/// Mapper for ProductCollectionV1
/// </summary>
public class ProductCollectionV1Mapper : Profile
{
    /// <summary>
    /// Create map
    /// </summary>
    public ProductCollectionV1Mapper()
    {
        CreateMap<IEnumerable<Product>, ProductCollectionResponseV1>()
            .ForMember(x => x.Count, x => x.MapFrom(source => source.Count()))
            .ForMember(x => x.ProductCollection, x => x.MapFrom(source => source));
    }
}
