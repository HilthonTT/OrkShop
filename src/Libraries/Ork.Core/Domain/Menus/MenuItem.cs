using Ork.Core.Domain.Localization;
using Ork.Core.Domain.Security;

namespace Ork.Core.Domain.Menus;

public sealed class MenuItem : BaseEntity, IAclSupported, IStoreMappingSupported, ILocalizedEntity
{
    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int MenuItemTypeId { get; set; }

    public int? EntityId { get; set; }

    public int? NumberOfSubItemsPerGridElement { get; set; }

    public int? NumberOfItemsPerGrid { get; set; }

    public int? MaximumNumberOfEntities { get; set; }

    public string RouteName { get; set; } = string.Empty;

    public int TemplateId { get; set; }

    public string CssClass { get; set; } = string.Empty;

    public bool Published { get; set; }

    public int MenuId { get; set; }

    public bool LimitedToStores { get; set; }

    public bool SubjectToAcl { get; set; }

    public MenuItemType MenuItemType
    {
        get => (MenuItemType)MenuItemTypeId;
        set => MenuItemTypeId = (int)value;
    }

    public MenuItemTemplate Template
    {
        get => (MenuItemTemplate)TemplateId;
        set => TemplateId = (int)value;
    }
}
