namespace CleanArchitecture.Domain.Entities.Products;
public class DirtyProduct : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Price { get; set; }
    public string Weight { get; set; }
    public string Discount { get; set; }
    public string DiscountPrice { get; set; }
    public string? Review { get; set; }
    public string SupplierId { get; set; }
    public string TenantId { get; set; }
}

