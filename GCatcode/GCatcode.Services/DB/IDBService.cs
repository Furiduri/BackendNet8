namespace GCatcode.Repository.DB
{
    public interface IDBService<T>
    {
        T? GetById(int id);

        List<T> Get(int maxItems = 100, object? filters = null);

        void Delete(int id);
    }
}