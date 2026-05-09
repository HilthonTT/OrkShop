namespace Ork.Core.Domain.Logging;

public sealed class ActivityLog : BaseEntity
{
    public int ActivityLogTypeId { get; set; }

    public int? EntityId { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedOnUtc { get; set; }

    public string IpAddress { get; set; } = string.Empty;
}
