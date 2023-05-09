namespace CleanArchitecture.Domain.Entities.Products;
public class Product : BaseAuditableEntity
{
    /// <summary>
    ///      Provides products for store
    /// </summary>
    /// <param name="Supplier">Provides products for store.</param>
    /// <param name="Tenant">An exact shop with unique URL.</param>
    /// <returns></returns>
    public string Title { get; set; }
    public decimal Price { get; set; }
    public double Weight { get; set; }
    public int Discount { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal? Review { get; set; }
    public long SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public long TenantId { get; set; }
    public Tenant Tenant { get; set; }
}

