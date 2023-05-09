namespace CleanArchitecture.Domain.Common;
/// <summary>
/// Defines a type that can be uniquely identified by a property.
/// </summary>
/// <typeparam name="TIdentifierType">Type of the identifier.</typeparam>
public interface IIdentifiable<TIdentifierType>
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    TIdentifierType Id { get; set; }
}
