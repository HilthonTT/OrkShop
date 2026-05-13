namespace Ork.Core.Domain.Orders;

/// <summary>
/// Shopping cart cleared event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="cart">Shopping cart</param>
public sealed class ClearShoppingCartEvent(List<ShoppingCartItem> cart)
{
    /// <summary>
    /// Shopping cart
    /// </summary>
    public List<ShoppingCartItem> Cart { get; } = cart;
}
