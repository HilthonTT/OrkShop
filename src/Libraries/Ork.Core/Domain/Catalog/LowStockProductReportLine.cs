namespace Ork.Core.Domain.Catalog;

/// <summary>
/// Represents low stock report line
/// </summary>
public sealed class LowStockProductReportLine
{
    /// <summary>
    /// Product Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Product Attributes
    /// </summary>
    public string Attributes { get; set; } = string.Empty;

    /// <summary>
    /// Manage Inventory Method
    /// </summary>
    public string ManageInventoryMethod { get; set; } = string.Empty;

    /// <summary>
    /// Stock Quantity
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Published
    /// </summary>
    public bool Published { get; set; }
}