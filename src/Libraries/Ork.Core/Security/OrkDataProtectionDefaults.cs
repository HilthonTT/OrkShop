namespace Ork.Core.Security;

/// <summary>
/// Represents default values related to data protection
/// </summary>
public static class OrkDataProtectionDefaults
{
    /// <summary>
    /// Gets the name of the key path used to store the protection key list to local file system
    /// </summary>
    public const string DataProtectionKeysPath = "~/App_Data/DataProtectionKeys";
}
