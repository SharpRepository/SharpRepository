namespace SharpRepository.Repository.Transactions
{
    public interface IBatchItem<T>
    {
        BatchAction Action { get; set; }
        T Item { get; set; }
    }
}
