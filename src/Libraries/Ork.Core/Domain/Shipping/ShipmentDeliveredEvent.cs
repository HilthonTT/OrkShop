namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Shipment delivered event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="shipment">Shipment</param>
public sealed class ShipmentDeliveredEvent(Shipment shipment)
{
    /// <summary>
    /// Shipment
    /// </summary>
    public Shipment Shipment { get; } = shipment;
}
