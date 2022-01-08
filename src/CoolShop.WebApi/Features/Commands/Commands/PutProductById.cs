using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using CoolShop.WebApi.Domain.Entities;
using CoolShop.WebApi.Extensions;
using CoolShop.WebApi.Features.Queries.Products.Responses;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CoolShop.WebApi.Features.Commands.Commands;

public class PutProductById
{
    public record RequestCommand(string Description);

    public record Command(int Id, string Description) : IRequest<IResult>;

    public class Handler : IRequestHandler<Command, IResult>
    {
        private readonly CoolShopContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<Command> _validator;

        public Handler(CoolShopContext context, IMapper mapper, IValidator<Command> validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

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

            return Results.Ok(_mapper.Map<ProductResponse>(product));
        }
    }

    public class PutProductByIdValidator : AbstractValidator<Command>
    {
        public PutProductByIdValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Description)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(10000);
        }
    }
}
