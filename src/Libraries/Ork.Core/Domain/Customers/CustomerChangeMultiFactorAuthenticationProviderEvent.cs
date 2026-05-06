namespace Ork.Core.Domain.Customers;

/// <summary>
/// "Customer is change multi-factor authentication provider" event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="customer">Customer</param>
public sealed class CustomerChangeMultiFactorAuthenticationProviderEvent(Customer customer)
{
    /// <summary>
    /// Get or set the customer
    /// </summary>
    public Customer Customer { get; } = customer;
}