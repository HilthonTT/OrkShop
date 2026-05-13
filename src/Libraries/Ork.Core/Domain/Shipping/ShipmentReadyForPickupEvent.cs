namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Shipment ready for pickup event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="shipment">Shipment</param>
public sealed class ShipmentReadyForPickupEvent(Shipment shipment)
{
    /// <summary>
    /// Shipment
    /// </summary>
    public Shipment Shipment { get; } = shipment;
}