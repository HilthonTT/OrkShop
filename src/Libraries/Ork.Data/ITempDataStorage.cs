namespace Ork.Data;

public interface ITempDataStorage<T> : IQueryable<T>, IDisposable, IAsyncDisposable
    where T : class
{
}
