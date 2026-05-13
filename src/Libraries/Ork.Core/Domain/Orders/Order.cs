using Ork.Core.Domain.Common;
using Ork.Core.Domain.Payments;
using Ork.Core.Domain.Shipping;
using Ork.Core.Domain.Tax;

namespace Ork.Core.Domain.Orders;

public sealed class Order : BaseEntity, ISoftDeletedEntity
{
    public Guid OrderGuid { get; set; }

    public int StoreId { get; set; }

    public int CustomerId { get; set; }

    public int BillingAddressId { get; set; }

    public int? ShippingAddressId { get; set; }

    public DateTime? DesiredDeliveryDateUtc { get; set; }

    public int? PickupAddressId { get; set; }

    public bool PickupInStore { get; set; }

    public int OrderStatusId { get; set; }

    public int ShippingStatusId { get; set; }

    public int PaymentStatusId { get; set; }

    public string PaymentMethodSystemName { get; set; } = string.Empty;

    public string CustomerCurrencyCode { get; set; } = string.Empty;

    public decimal CurrencyRate { get; set; }

    public int CustomerTaxDisplayTypeId { get; set; }

    public string VatNumber { get; set; } = string.Empty;

    public decimal OrderSubTotalInclTax { get; set; }

    /// <summary>
    /// Gets or sets the order subtotal discount (include tax)
    /// </summary>
    public decimal OrderSubTotalDiscountInclTax { get; set; }

    /// <summary>
    /// Gets or sets the order subtotal discount (exclude tax)
    /// </summary>
    public decimal OrderSubTotalDiscountExclTax { get; set; }

    /// <summary>
    /// Gets or sets the order shipping (include tax)
    /// </summary>
    public decimal OrderShippingInclTax { get; set; }

    /// <summary>
    /// Gets or sets the order shipping (exclude tax)
    /// </summary>
    public decimal OrderShippingExclTax { get; set; }

    /// <summary>
    /// Gets or sets the payment method additional fee (incl tax)
    /// </summary>
    public decimal PaymentMethodAdditionalFeeInclTax { get; set; }

    /// <summary>
    /// Gets or sets the payment method additional fee (exclude tax)
    /// </summary>
    public decimal PaymentMethodAdditionalFeeExclTax { get; set; }

    /// <summary>
    /// Gets or sets the tax rates
    /// </summary>
    public string TaxRates { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the order tax
    /// </summary>
    public decimal OrderTax { get; set; }

    /// <summary>
    /// Gets or sets the order discount (applied to order total)
    /// </summary>
    public decimal OrderDiscount { get; set; }

    /// <summary>
    /// Gets or sets the order total
    /// </summary>
    public decimal OrderTotal { get; set; }

    /// <summary>
    /// Gets or sets the refunded amount
    /// </summary>
    public decimal RefundedAmount { get; set; }

    /// <summary>
    /// Gets or sets the reward points history entry identifier when reward points were earned (gained) for placing this order
    /// </summary>
    public int? RewardPointsHistoryEntryId { get; set; }

    /// <summary>
    /// Gets or sets the checkout attribute description
    /// </summary>
    public string CheckoutAttributeDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the checkout attributes in XML format
    /// </summary>
    public string CheckoutAttributesXml { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer language identifier
    /// </summary>
    public int CustomerLanguageId { get; set; }

    /// <summary>
    /// Gets or sets the affiliate identifier
    /// </summary>
    public int AffiliateId { get; set; }

    /// <summary>
    /// Gets or sets the customer IP address
    /// </summary>
    public string CustomerIp { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether storing of credit card number is allowed
    /// </summary>
    public bool AllowStoringCreditCardNumber { get; set; }

    /// <summary>
    /// Gets or sets the card type
    /// </summary>
    public string CardType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the card name
    /// </summary>
    public string CardName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the card number
    /// </summary>
    public string CardNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the masked credit card number
    /// </summary>
    public string MaskedCreditCardNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the card CVV2
    /// </summary>
    public string CardCvv2 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the card expiration month
    /// </summary>
    public string CardExpirationMonth { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the card expiration year
    /// </summary>
    public string CardExpirationYear { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the authorization transaction identifier
    /// </summary>
    public string AuthorizationTransactionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the authorization transaction code
    /// </summary>
    public string AuthorizationTransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the authorization transaction result
    /// </summary>
    public string AuthorizationTransactionResult { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the capture transaction identifier
    /// </summary>
    public string CaptureTransactionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the capture transaction result
    /// </summary>
    public string CaptureTransactionResult { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the subscription transaction identifier
    /// </summary>
    public string SubscriptionTransactionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the paid date and time
    /// </summary>
    public DateTime? PaidDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the shipping method
    /// </summary>
    public string ShippingMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the shipping rate computation method identifier or the pickup point provider identifier (if PickupInStore is true)
    /// </summary>
    public string ShippingRateComputationMethodSystemName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized CustomValues (values from ProcessPaymentRequest)
    /// </summary>
    public string CustomValuesXml { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the entity has been deleted
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// Gets or sets the date and time of order creation
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the custom order number without prefix
    /// </summary>
    public string CustomOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reward points history record (spent by a customer when placing this order)
    /// </summary>
    public int? RedeemedRewardPointsEntryId { get; set; }

    /// <summary>
    /// Gets or sets the current follow-up number for the pending order
    /// </summary>
    public int? LastPendingOrderFollowUpNumber { get; set; }

    /// <summary>
    /// Gets or sets the date and time (UTC) when the last follow-up for the pending order was sent
    /// </summary>
    public DateTime? LastPendingOrderFollowUpDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the order status
    /// </summary>
    public OrderStatus OrderStatus
    {
        get => (OrderStatus)OrderStatusId;
        set => OrderStatusId = (int)value;
    }

    /// <summary>
    /// Gets or sets the payment status
    /// </summary>
    public PaymentStatus PaymentStatus
    {
        get => (PaymentStatus)PaymentStatusId;
        set => PaymentStatusId = (int)value;
    }

    /// <summary>
    /// Gets or sets the shipping status
    /// </summary>
    public ShippingStatus ShippingStatus
    {
        get => (ShippingStatus)ShippingStatusId;
        set => ShippingStatusId = (int)value;
    }

    /// <summary>
    /// Gets or sets the customer tax display type
    /// </summary>
    public TaxDisplayType CustomerTaxDisplayType
    {
        get => (TaxDisplayType)CustomerTaxDisplayTypeId;
        set => CustomerTaxDisplayTypeId = (int)value;
    }
}
