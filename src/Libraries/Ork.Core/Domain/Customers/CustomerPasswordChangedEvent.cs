namespace Ork.Core.Domain.Customers;

/// <summary>
/// Customer password changed event
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="password">Password</param>
public sealed class CustomerPasswordChangedEvent(CustomerPassword password)
{
    /// <summary>
    /// Customer password
    /// </summary>
    public CustomerPassword Password { get; } = password;
}