using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolShop.WebApi.Features.Products.Queries;

/// <summary>
/// GetProductCollectionV1 operations
/// </summary>
public sealed class GetProductCollectionV1
{
    /// <summary>
    /// Request object
    /// </summary>
    [SuppressMessage("Naming", "CA1724: Type names should not match namespaces", Justification = "Generic query object")]
    public record Query() : IRequest<ProductCollectionResponseV1>;

    /// <summary>
    /// Collection of all products
    /// </summary>
    public record ProductCollectionResponseV1
    {
        /// <summary>
        /// Number of returned products
        /// </summary>
        [Required]
        public int Count { get; set; }

        /// <summary>
        /// Collection of products
        /// </summary>
        [Required]
        public IEnumerable<GetProductById.Response> ProductCollection { get; set; }
    }

    /// <summary>
    /// Handler
    /// </summary>
    public class Handler : IRequestHandler<Query, ProductCollectionResponseV1>
    {
        private readonly CoolShopContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Mapper</param>
        public Handler(CoolShopContext context, IMapper mapper)
        {
            Guard.Against.Null(context, nameof(context));
            Guard.Against.Null(mapper, nameof(mapper));

            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<ProductCollectionResponseV1> Handle(Query request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var products = await _context
                .Products
                .ToListAsync(cancellationToken);

            return _mapper.Map<ProductCollectionResponseV1>(products);
        }
    }
}
