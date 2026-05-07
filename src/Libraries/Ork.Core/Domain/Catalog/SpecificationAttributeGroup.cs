using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Catalog;

/// <summary>
/// Represents a specification attribute group
/// </summary>
public sealed class SpecificationAttributeGroup : BaseEntity, ILocalizedEntity
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