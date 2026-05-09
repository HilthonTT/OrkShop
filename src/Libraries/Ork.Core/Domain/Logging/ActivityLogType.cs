namespace Ork.Core.Domain.Logging;

/// <summary>
/// Represents an activity log type record
/// </summary>
public sealed class ActivityLogType : BaseEntity
{
    /// <summary>
    /// Gets or sets the system keyword
    /// </summary>
    public string SystemKeyword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the activity log type is enabled
    /// </summary>
    public bool Enabled { get; set; }
}