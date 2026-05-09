using Microsoft.Extensions.Logging;

namespace Ork.Core.Domain.Logging;

/// <summary>
/// Represents a log record
/// </summary>
public sealed class Log : BaseEntity
{
    /// <summary>
    /// Gets or sets the log level identifier
    /// </summary>
    public int LogLevelId { get; set; }

    /// <summary>
    /// Gets or sets the short message
    /// </summary>
    public string ShortMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full exception
    /// </summary>
    public string FullMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the IP address
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer identifier
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the page URL
    /// </summary>
    public string PageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the referrer URL
    /// </summary>
    public string ReferrerUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time of instance creation
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the log level
    /// </summary>
    public LogLevel LogLevel
    {
        get => (LogLevel)LogLevelId;
        set => LogLevelId = (int)value;
    }
}
