using CleanArchitecture.Domain.Entities.Products;

namespace CleanArchitecture.Domain.Events;

public class ProductCleanedEvent : BaseEvent
{
    public ProductCleanedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}