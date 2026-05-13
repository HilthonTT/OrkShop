using Ork.Core.Domain.Customers;

namespace Ork.Core.Domain.Orders;

/// <summary>
/// Reset checkout data event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="customer">Customer</param>
/// <param name="storeId">Store identifier</param>
public sealed class ResetCheckoutDataEvent(Customer customer, int storeId)
{
    /// <summary>
    /// Customer
    /// </summary>
    public Customer Customer { get; } = customer;

    /// <summary>
    /// Store identifier
    /// </summary>
    public int StoreId { get; } = storeId;
}