namespace Ork.Core.Domain.Catalog;

/// <summary>
/// Represents a back in stock subscription
/// </summary>
public sealed class BackInStockSubscription : BaseEntity
{
    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
