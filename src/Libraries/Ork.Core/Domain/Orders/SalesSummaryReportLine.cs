namespace Ork.Core.Domain.Orders;

/// <summary>
/// Represents sales summary report line
/// </summary>
public sealed class SalesSummaryReportLine
{
    public string Summary { get; set; } = string.Empty;

    public DateTime SummaryDate { get; set; }

    public int NumberOfOrders { get; set; }

    public decimal Profit { get; set; }

    public string ProfitStr { get; set; } = string.Empty;

    public string Shipping { get; set; } = string.Empty;

    public string Tax { get; set; } = string.Empty;

    public string OrderTotal { get; set; } = string.Empty;

    public int SummaryType { get; set; }
}