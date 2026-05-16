namespace Ork.Data;

/// <summary>
/// Represents default values related to data settings
/// </summary>
public static class OrkDataSettingsDefaults
{
    /// <summary>
    /// Gets a path to the file that was used in old nopCommerce versions to contain data settings
    /// </summary>
    public const string ObsoleteFilePath = "~/App_Data/Settings.txt";

    /// <summary>
    /// Gets a path to the file that contains data settings
    /// </summary>
    public const string FilePath = "~/App_Data/dataSettings.json";
}