namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Type converter for a list of <see cref="ShippingOption"/>.
/// </summary>
public partial class ShippingOptionListTypeConverter : XmlTypeConverter<List<ShippingOption>>;