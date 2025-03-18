namespace GCatcode.Repository.DB
{
    internal interface IDBService<Table, TInsert, TUpdate>
    {
        Table? GetById(int id);

        IEnumerable<Table> Get(int maxItems = 100, int page = 1, object? filters = null);

        Table Delete(int id);

        Table Update(TUpdate data);

        Table Insert(TInsert data);
    }
}