using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Expressions;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;
using Ork.Core;
using Ork.Core.Infrastructure;
using Ork.Data.Extensions;
using Ork.Data.Migrations;
using System.Collections.Concurrent;

namespace Ork.Data.Mapping;

public static class OrkMappingSchema
{
    private static ConcurrentDictionary<Type, OrkEntityDescriptor> EntityDescriptors { get; } = new();

    public static OrkEntityDescriptor? GetEntityDescriptor(Type entityType)
    {
        if (!(typeof(BaseEntity).IsAssignableFrom(entityType)))
        {
            return null;
        }

        return EntityDescriptors.GetOrAdd(entityType, t =>
        {
            var tableName = NameCompatibilityManager.GetTableName(t);
            var expression = new CreateTableExpression { TableName = tableName };
            var builder = new CreateTableExpressionBuilder(expression, new NullMigrationContext());
            builder.RetrieveTableExpressions(t);

            return new OrkEntityDescriptor
            {
                EntityName = tableName,
                SchemaName = builder.Expression.SchemaName,
                Fields = builder.Expression.Columns.Select(column => new OrkEntityFieldDescriptor
                {
                    Name = column.Name,
                    IsPrimaryKey = column.IsPrimaryKey,
                    IsNullable = column.IsNullable,
                    Size = column.Size,
                    Precision = column.Precision,
                    IsIdentity = column.IsIdentity,
                    Type = column.Type ?? System.Data.DbType.String
                }).ToList()
            };
        });
    }

    public static MappingSchema GetMappingSchema(string configurationName, IDataProvider mappings)
    {

        if (Singleton<MappingSchema>.Instance is null)
        {
            Singleton<MappingSchema>.Instance = new MappingSchema(configurationName, mappings.MappingSchema);
            Singleton<MappingSchema>.Instance.AddMetadataReader(new FluentMigratorMetadataReader());
        }

        return Singleton<MappingSchema>.Instance;
    }
}
