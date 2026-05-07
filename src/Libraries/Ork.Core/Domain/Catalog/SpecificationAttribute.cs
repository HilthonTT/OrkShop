using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Catalog;

/// <summary>
/// Represents a specification attribute
/// </summary>
public sealed class SpecificationAttribute : BaseEntity, ILocalizedEntity
{
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets the specification attribute group identifier
    /// </summary>
    public int? SpecificationAttributeGroupId { get; set; }
}