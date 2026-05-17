using System.Data;

namespace Ork.Data.Mapping;

public sealed class OrkEntityFieldDescriptor
{
    public string Name { get; set; } = string.Empty;
    public bool IsIdentity { get; set; }

    public bool? IsNullable { get; set; }

    public bool IsPrimaryKey { get; set; }

    public bool IsUnique { get; set; }

    public int? Precision { get; set; }

    public int? Size { get; set; }
    
    public DbType Type { get; set; }
}