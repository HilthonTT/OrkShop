using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Infrastructure.Extensions;
using FluentMigrator.Model;
using LinqToDB.Mapping;
using Ork.Core;
using Ork.Core.Infrastructure;
using Ork.Data.Configuration;
using Ork.Data.Mapping;
using Ork.Data.Mapping.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Ork.Data.Extensions;

public static class FluentMigratorExtensions
{
    private const int DateTimePrecision = 6;

    private static Dictionary<Type, Action<ICreateTableColumnAsTypeSyntax>> TypeMapping { get; } = new Dictionary<Type, Action<ICreateTableColumnAsTypeSyntax>>
    {
        [typeof(int)] = c => c.AsInt32(),
        [typeof(long)] = c => c.AsInt64(),
        [typeof(string)] = c => c.AsString(int.MaxValue).Nullable(),
        [typeof(bool)] = c => c.AsBoolean(),
        [typeof(decimal)] = c => c.AsDecimal(18, 4),
        [typeof(DateTime)] = c => c.AsOrkDateTime2(),
        [typeof(byte[])] = c => c.AsBinary(int.MaxValue),
        [typeof(Guid)] = c => c.AsGuid()
    };

    public static void RetrieveTableExpressions(this CreateTableExpressionBuilder builder, Type type)
    {
        Type? typeFinder = Singleton<ITypeFinder>.Instance?
            .FindClassesOfType(typeof(IEntityBuilder))
            .FirstOrDefault(t => t.BaseType?.GetGenericArguments().Contains(type) ?? false);

        if (typeFinder is not null)
        {
            (EngineContext.Current.ResolveUnregistered(typeFinder) as IEntityBuilder)?.MapEntity(builder);
        }

        var expression = builder.Expression;
        if (!expression.Columns.Any(c => c.IsPrimaryKey))
        {
            var pk = new ColumnDefinition
            {
                Name = nameof(BaseEntity.Id),
                Type = DbType.Int32,
                IsIdentity = true,
                TableName = NameCompatibilityManager.GetTableName(type),
                ModificationType = ColumnModificationType.Create,
                IsPrimaryKey = true
            };
            expression.Columns.Insert(0, pk);
            builder.CurrentColumn = pk;
        }

        var propertiesToAutoMap = type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
            .Where(pi => 
                pi.DeclaringType != typeof(BaseEntity) &&
                pi.CanWrite &&
                !pi.HasAttribute<NotMappedAttribute>() && !pi.HasAttribute<NotColumnAttribute>() &&
                !expression.Columns.Any(x => x.Name.Equals(NameCompatibilityManager.GetColumnName(type, pi.Name), StringComparison.OrdinalIgnoreCase)) &&
                TypeMapping.ContainsKey(getTypeToMap(pi.PropertyType).propType));

        foreach (var prop in propertiesToAutoMap)
        {
            var columnName = NameCompatibilityManager.GetColumnName(type, prop.Name);
            var (propType, canBeNullable) = getTypeToMap(prop.PropertyType);
            DefineByOwnType(columnName, propType, builder, canBeNullable);
        }

        return;

        (Type propType, bool canBeNullable) getTypeToMap(Type typeToMap)
        {
            if (Nullable.GetUnderlyingType(typeToMap) is { } uType)
                return (uType, true);

            return (typeToMap, false);
        }
    }

    public static ICreateTableColumnOptionOrWithColumnSyntax AsOrkDateTime2(this ICreateTableColumnAsTypeSyntax syntax)
    {
        DataConfig dataSettings = DataSettingsManager.LoadSettings() ?? new DataConfig();

        return dataSettings.DataProvider switch
        {
            DataProviderType.MySql => syntax.AsCustom($"datetime({DateTimePrecision})"),
            DataProviderType.SqlServer => syntax.AsCustom($"datetime2({DateTimePrecision})"),
            _ => syntax.AsDateTime2()
        };
    }

    private static void DefineByOwnType(
        string columnName, 
        Type propType, 
        CreateTableExpressionBuilder create, 
        bool canBeNullable = false)
    {
        if (string.IsNullOrEmpty(columnName))
        {
            throw new ArgumentException("The column name cannot be empty");
        }

        if (propType == typeof(string) || 
            propType.FindInterfaces((t, o) => t.FullName?.Equals(o?.ToString(), StringComparison.InvariantCultureIgnoreCase) ?? false, "System.Collections.IEnumerable").Length > 0)
        {
            canBeNullable = true;
        }

        var column = create.WithColumn(columnName);

        TypeMapping[propType](column);

        if (propType == typeof(DateTime))
        {
            create.CurrentColumn.Precision = DateTimePrecision;
        }

        if (canBeNullable)
        {
            create.Nullable();
        }
    }
}
