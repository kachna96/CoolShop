using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using CoolShop.WebApi.Extensions;
using CoolShop.WebApi.Features.Products.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CoolShop.WebApi.Features.Products.Commands;

/// <summary>
/// PutProduct operations
/// </summary>
public sealed class PutProductById
{
    /// <summary>
    /// Request object
    /// </summary>
    public record RequestCommand
    {
        /// <summary>
        /// New product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="description">New product description</param>
        public RequestCommand(string description)
        {
            Description = description;
        }
    }

    /// <summary>
    /// Request object with Id
    /// </summary>
    public record Command : IRequest<IResult>
    {
        /// <summary>
        /// Product id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// New product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="description">New product description</param>
        public Command(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }

    /// <summary>
    /// Handler
    /// </summary>
    public class Handler : IRequestHandler<Command, IResult>
    {
        private readonly CoolShopContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<Command> _validator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="validator">Request validator</param>
        public Handler(CoolShopContext context, IMapper mapper, IValidator<Command> validator)
        {
            Guard.Against.Null(context, nameof(context));
            Guard.Against.Null(mapper, nameof(mapper));
            Guard.Against.Null(validator, nameof(validator));

            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        /// <inheritdoc/>
        public async Task<IResult> Handle(Command request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.GetValidationErrors());
            }

            var product = await _context
                .Products
                .FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            if (product is null)
            {
                return Results.NotFound();
            }

            product.Description = request.Description;

            _context.Products.Update(product);

            await _context.SaveChangesAsync(cancellationToken);

            return Results.Ok(_mapper.Map<GetProductById.Response>(product));
        }
    }

    /// <summary>
    /// Request validator
    /// </summary>
    public class PutProductByIdValidator : AbstractValidator<Command>
    {
        /// <summary>
        /// Product validator rules
        /// </summary>
        public PutProductByIdValidator()
        {
            RuleFor(r => r.Id)
                .GreaterThanOrEqualTo(0);
            RuleFor(r => r.Description)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(10000);
        }
    }
}
