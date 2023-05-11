namespace CleanArchitecture.Domain.Common;

/// <summary>
/// Defines a type with audit timestamps.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Gets or sets the date and time when this entity was created.
    /// </summary>
    DateTimeOffset Created { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this entity was last modified.
    /// </summary>
    DateTimeOffset Modified { get; set; }
}