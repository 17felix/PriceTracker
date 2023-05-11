namespace CleanArchitecture.Domain.Common;
/// <summary>
/// Defines a type that can be soft-deleted or deactivated.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets or sets a value indicating whether or not this object has been soft-deleted.
    /// </summary>
    bool IsDeleted { get; set; }
}
