namespace Ork.Core.Domain.Media;

/// <summary>
/// Represents a download
/// </summary>
public sealed class Download : BaseEntity
{
    /// <summary>
    /// Gets or sets a GUID
    /// </summary>
    public Guid DownloadGuid { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether DownloadUrl property should be used
    /// </summary>
    public bool UseDownloadUrl { get; set; }

    /// <summary>
    /// Gets or sets a download URL
    /// </summary>
    public string DownloadUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the download binary
    /// </summary>
    public byte[] DownloadBinary { get; set; } = [];

    /// <summary>
    /// The mime-type of the download
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// The filename of the download
    /// </summary>
    public string Filename { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the extension
    /// </summary>
    public string Extension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the download is new
    /// </summary>
    public bool IsNew { get; set; }
}
