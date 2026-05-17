using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.Metadata;
using Ork.Core;
using System.Collections.Concurrent;
using System.Data;
using System.Reflection;

namespace Ork.Data.Mapping;

public sealed class FluentMigratorMetadataReader : IMetadataReader
{
    private static readonly ConcurrentDictionary<(Type, MemberInfo?), MappingAttribute?> Types = [];

    public MappingAttribute[] GetAttributes(Type type)
    {
        if (!type.IsSubclassOf(typeof(BaseEntity)))
        {
            return [];
        }

        var attribute = Types.GetOrAdd((type, null), _ =>
        {
            var entityDescriptor = OrkMappingSchema.GetEntityDescriptor(type);

            if (entityDescriptor is null)
            {
                return null;
            }

            return new TableAttribute() { Schema = entityDescriptor.SchemaName, Name = entityDescriptor.EntityName };
        });

        if (attribute is null)
        {
            return [];
        }

        return [attribute];
    }

    public MappingAttribute[] GetAttributes(Type type, MemberInfo memberInfo)
    {
        throw new NotImplementedException();
    }

    public MemberInfo[] GetDynamicColumns(Type type)
    {
        throw new NotImplementedException();
    }

    public string GetObjectID()
    {
        throw new NotImplementedException();
    }

    private static DataType DbTypeToDataType(DbType dbType)
    {
        return dbType switch
        {
            DbType.AnsiString => DataType.VarChar,
            DbType.AnsiStringFixedLength => DataType.VarChar,
            DbType.Binary => DataType.Binary,
            DbType.Boolean => DataType.Boolean,
            DbType.Byte => DataType.Byte,
            DbType.Currency => DataType.Money,
            DbType.Date => DataType.Date,
            DbType.DateTime => DataType.DateTime,
            DbType.DateTime2 => DataType.DateTime2,
            DbType.DateTimeOffset => DataType.DateTimeOffset,
            DbType.Decimal => DataType.Decimal,
            DbType.Double => DataType.Double,
            DbType.Guid => DataType.Guid,
            DbType.Int16 => DataType.Int16,
            DbType.Int32 => DataType.Int32,
            DbType.Int64 => DataType.Int64,
            DbType.Object => DataType.Undefined,
            DbType.SByte => DataType.SByte,
            DbType.Single => DataType.Single,
            DbType.String => DataType.NVarChar,
            DbType.StringFixedLength => DataType.NVarChar,
            DbType.Time => DataType.Time,
            DbType.UInt16 => DataType.UInt16,
            DbType.UInt32 => DataType.UInt32,
            DbType.UInt64 => DataType.UInt64,
            DbType.VarNumeric => DataType.VarNumeric,
            DbType.Xml => DataType.Xml,
            _ => DataType.Undefined
        };
    }
}
