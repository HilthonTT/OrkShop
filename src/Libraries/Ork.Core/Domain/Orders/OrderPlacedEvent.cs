namespace Ork.Core.Domain.Orders;

/// <summary>
/// Order placed event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="order">Order</param>
public sealed class OrderPlacedEvent(Order order)
{
    /// <summary>
    /// Order
    /// </summary>
    public Order Order { get; } = order;
}