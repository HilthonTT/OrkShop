namespace Ork.Core.Domain.Customers;

/// <summary>
/// Customer activated event
/// </summary>
/// <param name="customer">The customer that was activated</param>
public sealed class CustomerActivatedEvent(Customer customer)
{
    public Customer Customer { get; } = customer;
}
