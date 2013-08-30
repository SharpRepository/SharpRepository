using System;

namespace SharpRepository.Repository.Aspects
{
//    public interface IRepositoryActionAttribute
//    {
//        bool OnAddExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class;
//        void OnAddExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class;
//        
//        bool OnUpdateExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class;
//        void OnUpdateExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class;
//
//        bool OnDeleteExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class;
//        void OnDeleteExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class;
//
//        bool OnSaveExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class;
//        void OnSaveExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class;
//
//    }

    public abstract class RepositoryActionBaseAttribute : Attribute
    {
        public virtual void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {

        }

        public virtual bool OnAddExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnAddExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnUpdateExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnUpdateExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnDeleteExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnDeleteExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnSaveExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnSaveExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }
    }

    public class RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryActionContext(T entity, IRepository<T, TKey> repository)
        {
            Entity = entity;
            Repository = repository;
        }

        public T Entity { get; set; }
        public IRepository<T, TKey> Repository { get; set; }
    }
}
