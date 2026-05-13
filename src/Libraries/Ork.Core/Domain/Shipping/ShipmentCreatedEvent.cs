namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Shipment created event
/// </summary>
public sealed class ShipmentCreatedEvent(Shipment shipment)
{
    /// <summary>
    /// Gets the shipment
    /// </summary>
    public Shipment Shipment { get; } = shipment;
}