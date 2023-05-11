using System.Text.RegularExpressions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Products;
using CleanArchitecture.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Products.Commands;

public record CleanProductsCommand : IRequest<int>
{
    public int Id { get; init; }
}

public class CleanProductsCommandHandler : IRequestHandler<CleanProductsCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CleanProductsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CleanProductsCommand request, CancellationToken cancellationToken)
    {
        var entity = await GetAsync(request.Id, cancellationToken);

        var cleanEntity = new Product
        {
            Price = 0,
            Title = entity.Title ?? "",
            Weight = MapWeight(entity.Weight.ToString()),
        };

        cleanEntity.AddDomainEvent(new ProductCleanedEvent(cleanEntity));

        await InsertAsync(cleanEntity, cancellationToken);
        return cleanEntity.Id;
    }

    private double MapWeight(string source)
    {
        // Use regex to extract the weight in grams or kilograms
        var regex = new Regex(@"(\d+(\.|,)\d+|\d+)\s*(г|кг|мл|л)", RegexOptions.IgnoreCase);
        var match = regex.Match(source);
        if (match.Success)
        {
            var weight = double.Parse(match.Groups[1].Value);
            var unit = match.Groups[3].Value.ToLowerInvariant();
            if (unit == "г" || unit == "мл")
            {
                weight /= 1000.0; // convert to kilo(1000)
            }

            return weight;
        }
        
        return 0;
    }

    public async Task<Product> GetAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Products.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Product> InsertAsync(Product newEntity, CancellationToken cancellationToken)
    {
        _context.Products.Add(newEntity);
        
        await _context.SaveChangesAsync(cancellationToken);

        return newEntity;
    }
}
