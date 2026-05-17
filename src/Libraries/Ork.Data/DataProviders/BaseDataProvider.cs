using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using Ork.Data.Configuration;
using Ork.Data.Mapping;
using System.Data.Common;

namespace Ork.Data.DataProviders;

public abstract class BaseDataProvider
{
    protected abstract IDataProvider LinqToDbDataProvider { get; }

    protected static DataConfig DataSettings => DataSettingsManager.LoadSettings() ?? new DataConfig();

    public string ConfigurationName => LinqToDbDataProvider.Name;

    protected virtual BulkCopyOptions CreateBulkCopyOptions()
    {
        return new BulkCopyOptions
        {
            CheckConstraints = DataSettings.BulkCopyWithCheckConstraints,
            KeepIdentity = true
        };
    }

    protected abstract DbConnection GetInternalDbConnection(string connectionString);

    protected virtual DataConnection CreateDataConnection()
    {
        return CreateDataConnection(LinqToDbDataProvider);
    }

    protected virtual DataConnection CreateDataConnection(IDataProvider dataProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProvider, nameof(dataProvider));

        var options = new DataOptions()
            .UseConnection(dataProvider, CreateDbConnection())
            .UseMappingSchema(OrkMappingSchema.GetMappingSchema(ConfigurationName, LinqToDbDataProvider));

        var dataConnection = new DataConnection(options);

        var sqlCommandTimeout = DataSettings.SQLCommandTimeout ?? -1;
        if (sqlCommandTimeout == -1)
        {
            dataConnection.ResetCommandTimeout();
        }
        else
        {
            dataConnection.CommandTimeout = sqlCommandTimeout;
        }

        return dataConnection;
    }

    protected virtual DbConnection CreateDbConnection(string? connectionString = null)
    {
        return GetInternalDbConnection(!string.IsNullOrEmpty(connectionString) ? connectionString : DataSettings.ConnectionString);
    }

    protected virtual async Task<string> GetSqlStringValueAsync(string sql, params DataParameter[] parameters)
    {
        ArgumentException.ThrowIfNullOrEmpty(sql, nameof(sql));

        await using var dbConnection = CreateDbConnection();
        await using var command = dbConnection.CreateCommand();
        command.Connection = dbConnection;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);
        await dbConnection.OpenAsync();

        var value = await command.ExecuteScalarAsync();

        return value?.ToString() ?? string.Empty;
    }

    [Sql.Expression("CONVERT(VARCHAR(128), HASHBYTES('SHA2_512', SUBSTRING({0}, 0, 8000)), 2)", ServerSideOnly = true, Configuration = ProviderName.SqlServer)]
    [Sql.Expression("SHA2({0}, 512)", ServerSideOnly = true, Configuration = ProviderName.MySql)]
    [Sql.Expression("encode(digest({0}, 'sha512'), 'hex')", ServerSideOnly = true, Configuration = ProviderName.PostgreSQL)]
    protected static string SqlSha2(object binaryData)
    {
        throw new InvalidOperationException("This function should be used only in database code");
    }
}
