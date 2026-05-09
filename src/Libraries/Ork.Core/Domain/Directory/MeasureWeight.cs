namespace Ork.Core.Domain.Directory;

public sealed class MeasureWeight : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string SystemKeyword { get; set; } = string.Empty;

    public decimal Ratio { get; set; }

    public int DisplayOrder { get; set; }
}