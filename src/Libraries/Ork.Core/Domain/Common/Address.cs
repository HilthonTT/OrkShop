namespace Ork.Core.Domain.Common;

public sealed class Address : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public int? CountryId { get; set; }

    public int? StateProvinceId { get; set; }

    public string County { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Address1 { get; set; } = string.Empty;

    public string Address2 { get; set; } = string.Empty;

    public string ZipPostalCode { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string FaxNumber { get; set; } = string.Empty;


    /// <summary>
    /// Gets or sets the custom attributes (see "AddressAttribute" entity for more info)
    /// </summary>
    public string CustomAttributes { get; set; } = string.Empty;

    public DateTime CreatedOnUtc { get; set; }
}
