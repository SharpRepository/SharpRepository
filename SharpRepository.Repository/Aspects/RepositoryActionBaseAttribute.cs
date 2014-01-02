using System;

namespace SharpRepository.Repository.Aspects
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public abstract class RepositoryActionBaseAttribute : Attribute
    {
        protected RepositoryActionBaseAttribute()
        {
            Enabled = true;
            Order = 9999; // high number so if they don't set it, it happens last
        }

        public int Order { get; set; }
        public bool Enabled { get; set; }

        public virtual void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnError<T, TKey>(RepositoryActionContext<T, TKey> context, Exception ex) where T : class
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
        public virtual void OnGetExecuting<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnGetExecuted<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnGetAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnGetAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnFindExecuting<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnFindExecuted<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnFindAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnFindAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context) where T : class
        {
        }
    }
}
