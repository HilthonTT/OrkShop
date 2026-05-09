namespace Ork.Core.Domain.Discounts;

public sealed class DiscountRequirement : BaseEntity
{
    public int DiscountId { get; set; }

    public string DiscountRequirementRuleSystemName { get; set; } = string.Empty;

    public int? ParentId { get; set; }

    public int? InteractionTypeId { get; set; }

    public bool IsGroup { get; set; }

    public RequirementGroupInteractionType? InteractionType
    {
        get => (RequirementGroupInteractionType?)InteractionTypeId;
        set => InteractionTypeId = (int?)value;
    }
}
