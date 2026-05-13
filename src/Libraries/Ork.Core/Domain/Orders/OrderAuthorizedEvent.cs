namespace Ork.Core.Domain.Orders;

/// <summary>
/// Order authorized event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="order">Order</param>
public sealed class OrderAuthorizedEvent(Order order)
{

    /// <summary>
    /// Order
    /// </summary>
    public Order Order { get; } = order;
}