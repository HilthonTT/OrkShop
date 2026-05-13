using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Represents a product availability range
/// </summary>
public sealed class ProductAvailabilityRange : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}