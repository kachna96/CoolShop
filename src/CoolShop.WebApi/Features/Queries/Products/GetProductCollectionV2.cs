using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolShop.WebApi.Features.Queries.Products;

public class GetProductCollectionV2
{
    public record Query(int Page, int Take) : IRequest<ProductCollectionResponseV2>;

    public class ProductCollectionResponseV2
    {
        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<InnerProductV2> ProductCollection { get; set; }
    }

    public class InnerProductV2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImageUri { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class Handler : IRequestHandler<Query, ProductCollectionResponseV2>
    {
        private readonly CoolShopContext _context;
        private readonly IMapper _mapper;

        public Handler(CoolShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductCollectionResponseV2> Handle(Query request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var count = await _context.Products.CountAsync(cancellationToken);
            var products = await _context
                .Products
                .Skip((request.Page - 1) * request.Take)
                .Take(request.Take)
                .ToListAsync(cancellationToken);

            return new ProductCollectionResponseV2
            {
                Page = request.Page,
                PageCount = request.Take,
                TotalCount = count,
                ProductCollection = _mapper.Map<IEnumerable<InnerProductV2>>(products)
            };
        }
    }
}
