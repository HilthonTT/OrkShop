using Ork.Core.Domain.Discounts;
using Ork.Core.Events;

namespace Ork.Core.Domain.Orders;

/// <summary>
/// Shopping cart item get unit price event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="shoppingCartItem">Shopping cart item</param>
/// <param name="includeDiscounts">A value indicating whether include discounts or not for price computation</param>
public sealed class GetShoppingCartItemUnitPriceEvent(ShoppingCartItem shoppingCartItem, bool includeDiscounts) 
    : IStopProcessingEvent
{
    /// <summary>
    /// Gets a value indicating whether include discounts or not for price computation
    /// </summary>
    public bool IncludeDiscounts { get; } = includeDiscounts;

    /// <summary>
    /// Gets the shopping cart item
    /// </summary>
    public ShoppingCartItem ShoppingCartItem { get; } = shoppingCartItem;

    /// <summary>
    /// Gets or sets a value whether processing of event publishing should be stopped
    /// </summary>
    public bool StopProcessing { get; set; }

    /// <summary>
    /// Gets or sets the unit price in primary store currency
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount amount
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the applied discounts
    /// </summary>
    public List<Discount> AppliedDiscounts { get; set; } = new();
}
