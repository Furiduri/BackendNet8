namespace GCatcode.Utils.ChangeHistory
{
    public class ChangeHistoryData<T>
    {
        public int Id { get; set; }
        public T OriginalData { get; set; }
        public T UpdatedData { get; set; }
    }
}