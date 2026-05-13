namespace Ork.Core.Domain.Orders;

/// <summary>
/// Order status changed event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="order">Order</param>
/// <param name="previousOrderStatus">Previous order status</param>
public sealed class OrderStatusChangedEvent(Order order, OrderStatus previousOrderStatus)
{
    /// <summary>
    /// Order
    /// </summary>
    public Order Order { get; } = order;

    /// <summary>
    /// Previous order status
    /// </summary>
    public OrderStatus PreviousOrderStatus { get; set; } = previousOrderStatus;
}