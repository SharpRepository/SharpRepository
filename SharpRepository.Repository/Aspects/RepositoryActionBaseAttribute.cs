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


        #region INIT
        public virtual void OnInitialized<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnInitialized<T, TKey, TKey2>(RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
        }

        public virtual void OnInitialized<T, TKey, TKey2, TKey3>(RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
        }

        public virtual void OnInitialized<T>(RepositoryActionContext<T> context) where T : class
        {
        }
        #endregion

        #region ERROR
        public virtual void OnError<T, TKey>(RepositoryActionContext<T, TKey> context, Exception ex) where T : class
        {
        }

        public virtual void OnError<T, TKey, TKey2>(RepositoryActionContext<T, TKey, TKey2> context, Exception ex) where T : class
        {
        }

        public virtual void OnError<T, TKey, TKey2, TKey3>(RepositoryActionContext<T, TKey, TKey2, TKey3> context, Exception ex) where T : class
        {
        }

        public virtual void OnError<T>(RepositoryActionContext<T> context, Exception ex) where T : class
        {
        }
        #endregion

        #region ADD
        public virtual bool OnAddExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual bool OnAddExecuting<T, TKey, TKey2>(T entity, RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
            return true;
        }
        public virtual bool OnAddExecuting<T, TKey, TKey2, TKey3>(T entity, RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
            return true;
        }
        public virtual bool OnAddExecuting<T>(T entity, RepositoryActionContext<T> context) where T : class
        {
            return true;
        }

        public virtual void OnAddExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnAddExecuted<T, TKey, TKey2>(T entity, RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
        }

        public virtual void OnAddExecuted<T, TKey, TKey2, TKey3>(T entity, RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
        }

        public virtual void OnAddExecuted<T>(T entity, RepositoryActionContext<T> context) where T : class
        {
        }
        #endregion

        #region UPDATE
        public virtual bool OnUpdateExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }
        public virtual bool OnUpdateExecuting<T, TKey, TKey2>(T entity, RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
            return true;
        }
        public virtual bool OnUpdateExecuting<T, TKey, TKey2, TKey3>(T entity, RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
            return true;
        }
        public virtual bool OnUpdateExecuting<T>(T entity, RepositoryActionContext<T> context) where T : class
        {
            return true;
        }

        public virtual void OnUpdateExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
        }

        public virtual void OnUpdateExecuted<T, TKey, TKey2>(T entity, RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
        }

        public virtual void OnUpdateExecuted<T, TKey, TKey2, TKey3>(T entity, RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
        }

        public virtual void OnUpdateExecuted<T>(T entity, RepositoryActionContext<T> context) where T : class
        {
        }
        #endregion

        #region DELETE
        public virtual bool OnDeleteExecuting<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }
        public virtual bool OnDeleteExecuting<T, TKey, TKey2>(T entity, RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
            return true;
        }
        public virtual bool OnDeleteExecuting<T, TKey, TKey2, TKey3>(T entity, RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
            return true;
        }
        public virtual bool OnDeleteExecuting<T>(T entity, RepositoryActionContext<T> context) where T : class
        {
            return true;
        }

        public virtual void OnDeleteExecuted<T, TKey>(T entity, RepositoryActionContext<T, TKey> context) where T : class
        {
        }
        public virtual void OnDeleteExecuted<T, TKey, TKey2>(T entity, RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
        }
        public virtual void OnDeleteExecuted<T, TKey, TKey2, TKey3>(T entity, RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
        }
        public virtual void OnDeleteExecuted<T>(T entity, RepositoryActionContext<T> context) where T : class
        {
        }
        #endregion DELETE

        #region SAVE
        public virtual bool OnSaveExecuting<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
            return true;
        }
        public virtual bool OnSaveExecuting<T, TKey, TKey2>(RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
            return true;
        }
        public virtual bool OnSaveExecuting<T, TKey, TKey2, TKey3>(RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
            return true;
        }
        public virtual bool OnSaveExecuting<T>(RepositoryActionContext<T> context) where T : class
        {
            return true;
        }

        public virtual void OnSaveExecuted<T, TKey>(RepositoryActionContext<T, TKey> context) where T : class
        {
        }
        public virtual void OnSaveExecuted<T, TKey, TKey2>(RepositoryActionContext<T, TKey, TKey2> context) where T : class
        {
        }
        public virtual void OnSaveExecuted<T, TKey, TKey2, TKey3>(RepositoryActionContext<T, TKey, TKey2, TKey3> context) where T : class
        {
        }
        public virtual void OnSaveExecuted<T>(RepositoryActionContext<T> context) where T : class
        {
        }
        #endregion

        #region GET
        public virtual bool OnGetExecuting<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnGetExecuting<T, TKey, TKey2, TResult>(CompoundKeyRepositoryGetContext<T, TKey, TKey2, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnGetExecuting<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryGetContext<T, TKey, TKey2, TKey3, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnGetExecuting<T, TResult>(CompoundKeyRepositoryGetContext<T, TResult> context) where T : class
        {
            return true;
        }

        public virtual void OnGetExecuted<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context) where T : class
        {
        }
        public virtual void OnGetExecuted<T, TKey, TKey2, TResult>(CompoundKeyRepositoryGetContext<T, TKey, TKey2, TResult> context) where T : class
        {
        }
        public virtual void OnGetExecuted<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryGetContext<T, TKey, TKey2, TKey3, TResult> context) where T : class
        {
        }
        public virtual void OnGetExecuted<T, TResult>(RepositoryGetContext<T, TResult> context) where T : class
        {
        }

        public virtual bool OnGetAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnGetAllExecuting<T, TKey, TKey2, TResult>(CompoundKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnGetAllExecuting<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TKey3, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnGetAllExecuting<T, TResult>(CompoundKeyRepositoryQueryMultipleContext<T, TResult> context) where T : class
        {
            return true;
        }

        public virtual void OnGetAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnGetAllExecuted<T, TKey, TKey2, TResult>(CompoundKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TResult> context) where T : class
        {
        }

        public virtual void OnGetAllExecuted<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TKey3, TResult> context) where T : class
        {
        }

        public virtual void OnGetAllExecuted<T, TResult>(CompoundKeyRepositoryQueryMultipleContext<T, TResult> context) where T : class
        {
        }
        #endregion

        #region FIND
        public virtual bool OnFindExecuting<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnFindExecuting<T, TKey, TKey2, TResult>(CompoundKeyRepositoryQuerySingleContext<T, TKey, TKey2, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnFindExecuting<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryQuerySingleContext<T, TKey, TKey2, TKey3, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnFindExecuting<T, TResult>(CompoundKeyRepositoryQuerySingleContext<T, TResult> context) where T : class
        {
            return true;
        }

        public virtual void OnFindExecuted<T, TKey, TResult>(RepositoryQuerySingleContext<T, TKey, TResult> context) where T : class
        {
        }

        public virtual void OnFindExecuted<T, TKey, TKey2, TResult>(CompoundKeyRepositoryQuerySingleContext<T, TKey, TKey2, TResult> context) where T : class
        {
        }

        public virtual void OnFindExecuted<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryQuerySingleContext<T, TKey, TKey2, TKey3, TResult> context) where T : class
        {
        }

        public virtual void OnFindExecuted<T, TResult>(CompoundKeyRepositoryQuerySingleContext<T, TResult> context) where T : class
        {
        }

        public virtual bool OnFindAllExecuting<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnFindAllExecuting<T, TKey, TKey2, TResult>(CompoundKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnFindAllExecuting<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TKey3, TResult> context) where T : class
        {
            return true;
        }

        public virtual bool OnFindAllExecuting<T, TResult>(CompoundKeyRepositoryQueryMultipleContext<T, TResult> context) where T : class
        {
            return true;
        }

        public virtual void OnFindAllExecuted<T, TKey, TResult>(RepositoryQueryMultipleContext<T, TKey, TResult> context) where T : class
        {
        }
        public virtual void OnFindAllExecuted<T, TKey, TKey2, TResult>(CompoundKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TResult> context) where T : class
        {
        }
        public virtual void OnFindAllExecuted<T, TKey, TKey2, TKey3, TResult>(CompoundTripleKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TKey3, TResult> context) where T : class
        {
        }
        public virtual void OnFindAllExecuted<T, TResult>(CompoundKeyRepositoryQueryMultipleContext<T, TResult> context) where T : class
        {
        }
        #endregion
    }
}
