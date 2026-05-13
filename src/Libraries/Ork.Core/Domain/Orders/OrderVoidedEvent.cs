namespace Ork.Core.Domain.Orders;

/// <summary>
/// Order voided event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="order">Order</param>
public sealed class OrderVoidedEvent(Order order)
{
    /// <summary>
    /// Voided order
    /// </summary>
    public Order Order { get; } = order;
}