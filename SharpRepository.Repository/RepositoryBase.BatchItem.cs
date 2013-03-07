using SharpRepository.Repository.Transactions;

namespace SharpRepository.Repository
{
    public abstract partial class RepositoryBase<T, TKey> where T : class
    {
        private sealed class BatchItem : IBatchItem<T>
        {
            public BatchAction Action { get; set; }
            public T Item { get; set; }
        }
    }
}
