using Ork.Core.Domain.Attributes;

namespace Ork.Core.Domain.Orders;

/// <summary>
/// Represents a checkout attribute value
/// </summary>
public sealed class CheckoutAttributeValue : BaseAttributeValue
{
    /// <summary>
    /// Gets or sets the color RGB value (used with "Color squares" attribute type)
    /// </summary>
    public string ColorSquaresRgb { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price adjustment
    /// </summary>
    public decimal PriceAdjustment { get; set; }

    /// <summary>
    /// Gets or sets the weight adjustment
    /// </summary>
    public decimal WeightAdjustment { get; set; }
}