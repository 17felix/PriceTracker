using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Events;
using CleanArchitecture.Domain.Entities.Products;
using MediatR;

namespace CleanArchitecture.Application.Products.Commands;

public record CreateProductCommand : IRequest<long>
{
    public decimal Price { get; init; }
    public string? Title { get; init; }
    public double? Weight { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            Price = request.Price,
            Title = request.Title ?? "",
            Weight = request.Weight ?? 0,
        };

        entity.AddDomainEvent(new ProductCreatedEvent(entity));

        _context.Products.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
