namespace Ork.Core.Domain.Messages;

/// <summary>
/// Represents a campaign
/// </summary>
public sealed class Campaign : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public int StoreId { get; set; }

    public int CustomerRoleId { get; set; }

    public int NewsLetterSubscriptionTypeId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? DontSendBeforeDateUtc { get; set; }
}
