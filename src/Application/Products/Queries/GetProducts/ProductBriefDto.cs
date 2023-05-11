using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities.Products;

namespace CleanArchitecture.Application.Products.Queries.GetProducts;
public class ProductBriefDto : IMapFrom<Product>
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public double Weight { get; init; }
}
