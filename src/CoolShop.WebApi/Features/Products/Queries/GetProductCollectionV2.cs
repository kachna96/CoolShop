﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoolShop.WebApi.Features.Products.Queries;

public sealed class GetProductCollectionV2
{
    /// <summary>
    /// Request object
    /// </summary>
    [SuppressMessage("Naming", "CA1724: Type names should not match namespaces", Justification = "Generic query object")]
    public record Query : IRequest<ProductCollectionResponseV2>
    {
        /// <summary>
        /// Id of a product
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Id of a product
        /// </summary>
        public int Take { get; set; }

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
    public class Handler : IRequestHandler<Query, ProductCollectionResponseV2>
    {
        private readonly CoolShopContext _context;
        private readonly IMapper _mapper;

        public Handler(CoolShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc/>
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
                TotalCount = count,
                ProductCollection = _mapper.Map<IEnumerable<GetProductById.Response>>(products)
            };
        }
    }
}