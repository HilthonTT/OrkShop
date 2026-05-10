using Ork.Core.Domain.Common;
using Ork.Core.Domain.Localization;
using Ork.Core.Domain.Security;

namespace Ork.Core.Domain.Menus;

public sealed class Menu : BaseEntity, IAclSupported, IStoreMappingSupported, ISoftDeletedEntity, ILocalizedEntity
{
    public string Name { get; set; } = string.Empty;

    public int MenuTypeId { get; set; }

    public string CssClass { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public bool Published { get; set; }

    public bool Deleted { get; set; }

    public bool DisplayAllCategories { get; set; }

    public bool LimitedToStores { get; set; }

    public bool SubjectToAcl { get; set; }

    public MenuType MenuType
    {
        get => (MenuType)MenuTypeId;
        set => MenuTypeId = (int)value;
    }
}
