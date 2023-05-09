using System.Text.RegularExpressions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Products;
using CleanArchitecture.Domain.Events;
using MediatR;

namespace CleanArchitecture.Application.Products.Commands;

public record CleanProductsCommand : IRequest<long>
{
    public long Id { get; init; }
}

public class CleanProductsCommandHandler : IRequestHandler<CleanProductsCommand, long>
{
    private readonly IApplicationDbContext _context;
    private readonly IProductService _productService;
    private readonly IDirtyProductService _dirtyProductService;

    public CleanProductsCommandHandler(IApplicationDbContext context, IProductService productService, IDirtyProductService dirtyProductService)
    {
        _context = context;
        _productService = productService;
        _dirtyProductService = dirtyProductService;
    }

    public async Task<long> Handle(CleanProductsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dirtyProductService.GetAsync(request.Id, cancellationToken);

        var cleanEntity = new Product
        {
            Price = 0,
            Title = entity.Title ?? "",
            Weight = MapWeight(entity.Weight),
        };

        cleanEntity.AddDomainEvent(new ProductCleanedEvent(cleanEntity));

        await _productService.InsertAsync(cleanEntity, cancellationToken);
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
                weight /= 1000.0; // convert from grams to kilograms
            }

            return weight;
        }
        
        return 0;
    }
}
