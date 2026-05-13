namespace Ork.Core.Domain.Messages;

public sealed class NewsLetterSubscription : BaseEntity
{
    public Guid NewsLetterSubscriptionGuid { get; set; }

    public string Email { get; set; } = string.Empty;

    public bool Active { get; set; }

    public int StoreId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int LanguageId { get; set; }

    /// <summary>
    /// Gets or sets the subscription type identifier in which a customer has subscribed to newsletter
    /// </summary>
    public int TypeId { get; set; }
}