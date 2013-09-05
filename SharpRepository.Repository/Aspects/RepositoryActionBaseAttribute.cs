using System;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Aspects
{
    public abstract class RepositoryActionBaseAttribute : Attribute
    {
        public virtual void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnAddExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnAddExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnUpdateExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnUpdateExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnDeleteExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnDeleteExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnSaveExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnSaveExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        /* Queries */
        public virtual void OnGetExecuting<T, TKey>(RepositoryGetContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnGetExecuted<T, TKey>(RepositoryGetContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnGetAllExecuting<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnGetAllExecuted<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnFindExecuting<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnFindExecuted<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnFindAllExecuting<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnFindAllExecuted<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
        }
    }
}
