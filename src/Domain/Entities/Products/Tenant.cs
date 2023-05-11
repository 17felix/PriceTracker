namespace CleanArchitecture.Domain.Entities.Products;

public class Tenant : BaseAuditableEntity
{    /// <summary>
     /// An exact shop with unique URL.
     /// </summary>
     public string Name { get; set; }
    public Country Country { get; set; }
    public string Url { get; set; }
    public Contacts Contacts { get; set; }
}
