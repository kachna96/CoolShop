using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolShop.WebApi.Features.Queries.Products;

public class GetProductCollection
{
    public record Query() : IRequest<ProductCollectionResponse>;

    public class ProductCollectionResponse
    {
        public int Count { get; set; }

        public IEnumerable<InnerProduct> ProductCollection { get; set; }
    }

    public class InnerProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImageUri { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class Handler : IRequestHandler<Query, ProductCollectionResponse>
    {
        private readonly CoolShopContext _context;
        private readonly IMapper _mapper;

        public Handler(CoolShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductCollectionResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var products = await _context
                .Products
                .ToListAsync(cancellationToken);

            return _mapper.Map<ProductCollectionResponse>(products);
        }
    }
}
