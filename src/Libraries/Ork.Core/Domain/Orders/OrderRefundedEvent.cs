namespace Ork.Core.Domain.Orders;

/// <summary>
/// Order refunded event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="order">Order</param>
/// <param name="amount">Amount</param>
public sealed class OrderRefundedEvent(Order order, decimal amount)
{
    /// <summary>
    /// Order
    /// </summary>
    public Order Order { get; } = order;

    /// <summary>
    /// Amount
    /// </summary>
    public decimal Amount { get; } = amount;
}