namespace Ork.Core.Configuration;

/// <summary>
/// Represents default values related to configuration services
/// </summary>
public static class OrkConfigurationDefaults
{
    /// <summary>
    /// Gets the path to file that contains app settings
    /// </summary>
    public const string AppSettingsFilePath = "App_Data/appsettings.json";

    /// <summary>
    /// Gets the path to file that contains app settings for specific hosting environment
    /// </summary>
    /// <remarks>0 - Environment name</remarks>
    public const string AppSettingsEnvironmentFilePath = "App_Data/appsettings.{0}.json";
}