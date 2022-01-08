using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static CoolShop.WebApi.Features.Queries.Products.GetProductById;
using static CoolShop.WebApi.Features.Queries.Products.GetProductCollection;

namespace CoolShop.WebApi.Features.Queries.Products.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a single Product by its ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByProductIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetProductById.Query(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Gets a Product collection
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token</param>
    /// <returns></returns>
    [HttpGet("")]
    [ProducesResponseType(typeof(ProductCollectionResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductCollectionAsync(CancellationToken cancellationToken)
    {
        var query = new GetProductCollection.Query();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
