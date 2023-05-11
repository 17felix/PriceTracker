using System.ComponentModel.DataAnnotations.Schema;

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
    [Column(TypeName = "decimal(10,4)")]
    public decimal Price { get; set; }
    public double Weight { get; set; }
    public int? Discount { get; set; }
    [Column(TypeName = "decimal(10,4)")]
    public decimal DiscountPrice { get; set; }
    public int Review { get; set; }
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; }
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; }
}

