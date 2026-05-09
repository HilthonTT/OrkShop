using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Directory;

public sealed class StateProvince : BaseEntity, ILocalizedEntity
{
    public int CountryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Abbreviation { get; set; } = string.Empty;

    public bool Published { get; set; }

    public int DisplayOrder { get; set; }
}
