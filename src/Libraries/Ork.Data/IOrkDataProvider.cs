using LinqToDB.Data;
using Ork.Core;
using System.Linq.Expressions;

namespace Ork.Data;

/// <summary>
/// Defines methods for initializing, creating, and managing database and temporary data storage operations.
/// </summary>
/// <remarks>Implementations of this interface provide functionality for database setup, entity insertion, and
/// temporary storage management, supporting both synchronous and asynchronous operations. Methods may require callers
/// to handle resource management and ensure correct initialization order.</remarks>
public interface IOrkDataProvider
{
    /// <summary>
    /// Attempts to create a new database, retrying the connection if necessary.
    /// </summary>
    /// <param name="triesToConnect">The maximum number of times to attempt connecting to the database before giving up. Must be greater than zero.</param>
    void CreateDatabase(int triesToConnect = 10);

    /// <summary>
    /// Asynchronously creates a temporary data storage instance for the specified query and associates it with the
    /// given store key.
    /// </summary>
    /// <remarks>The returned temporary data storage is typically used for scenarios where intermediate query
    /// results need to be persisted for later retrieval, such as paging or caching. The caller is responsible for
    /// managing the lifecycle of the storage instance.</remarks>
    /// <typeparam name="TItem">The type of items to be stored in the temporary data storage. Must be a reference type.</typeparam>
    /// <param name="storeKey">A unique key that identifies the temporary data storage instance. Cannot be null or empty.</param>
    /// <param name="query">The queryable data source whose results will be stored in the temporary storage. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. The default value is None.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an instance of
    /// ITempDataStorage<TItem> for accessing the temporary data.</returns>
    Task<ITempDataStorage<TItem>> CreateTempDataStorageAsync<TItem>(
        string storeKey, 
        IQueryable<TItem> query,
        CancellationToken cancellationToken = default)
        where TItem : class;

    /// <summary>
    /// Initializes the database and prepares it for use.
    /// </summary>
    /// <remarks>Call this method before performing any database operations to ensure that the database is
    /// properly set up. This method may create necessary tables or schema if they do not exist.</remarks>
    void InitializeDatabase();

    /// <summary>
    /// Asynchronously inserts a new entity into the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to insert. Must inherit from BaseEntity.</typeparam>
    /// <param name="entity">The entity instance to insert. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation. The default value is None.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the inserted entity, including any
    /// updated properties such as generated keys.</returns>
    Task<TEntity> InserEntityAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity;

    /// <summary>
    /// Inserts the specified entity into the data store and returns the inserted entity.
    /// </summary>
    /// <remarks>The returned entity may include values set by the data store during insertion, such as
    /// identity fields or default values.</remarks>
    /// <typeparam name="TEntity">The type of the entity to insert. Must inherit from BaseEntity.</typeparam>
    /// <param name="entity">The entity to insert. Cannot be null.</param>
    /// <returns>The inserted entity, including any updated values such as generated keys or timestamps.</returns>
    TEntity InsertEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously updates the specified entity in the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to update. Must inherit from BaseEntity.</typeparam>
    /// <param name="entity">The entity instance to update. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateEntityAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Updates the specified entity in the underlying data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to update. Must inherit from BaseEntity.</typeparam>
    /// <param name="entity">The entity instance to update. Cannot be null.</param>
    void UpdateEntity<TEntity>(TEntity entity) where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously updates the specified collection of entities in the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to update. Must inherit from BaseEntity.</typeparam>
    /// <param name="entities">The collection of entities to update. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateEntitiesAsync<TEntity>(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default) where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously deletes the specified entity from the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to delete. Must inherit from BaseEntity.</typeparam>
    /// <param name="entity">The entity instance to delete. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteEntityAsync<TEntity>(
        TEntity entity, 
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity;

