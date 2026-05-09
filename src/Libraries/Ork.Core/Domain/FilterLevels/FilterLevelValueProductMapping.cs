namespace Ork.Core.Domain.FilterLevels;

/// <summary>
/// Represents a filter level value and product mapping
/// </summary>
public sealed class FilterLevelValueProductMapping : BaseEntity
{
    /// <summary>
    /// Gets or sets the filter level value identifier
    /// </summary>
    public int FilterLevelValueId { get; set; }

    /// <summary>
    /// Gets or sets the product identifier
    /// </summary>
    public int ProductId { get; set; }
}
