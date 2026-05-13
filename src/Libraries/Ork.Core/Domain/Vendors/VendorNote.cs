namespace Ork.Core.Domain.Vendors;

/// <summary>
/// Represents a vendor note
/// </summary>
public sealed class VendorNote : BaseEntity
{
    /// <summary>
    /// Gets or sets the vendor identifier
    /// </summary>
    public int VendorId { get; set; }

    /// <summary>
    /// Gets or sets the note
    /// </summary>
    public string Note { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time of vendor note creation
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }
}