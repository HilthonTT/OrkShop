namespace Ork.Core.Domain.Discounts;

/// <summary>
/// Represents a discount-category mapping class
/// </summary>
public sealed class DiscountCategoryMapping : DiscountMapping
{
    /// <summary>
    /// Gets or sets the category identifier
    /// </summary>
    public override int EntityId { get; set; }
}
