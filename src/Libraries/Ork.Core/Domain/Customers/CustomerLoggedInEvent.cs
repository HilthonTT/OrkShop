namespace Ork.Core.Domain.Customers;

/// <summary>
/// Customer logged-in event
/// </summary>
public sealed class CustomerLoggedInEvent(Customer customer, Customer? guestCustomer = null)
{
    public Customer Customer { get; } = customer;

    public Customer? GuestCustomer { get; } = guestCustomer;
}

/// <summary>
/// "Customer is logged out" event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="customer">Customer</param>
public sealed class CustomerLoggedOutEvent(Customer customer)
{
    /// <summary>
    /// Get or set the customer
    /// </summary>
    public Customer Customer { get; } = customer;
}
