using Ork.Core.Configuration;

namespace Ork.Core.Domain.FilterLevels;

public sealed class FilterLevelSettings : ISettings
{
    public bool FilterLevelEnabled { get; set; }

    public bool DisplayOnHomePage { get; set; }

    public bool DisplayOnProductDetailsPage { get; set; }

    public List<int> FilterLevelEnumDisabled { get; set; } = [];
}