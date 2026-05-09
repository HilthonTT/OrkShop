namespace Ork.Core.Domain.Directory;

public sealed class ExchangeRate
{
    public string CurrencyCode { get; set; } = string.Empty;

    public decimal Rate { get; set; } = 1.0m;

    public DateTime UpdatedOn { get; set; }

    /// <summary>
    /// Format the rate into a string with the currency code, e.g. "USD 0.72543"
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{CurrencyCode} {Rate}";
    }
}
