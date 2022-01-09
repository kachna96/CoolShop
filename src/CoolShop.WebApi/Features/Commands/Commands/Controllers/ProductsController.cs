using System.Threading;
using System.Threading.Tasks;
using CoolShop.WebApi.Features.Queries.Products.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoolShop.WebApi.Features.Commands.Commands.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status404NotFound)]
    public async Task PutProductById([FromRoute] int id, [FromBody] PutProductById.RequestCommand requestCommand, CancellationToken cancellationToken)
    {
        var command = new PutProductById.Command(id, requestCommand?.Description);
        var result = await _mediator.Send(command, cancellationToken);
        await result.ExecuteAsync(HttpContext);
    }
}
