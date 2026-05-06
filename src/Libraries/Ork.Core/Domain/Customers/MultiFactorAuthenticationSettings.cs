using Ork.Core.Configuration;

namespace Ork.Core.Domain.Customers;

/// <summary>
/// Multi-factor authentication settings
/// </summary>
public sealed class MultiFactorAuthenticationSettings : ISettings
{
    /// <summary>
    /// Gets or sets system names of active multi-factor authentication methods
    /// </summary>
    public List<string> ActiveAuthenticationMethodSystemNames { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether to force multi-factor authentication
    /// </summary>
    public bool ForceMultifactorAuthentication { get; set; }
}
