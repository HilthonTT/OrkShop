using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Directory;

public sealed class Currency : BaseEntity, ILocalizedEntity, IStoreMappingSupported
{
    public string Name { get; set; } = string.Empty;

    public string CurrencyCode { get; set; } = string.Empty;

    public decimal Rate { get; set; }

    public string DisplayLocale { get; set; } = string.Empty;

    public bool LimitedToStores { get; set; }

    public string CustomFormatting { get; set; } = string.Empty;

    public bool Published { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime UpdatedOnUtc { get; set; }

    public int RoundingTypeId { get; set; }

    public RoundingType RoundingType
    {
        get => (RoundingType)RoundingTypeId;
        set => RoundingTypeId = (int)value;
    }
}
