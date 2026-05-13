namespace Ork.Core.Domain.Messages;

public sealed class EmailAccount : BaseEntity
{
    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool EnableSsl { get; set; }

    public int MaxNumberOfEmails { get; set; }

    public int EmailAuthenticationMethodId { get; set; }

    public EmailAuthenticationMethod EmailAuthenticationMethod
    {
        get => (EmailAuthenticationMethod)EmailAuthenticationMethodId;
        set => EmailAuthenticationMethodId = (int)value;
    }

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string TenantId { get; set; } = string.Empty;
}
