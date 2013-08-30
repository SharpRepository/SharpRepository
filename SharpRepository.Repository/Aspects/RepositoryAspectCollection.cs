using System.Collections.ObjectModel;
using System.Linq;

namespace SharpRepository.Repository.Aspects
{
    public class RepositoryAspectCollection<T, TKey> : Collection<IRepositoryAspect<T, TKey>>, IRepositoryAspect<T, TKey> where T : class
    {
        public void OnInitialize(IRepository<T, TKey> repository)
        {
            foreach (var item in Items)
            {
                item.OnInitialize(repository);
            }
        }

        public bool OnAddExecuting(T entity)
        {
            return Items.All(x => x.OnAddExecuting(entity));
        }

        public void OnAddExecuted(T entity)
        {
            foreach (var item in Items)
            {
                item.OnAddExecuted(entity);
            }
        }

        public bool OnUpdateExecuting(T entity)
        {
            return Items.All(x => x.OnUpdateExecuting(entity));
        }

        public void OnUpdateExecuted(T entity)
        {
            foreach (var item in Items)
            {
                item.OnUpdateExecuted(entity);
            }
        }

        public bool OnDeleteExecuting(T entity)
        {
            return Items.All(x => x.OnDeleteExecuting(entity));
        }

        public void OnDeleteExecuted(T entity)
        {
            foreach (var item in Items)
            {
                item.OnDeleteExecuted(entity);
            }
        }

        public bool OnSaveExecuting()
        {
            return Items.All(x => x.OnSaveExecuting());
        }

        public void OnSaveExecuted()
        {
            foreach (var item in Items)
            {
                item.OnSaveExecuted();
            }
        }
    }
}
