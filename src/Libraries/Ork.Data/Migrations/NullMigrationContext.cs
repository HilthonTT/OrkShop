using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace Ork.Data.Migrations;

/// <summary>
/// Represents the migration context with a null implementation of a processor that does not do any work
/// </summary>
public sealed class NullMigrationContext : IMigrationContext
{
    public IServiceProvider ServiceProvider { get; set; } = default!;

    public ICollection<IMigrationExpression> Expressions { get; set; } = [];

    public IQuerySchema QuerySchema { get; set; } = default!;

    public string Connection { get; set; } = string.Empty;
}