    /// <summary>
    /// Performs delete records in a table
    /// </summary>
    /// <param name="entities">Entities for delete operation</param>
    /// <typeparam name="TEntity">Entity type</typeparam>
    void BulkDeleteEntities<TEntity>(IList<TEntity> entities) 
        where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously deletes all entities of type TEntity that match the specified predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entities to delete. Must inherit from BaseEntity.</typeparam>
    /// <param name="predicate">An expression that specifies the conditions used to select entities to delete.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities deleted.</returns>
    Task<int> BulkDeleteEntitiesAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously deletes a collection of entities in bulk.
    /// </summary>
    /// <remarks>Bulk deletion may improve performance compared to deleting entities individually. The
    /// operation is not guaranteed to be atomic; some entities may be deleted even if the operation is canceled or
    /// fails partway through.</remarks>
    /// <typeparam name="TEntity">The type of entities to delete. Must inherit from BaseEntity.</typeparam>
    /// <param name="entities">The list of entities to be deleted. Cannot be null or contain null elements.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous bulk delete operation.</returns>
    Task BulkDeleteEntitiesAsync<TEntity>(
        List<TEntity> entities,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously inserts a collection of entities into the data store in a single bulk operation.
    /// </summary>
    /// <remarks>Bulk insertion is typically more efficient than inserting entities individually. The order of
    /// entities in the collection may not be preserved in the data store.</remarks>
    /// <typeparam name="TEntity">The type of entities to insert. Must inherit from BaseEntity.</typeparam>
    /// <param name="entities">The collection of entities to insert. Cannot be null or contain null elements.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous bulk insert operation.</returns>
    Task BulkInsertEntitiesAsync<TEntity>(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default) where TEntity : BaseEntity;

    /// <summary>
    /// Inserts a collection of entities into the data store in a single bulk operation.
    /// </summary>
    /// <remarks>Bulk insertion is typically more efficient than inserting entities individually. The order of
    /// entities in the collection may not be preserved in the data store.</remarks>
    /// <typeparam name="TEntity">The type of entities to insert. Must inherit from BaseEntity.</typeparam>
    /// <param name="entities">The collection of entities to insert. Cannot be null or contain null elements.</param>
    void BulkInsertEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;

    /// <summary>
    /// Generates a foreign key constraint name based on the specified foreign and primary table and column names.
    /// </summary>
    /// <param name="foreignTable">The name of the table containing the foreign key column. Cannot be null or empty.</param>
    /// <param name="foreignColumn">The name of the foreign key column in the foreign table. Cannot be null or empty.</param>
    /// <param name="primaryTable">The name of the table referenced by the foreign key. Cannot be null or empty.</param>
    /// <param name="primaryColumn">The name of the primary key column in the referenced table. Cannot be null or empty.</param>
    /// <returns>A string representing the generated foreign key constraint name.</returns>
    string CreateForeignKeyName(string foreignTable, string foreignColumn, string primaryTable, string primaryColumn);

    /// <summary>
    /// Gets the name of the index associated with the specified table and column.
    /// </summary>
    /// <param name="targetTable">The name of the table containing the target column. Cannot be null or empty.</param>
    /// <param name="targetColumn">The name of the column for which to retrieve the index name. Cannot be null or empty.</param>
    /// <returns>The name of the index for the specified table and column. Returns null if no index exists for the given
    /// combination.</returns>
    string GetIndexName(string targetTable, string targetColumn);

    /// <summary>
    /// Returns a queryable collection of entities of the specified type for querying and data manipulation operations.
    /// </summary>
    /// <remarks>The returned IQueryable supports deferred execution and can be further filtered or projected
    /// using LINQ methods. Changes to the returned query do not affect the underlying data until executed.</remarks>
    /// <typeparam name="TEntity">The type of entity to retrieve. Must inherit from BaseEntity.</typeparam>
    /// <returns>An IQueryable of TEntity that can be used to query and manipulate entities in the data store.</returns>
    IQueryable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously retrieves the current identity value for the table associated with the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity for which to retrieve the table identity value. Must inherit from BaseEntity.</typeparam>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the current identity value for the
    /// table, or null if the table does not have an identity column.</returns>
    Task<int?> GetTableIdentAsync<TEntity>(CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously determines whether the database exists and is accessible.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
    /// database exists; otherwise, <see langword="false"/>.</returns>
    Task<bool> DatabaseExistsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether the database currently exists.
    /// </summary>
    /// <returns>true if the database exists; otherwise, false.</returns>
    bool DatabaseExists();

    /// <summary>
    /// Asynchronously creates a backup of the database and saves it to the specified file.
    /// </summary>
    /// <param name="filename">The path and file name where the database backup will be saved. Cannot be null or empty.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the backup operation.</param>
    /// <returns>A task that represents the asynchronous backup operation.</returns>
    Task BackupDatabaseAsync(string filename, CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores the database from the specified backup file asynchronously.
    /// </summary>
    /// <remarks>The database will be replaced with the contents of the backup file. Any existing data will be
    /// overwritten. This operation may take significant time depending on the size of the backup file.</remarks>
    /// <param name="backupFilename">The path to the backup file to restore from. The file must exist and be accessible by the application.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the restore operation.</param>
    /// <returns>A task that represents the asynchronous restore operation.</returns>
    Task RestoreDatabaseAsync(string backupFilename, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously reindexes all relevant database tables to improve query performance and maintain data integrity.
    /// </summary>
    /// <remarks>This method should be called during maintenance windows or when significant changes to table
    /// data have occurred. The operation may be resource-intensive and could temporarily impact database
    /// performance.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the reindexing operation. The default value is <see
    /// cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous reindexing operation.</returns>
    Task ReIndexTablesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously shrinks the database to reclaim unused space.
    /// </summary>
    /// <remarks>Shrinking the database may impact performance and should be used judiciously in production
    /// environments. The operation reclaims unused space but may cause fragmentation.</remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the shrink operation. The default value is <see
    /// cref="CancellationToken.None"/>.</param>
    /// <returns>A task that represents the asynchronous shrink operation.</returns>
    Task ShrinkDatabaseAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the total size of the database, in bytes.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the total size of the database, in
    /// bytes.</returns>
    Task<long> GetDatabaseSizeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously sets the identity seed value for the specified table entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the table entity. Must inherit from BaseEntity.</typeparam>
    /// <param name="ident">The new identity seed value to set for the table. Must be a non-negative integer.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetTableIdentAsync<TEntity>(int ident, CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously retrieves a dictionary mapping entity identifiers to hash values for the specified fields of
    /// entities that match the given predicate.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity. Must inherit from BaseEntity.</typeparam>
    /// <param name="predicate">An expression that defines the filter criteria to select entities. Only entities matching this predicate are
    /// included.</param>
    /// <param name="fieldSelector">An expression that specifies the field or fields for which to compute hash values.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary where each key is the
    /// identifier of an entity and each value is the hash of the selected field(s).</returns>
    Task<Dictionary<int, string>> GetFieldHashesAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>> fieldSelector) 
        where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously executes a SQL statement against the database and returns the number of rows affected.
    /// </summary>
    /// <remarks>This method does not return any result sets. Use this method for commands that modify data
    /// but do not return rows. If the command executes successfully but affects no rows, the result is 0.</remarks>
    /// <param name="sql">The SQL statement to execute. This statement can be an INSERT, UPDATE, DELETE, or any statement that does not
    /// return rows.</param>
    /// <param name="dataParameters">An optional array of parameters to be applied to the SQL statement. Each parameter represents a value to be
    /// substituted in the command.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of rows affected by the
    /// command.</returns>
    Task<int> ExecuteNonQueryAsync(string sql, params DataParameter[] dataParameters);

    /// <summary>
    /// Executes a stored procedure asynchronously and returns the result set as a list of objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of objects to map each row of the result set to.</typeparam>
    /// <param name="procedureName">The name of the stored procedure to execute. Cannot be null or empty.</param>
    /// <param name="parameters">An optional array of parameters to pass to the stored procedure.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of objects of type T mapped
    /// from the result set. The list will be empty if the procedure returns no rows.</returns>
    Task<List<T>> QueryProcAsync<T>(string procedureName, params DataParameter[] parameters);

    /// <summary>
    /// Executes the specified SQL query asynchronously and returns a list of mapped entities of type T.
    /// </summary>
    /// <typeparam name="T">The type of the entities to map the query results to.</typeparam>
    /// <param name="sql">The SQL query to execute. The query should be valid for the underlying database provider.</param>
    /// <param name="parameters">An optional array of parameters to apply to the SQL query. Each parameter represents a value to be substituted
    /// in the query.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities of type T mapped
    /// from the query results. The list will be empty if no records are returned.</returns>
    Task<List<T>> QueryAsync<T>(string sql, params DataParameter[] parameters);

    /// <summary>
    /// Asynchronously removes all records from the table associated with the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity whose table will be truncated. Must inherit from BaseEntity.</typeparam>
    /// <param name="resetIdentity">true to reset the identity seed for the table; otherwise, false.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of rows affected.</returns>
    Task<int> TruncateAsync<TEntity>(bool resetIdentity)
        where TEntity : BaseEntity;

    /// <summary>
    /// Asynchronously retrieves the collation setting for the connected database.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collation name of the database
    /// as a string.</returns>
    Task<string> GetDataBaseCollationAsync(CancellationToken cancellationToken = default);
}
