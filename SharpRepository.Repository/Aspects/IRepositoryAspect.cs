namespace SharpRepository.Repository.Aspects
{
    public interface IRepositoryAspect<T, TKey> where T : class
    {
        // TODO: add hooks for queries as well

        void OnInitialize(IRepository<T, TKey> repository);

        bool OnAddExecuting(T entity);
        void OnAddExecuted(T entity);

        bool OnUpdateExecuting(T entity);
        void OnUpdateExecuted(T entity);

        bool OnDeleteExecuting(T entity);
        void OnDeleteExecuted(T entity);

        bool OnSaveExecuting();
        void OnSaveExecuted();
    }
}
