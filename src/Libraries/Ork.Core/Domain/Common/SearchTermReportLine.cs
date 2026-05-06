namespace Ork.Core.Domain.Common;

/// <summary>
/// Search term record (for statistics)
/// </summary>
public sealed class SearchTermReportLine
{
    /// <summary>
    /// Gets or sets the keyword
    /// </summary>
    public string Keyword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets search count
    /// </summary>
    public int Count { get; set; }
}