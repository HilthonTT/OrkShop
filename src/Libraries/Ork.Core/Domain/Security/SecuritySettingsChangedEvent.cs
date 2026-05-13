namespace Ork.Core.Domain.Security;

/// <summary>
/// Security setting changed event
/// </summary>
/// <remarks>
/// Initialize a new instance of the SecuritySettingsChangedEvent
/// </remarks>
/// <param name="SecuritySettings">Security settings</param>
/// <param name="oldEncryptionPrivateKey">Previous encryption key value</param>
public sealed class SecuritySettingsChangedEvent(SecuritySettings securitySettings, string oldEncryptionPrivateKey)
{
    /// <summary>
    /// Security settings
    /// </summary>
    public SecuritySettings SecuritySettings { get; set; } = securitySettings;

    /// <summary>
    /// Previous encryption key value
    /// </summary>
    public string OldEncryptionPrivateKey { get; set; } = oldEncryptionPrivateKey;
}