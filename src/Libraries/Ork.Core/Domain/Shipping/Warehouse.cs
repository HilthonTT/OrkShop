namespace Ork.Core.Domain.Shipping;

/// <summary>
/// Represents a warehouse
/// </summary>
public sealed class Warehouse : BaseEntity
{
    /// <summary>
    /// Gets or sets the warehouse name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the admin comment
    /// </summary>
    public string AdminComment { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address identifier of the warehouse
    /// </summary>
    public int AddressId { get; set; }
}