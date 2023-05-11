using CleanArchitecture.Domain.Entities.Products;

namespace CleanArchitecture.Domain.Events;

public class ProductDeletedEvent : BaseEvent
{
    public ProductDeletedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}
