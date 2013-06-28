
namespace SharpRepository.Repository.Aspects
{
    public abstract class RepositoryAspect<T, TKey> : IRepositoryAspect<T, TKey> where T : class
    {
        public virtual void OnInitialize(IRepository<T, TKey> repository)
        {
            
        }

        public virtual bool OnAddExecuting(T entity)
        {
            return true;
        }

        public virtual void OnAddExecuted(T entity)
        {
            
        }

        public virtual bool OnUpdateExecuting(T entity)
        {
            return true;
        }

        public virtual void OnUpdateExecuted(T entity)
        {
            
        }

        public virtual bool OnDeleteExecuting(T entity)
        {
            return true;
        }

        public virtual void OnDeleteExecuted(T entity)
        {
            
        }

        public virtual bool OnSaveExecuting()
        {
            return true;
        }

        public virtual void OnSaveExecuted()
        {
            
        }
    }
}
