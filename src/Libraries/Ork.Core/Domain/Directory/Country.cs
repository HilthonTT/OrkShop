using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Directory;

public sealed class Country : BaseEntity, ILocalizedEntity, IStoreMappingSupported
{
    public string Name { get; set; } = string.Empty;

    public bool AllowsBilling { get; set; }

    public bool AllowsShipping { get; set; }

    public string TwoLetterIsoCode { get; set; } = string.Empty;

    public string ThreeLetterIsoCode { get; set; } = string.Empty;

    public bool LimitedToStores { get; set; }

    public bool SubjectToVat { get; set; }

    public bool Published { get; set; }

    public int DisplayOrder { get; set; }
}
