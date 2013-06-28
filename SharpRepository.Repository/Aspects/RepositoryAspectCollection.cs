using System.Collections.ObjectModel;

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
            var ret = true;
            foreach (var item in Items)
            {
                if (!item.OnAddExecuting(entity)) ret = false;
            }

            return ret;
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
            var ret = true;
            foreach (var item in Items)
            {
                if (!item.OnUpdateExecuting(entity)) ret = false;
            }

            return ret;
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
            var ret = true;
            foreach (var item in Items)
            {
                if (!item.OnDeleteExecuting(entity)) ret = false;
            }

            return ret;
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
            var ret = true;
            foreach (var item in Items)
            {
                if (!item.OnSaveExecuting()) ret = false;
            }

            return ret;
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
