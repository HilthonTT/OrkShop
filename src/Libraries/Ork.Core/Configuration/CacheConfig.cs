namespace Ork.Core.Configuration;

/// <summary>
/// Represents cache configuration parameters. 
/// This class is used to store cache-related settings that can be accessed throughout the application.
/// </summary>
public sealed class CacheConfig : IConfig
{
    public static class Defaults
    {
        public const int DefaultCacheTime = 60; // seconds
    }

    /// <summary>
    /// Gets or sets the default cache time in minutes
    /// </summary>
    public int DefaultCacheTime { get; set; } = Defaults.DefaultCacheTime;

    /// <summary>
    /// Gets or sets whether to disable linq2db query cache
    /// </summary>
    public bool LinqDisableQueryCache { get; set; } = false;
}
