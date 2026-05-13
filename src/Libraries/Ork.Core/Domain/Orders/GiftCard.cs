using Ork.Core.Domain.Catalog;

namespace Ork.Core.Domain.Orders;

/// <summary>
/// Represents a gift card
/// </summary>
public sealed class GiftCard : BaseEntity
{
    /// <summary>
    /// Gets or sets the associated order item identifier
    /// </summary>
    public int? PurchasedWithOrderItemId { get; set; }

    /// <summary>
    /// Gets or sets the gift card type identifier
    /// </summary>
    public int GiftCardTypeId { get; set; }

    /// <summary>
    /// Gets or sets the amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gift card is activated
    /// </summary>
    public bool IsGiftCardActivated { get; set; }

    /// <summary>
    /// Gets or sets a gift card coupon code
    /// </summary>
    public string GiftCardCouponCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a recipient name
    /// </summary>
    public string RecipientName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a recipient email
    /// </summary>
    public string RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a sender name
    /// </summary>
    public string SenderName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a sender email
    /// </summary>
    public string SenderEmail { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether recipient is notified
    /// </summary>
    public bool IsRecipientNotified { get; set; }

    /// <summary>
    /// Gets or sets the date and time of instance creation
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the gift card type
    /// </summary>
    public GiftCardType GiftCardType
    {
        get => (GiftCardType)GiftCardTypeId;
        set => GiftCardTypeId = (int)value;
    }
}
