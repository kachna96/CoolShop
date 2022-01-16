using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CoolShop.WebApi.Features.Products.Queries;

/// <summary>
/// GetProductCollectionV2 operations
/// </summary>
public sealed class GetProductCollectionV2
{
    /// <summary>
    /// Request object
    /// </summary>
    [SuppressMessage("Naming", "CA1724: Type names should not match namespaces", Justification = "Generic query object")]
    public record Query : IRequest<IResult>
    {
        /// <summary>
        /// Id of a product
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Id of a product
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">Page to query</param>
        /// <param name="take">Number of items on page</param>
        public Query(int page, int take)
        {
            Page = page;
            Take = take;
        }
    }

    /// <summary>
    /// Collection of paged products
    /// </summary>
    public class ProductCollectionResponseV2
    {
        /// <summary>
        /// Total number of products
        /// </summary>
        [Required]
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page
        /// </summary>
        [Required]
        public int Page { get; set; }

        /// <summary>
        /// Collection of products on current page
        /// </summary>
        [Required]
        public IEnumerable<GetProductById.Response> ProductCollection { get; set; }
    }

    /// <summary>
    /// Handler
    /// </summary>
    public class Handler : IRequestHandler<Query, IResult>
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
        public async Task<IResult> Handle(Query request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var count = await _context.Products.CountAsync(cancellationToken);
            var products = await _context
                .Products
                .Skip((request.Page - 1) * request.Take)
                .Take(request.Take)
                .ToListAsync(cancellationToken);

            return Results.Ok(new ProductCollectionResponseV2
            {
                Page = request.Page,
                TotalCount = count,
                ProductCollection = _mapper.Map<IEnumerable<GetProductById.Response>>(products)
            });
        }
    }

    /// <summary>
    /// Request validator
    /// </summary>
    public class GetProductCollectionV2Validator : AbstractValidator<Query>
    {
        /// <summary>
        /// Product validator rules
        /// </summary>
        public GetProductCollectionV2Validator()
        {
            RuleFor(x => x.Take)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Page)
                .GreaterThan(0);
        }
    }
}
