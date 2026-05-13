namespace Ork.Core.Domain.Orders;

/// <summary>
/// Order paid event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="order">Order</param>
public sealed class OrderPaidEvent(Order order)
{
    /// <summary>
    /// Order
    /// </summary>
    public Order Order { get; } = order;
}