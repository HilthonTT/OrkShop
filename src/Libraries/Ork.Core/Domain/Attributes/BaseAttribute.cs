using Ork.Core.Domain.Catalog;
using Ork.Core.Domain.Localization;

namespace Ork.Core.Domain.Attributes;

public abstract class BaseAttribute : BaseEntity, ILocalizedEntity
{
    public string Name { get; set; } = string.Empty;

    public bool IsRequired { get; set; }

    public int AttributeControlTypeId { get; set; }

    public int DisplayOrder { get; set; }

    public AttributeControlType AttributeControlType
    {
        get => (AttributeControlType)AttributeControlTypeId;
        set => AttributeControlTypeId = (int)value;
    }

    /// <summary>
    /// A value indicating whether this attribute should have values
    /// </summary>
    public bool ShouldHaveValues
    {
        get
        {
            if (AttributeControlType == AttributeControlType.TextBox ||
                AttributeControlType == AttributeControlType.MultilineTextbox ||
                AttributeControlType == AttributeControlType.Datepicker ||
                AttributeControlType == AttributeControlType.FileUpload)
            {
                return false;
            }

            //other attribute control types support values
            return true;
        }
    }

    /// <summary>
    /// A value indicating whether this attribute can be used as condition for some other attribute
    /// </summary>
    public bool CanBeUsedAsCondition
    {
        get
        {
            if (AttributeControlType == AttributeControlType.ReadonlyCheckboxes ||
                AttributeControlType == AttributeControlType.TextBox ||
                AttributeControlType == AttributeControlType.MultilineTextbox ||
                AttributeControlType == AttributeControlType.Datepicker ||
                AttributeControlType == AttributeControlType.FileUpload)
            {
                return false;
            }

            //other attribute control types support it
            return true;
        }
    }
}
