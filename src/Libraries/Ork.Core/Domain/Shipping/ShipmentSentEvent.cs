namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Shipment sent event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="shipment">Shipment</param>
public sealed class ShipmentSentEvent(Shipment shipment)
{

    /// <summary>
    /// Shipment
    /// </summary>
    public Shipment Shipment { get; } = shipment;
}