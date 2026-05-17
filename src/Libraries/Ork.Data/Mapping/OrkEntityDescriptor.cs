namespace Ork.Data.Mapping;

public sealed class OrkEntityDescriptor
{
    public string EntityName { get; set; } = string.Empty;
    
    public string SchemaName { get; set; } = string.Empty;

    public ICollection<OrkEntityFieldDescriptor> Fields { get; set; } = [];
}
