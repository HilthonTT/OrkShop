using Newtonsoft.Json;
using Ork.Core;
using Ork.Core.Configuration;
using Ork.Core.Infrastructure;
using Ork.Data.Configuration;
using System.Text;

namespace Ork.Data;

internal static class DataSettingsManager
{
    /// <summary>
    /// Gets a cached value indicating whether the database is installed. We need this value invariable during installation process
    /// </summary>
    private static bool _databaseIsInstalled = false;

    public static DataConfig? LoadDataSettingsFromOldTxtFile(string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return null;
        }

        var dataSettings = new DataConfig();
        using var reader = new StringReader(data);
        string? settingsLine;

        while ((settingsLine = reader.ReadLine()) is not null)
        {
            int separatorIndex = settingsLine.IndexOf(':');
            if (separatorIndex == -1)
            {
                continue;
            }

            string key = settingsLine[..separatorIndex].Trim();
            string value = settingsLine[(separatorIndex + 1)..].Trim();

            switch (key)
            {
                case "DataProvider":
                    dataSettings.DataProvider = Enum.TryParse(value, true, out DataProviderType providerType)
                        ? providerType 
                        : DataProviderType.Unknown;
                    continue;
                case "DataConnectionString":
                    dataSettings.ConnectionString = value;
                    continue;
                case "SQLCommandTimeout":
                    //If parsing isn't successful, we set a negative timeout, that means the current provider will use a default value
                    dataSettings.SQLCommandTimeout = int.TryParse(value, out var timeout) ? timeout : -1;
                    continue;
                default:
                    break;
            }
        }

        return dataSettings;
    }

    public static DataConfig? LoadDataSettingsFromOldJsonFile(string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return null;
        }

        var jsonDataSettings = JsonConvert.DeserializeAnonymousType(
            data,
            new { DataConnectionString = "", DataProvider = DataProviderType.SqlServer, SQLCommandTimeout = "" });

        if (jsonDataSettings is null)
        {
            return null;
        }

        var dataSettings = new DataConfig
        {
            ConnectionString = jsonDataSettings.DataConnectionString,
            DataProvider = jsonDataSettings.DataProvider,
            SQLCommandTimeout = int.TryParse(jsonDataSettings.SQLCommandTimeout, out var result) ? result : null
        };

        return dataSettings;
    }

    public static DataConfig? LoadSettings(IOrkFileProvider? fileProvider = null, bool reload = false)
    {
        if (!reload && Singleton<DataConfig>.Instance is not null)
        {
            return Singleton<DataConfig>.Instance;
        }

        // backward compatibility
        fileProvider ??= CommonHelper.DefaultFileProvider;

        string filePathJson = fileProvider.MapPath(OrkDataSettingsDefaults.FilePath);
        string filePathTxt = fileProvider.MapPath(OrkDataSettingsDefaults.ObsoleteFilePath);

        if (fileProvider.FileExists(filePathJson) || fileProvider.FileExists(filePathTxt))
        {
            DataConfig dataSettings = (fileProvider.FileExists(filePathJson)
                ? LoadDataSettingsFromOldJsonFile(fileProvider.ReadAllText(filePathJson, Encoding.UTF8))
                : LoadDataSettingsFromOldTxtFile(fileProvider.ReadAllText(filePathTxt, Encoding.UTF8)))
                  ?? new DataConfig();

            fileProvider.DeleteFile(filePathJson);
            fileProvider.DeleteFile(filePathTxt);

            AppSettingsHelper.SaveAppSettings([dataSettings], fileProvider);
            Singleton<DataConfig>.Instance = dataSettings;
        }
        else
        {
            Singleton<DataConfig>.Instance = Singleton<AppSettings>.Instance?.Get<DataConfig>();
        }

        return Singleton<DataConfig>.Instance;
    }

    public static bool IsDatabaseInstalled()
    {
        _databaseIsInstalled = !string.IsNullOrEmpty(LoadSettings()?.ConnectionString);
        return _databaseIsInstalled;
    }

    public static void SaveSettings(DataConfig dataSettings, IOrkFileProvider fileProvider)
    {
        AppSettingsHelper.SaveAppSettings([dataSettings], fileProvider);
        LoadSettings(fileProvider, reload: true);
    }
}
