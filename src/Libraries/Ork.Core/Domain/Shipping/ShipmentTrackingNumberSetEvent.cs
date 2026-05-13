namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Shipment tracking number set event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="shipment">Shipment</param>
public sealed class ShipmentTrackingNumberSetEvent(Shipment shipment)
{

    /// <summary>
    /// Shipment
    /// </summary>
    public Shipment Shipment { get; } = shipment;
}