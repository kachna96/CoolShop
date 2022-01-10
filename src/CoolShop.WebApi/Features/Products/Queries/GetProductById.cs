using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using MediatR;

namespace CoolShop.WebApi.Features.Products.Queries;

/// <summary>
/// GetProductById operations
/// </summary>
public sealed class GetProductById
{
    /// <summary>
    /// Request object
    /// </summary>
    [SuppressMessage("Naming", "CA1724: Type names should not match namespaces", Justification = "Generic query object")]
    public record Query : IRequest<Response>
    {
        /// <summary>
        /// Id of a product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Product ID</param>
        public Query(int id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Single product definition
    /// </summary>
    public record Response
    {
        /// <summary>
        /// Id of a product
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Name of a product
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Product image link
        /// </summary>
        [Required]
        public Uri ImageUri { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Handler
    /// </summary>
    public class Handler : IRequestHandler<Query, Response>
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
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var product = await _context
                .Products
                .FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            return _mapper.Map<Response>(product);
        }
    }
}
