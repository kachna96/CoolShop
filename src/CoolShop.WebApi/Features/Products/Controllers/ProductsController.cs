using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CoolShop.WebApi.Features.Products.Commands;
using CoolShop.WebApi.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CoolShop.WebApi.Features.Products.Queries.GetProductCollectionV1;
using static CoolShop.WebApi.Features.Products.Queries.GetProductCollectionV2;

namespace CoolShop.WebApi.Features.Products.Controllers;

/// <summary>
/// Controller for products related actions
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mediator"></param>
    public ProductsController(IMediator mediator)
    {
        Guard.Against.Null(mediator, nameof(mediator));

        _mediator = mediator;
    }

    /// <summary>
    /// Update single product description
    /// </summary>
    /// <param name="id">Id of a product to update</param>
    /// <param name="requestCommand">New product values</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(GetProductById.Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status404NotFound)]
    public async Task PutProductById([FromRoute] int id, [FromBody] PutProductById.RequestCommand requestCommand, CancellationToken cancellationToken)
    {
        var command = new PutProductById.Command(id, requestCommand?.Description);
        var result = await _mediator.Send(command, cancellationToken);
        await result.ExecuteAsync(HttpContext);
    }

    /// <summary>
    /// Get single Product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(GetProductById.Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProductIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetProductById.Query(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Get Product collection
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet("")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(ProductCollectionResponseV1), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductCollectionAsync(CancellationToken cancellationToken)
    {
        var query = new GetProductCollectionV1.Query();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get paged Product collection
    /// </summary>
    /// <param name="page">Page to fetch</param>
    /// <param name="take">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet("")]
    [MapToApiVersion("2.0")]
    [ProducesResponseType(typeof(ProductCollectionResponseV2), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductCollectionV2Async(CancellationToken cancellationToken, [FromQuery] int page = 1, [FromQuery] int take = 10)
    {
        var query = new GetProductCollectionV2.Query(page, take);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
