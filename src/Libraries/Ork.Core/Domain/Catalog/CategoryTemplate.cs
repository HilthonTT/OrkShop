namespace Ork.Core.Domain.Catalog;

public sealed class CategoryTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string ViewPath { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
}
