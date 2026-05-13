using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Orders;

/// <summary>
/// Represents a return request reason
/// </summary>
public sealed class ReturnRequestReason : BaseEntity, ILocalizedEntity
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