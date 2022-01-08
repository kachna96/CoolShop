using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using MediatR;

namespace CoolShop.WebApi.Features.Queries.Products;

public class GetProductById
{
    public record Query(int Id) : IRequest<ProductResponse>;

    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImageUri { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class Handler : IRequestHandler<Query, ProductResponse>
    {
        private readonly CoolShopContext _context;
        private readonly IMapper _mapper;

        public Handler(CoolShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var product = await _context
                .Products
                .FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            return _mapper.Map<ProductResponse>(product);
        }
    }
}
