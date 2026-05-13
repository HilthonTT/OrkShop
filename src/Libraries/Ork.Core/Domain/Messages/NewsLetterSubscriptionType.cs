using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Messages;

/// <summary>
/// Represents newsletter subscription type entity
/// </summary>
public sealed class NewsLetterSubscriptionType : BaseEntity, ILocalizedEntity, IStoreMappingSupported
{
    /// <summary>
    /// Gets or sets the name of subscription type
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether subscription type is active
    /// </summary>
    public bool TickedByDefault { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is limited/restricted to certain stores
    /// </summary>
    public bool LimitedToStores { get; set; }
}
