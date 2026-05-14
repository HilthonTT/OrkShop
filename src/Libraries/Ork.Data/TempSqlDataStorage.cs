using LinqToDB;

namespace Ork.Data;

/// <summary>
/// Represents temporary storage.
/// </summary>
/// <typeparam name="T">The storage record mapping class.</typeparam>
internal sealed class TempSqlDataStorage<T> : TempTable<T>, ITempDataStorage<T> where T : class
{
    public TempSqlDataStorage(
        string storageName, 
        IQueryable<T> query, 
        IDataContext dataConnection)
        : base(dataConnection, storageName, query, tableOptions: TableOptions.NotSet | TableOptions.CheckExistence)
    {
        dataConnection.CloseAfterUse = true;
    }
}
