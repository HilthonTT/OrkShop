using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Represents a shipping method (used by offline shipping rate computation methods)
/// </summary>
public sealed class ShippingMethod : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}