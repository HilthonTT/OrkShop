namespace Ork.Core.Domain.Customers;

/// <summary>
/// Customer change working language event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="customer">Customer</param>
public sealed class CustomerChangeWorkingLanguageEvent(Customer customer)
{
    public Customer Customer { get; } = customer;
}
