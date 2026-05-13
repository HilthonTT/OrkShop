namespace Ork.Core.Domain.Orders;

[Serializable]
public sealed class BestsellersReportLine
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public string TotalAmountStr { get; set; } = string.Empty;

    public int TotalQuantity { get; set; }
}
