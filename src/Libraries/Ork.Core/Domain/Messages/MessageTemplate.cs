using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Messages;

public sealed class MessageTemplate : BaseEntity, ILocalizedEntity, IStoreMappingSupported
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int? DelayBeforeSend { get; set; }

    public int DelayPeriodId { get; set; }

    public int AttachedDownloadId { get; set; }

    public bool AllowDirectReply { get; set; }

    public int EmailAccountId { get; set; }

    public bool LimitedToStores { get; set; }

    public MessageDelayPeriod DelayPeriod
    {
        get => (MessageDelayPeriod)DelayPeriodId;
        set => DelayPeriodId = (int)value;
    }
}
