namespace CleanArchitecture.Domain.Entities.Products;
public class Supplier : BaseAuditableEntity
{
    /// <summary>
    ///      Provides products for store
    /// </summary>
    /// <param name="Name">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns></returns>

    public string Name { get; set; }
    public Country Country { get; set; }
    public string Website { get; set; }
    public Contacts Contacts { get; set; }
}
