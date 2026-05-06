namespace Ork.Core.Domain.Customers;

/// <summary>
/// Customer registered event
/// </summary>
/// <param name="customer">The customer that was registered</param>
public sealed class CustomerRegisteredEvent(Customer customer)
{
    public Customer Customer { get; } = customer;
}
