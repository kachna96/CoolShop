using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CoolShop.WebApi.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CoolShop.WebApi.Behaviors;

/// <inheritdoc/>
public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly CoolShopContext _context;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context">DbContext</param>
    /// <param name="logger">Logger</param>
    public TransactionBehavior(CoolShopContext context, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(logger, nameof(logger));

        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Guard.Against.Null(next, nameof(next));

        try
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
            var response = await next();
            await _context.Database.CommitTransactionAsync(cancellationToken);

            return response;
        }
        catch (Exception)
        {
            _logger.LogError("Request failed: Rolling back all the changes made to the Context");

            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }
}
