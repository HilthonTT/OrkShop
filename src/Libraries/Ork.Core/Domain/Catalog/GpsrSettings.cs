using Ork.Core.Configuration;

namespace Ork.Core.Domain.Catalog;

/// <summary>
/// GPSR settings
/// </summary>
public sealed class GpsrSettings : ISettings
{
    /// <summary>
    /// Gets or sets a value indicating whether GPSR is enabled
    /// </summary>
    public bool Enabled { get; set; }
}