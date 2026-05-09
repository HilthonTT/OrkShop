namespace Ork.Core.Domain.Gdpr;

/// <summary>
/// Customer permanently deleted (GDPR)
/// </summary>
public sealed class CustomerPermanentlyDeleted(int customerId, string email)
{
    public int CustomerId { get; set; } = customerId;

    public string Email { get; set; } = email;
}
