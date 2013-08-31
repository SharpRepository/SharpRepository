using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

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
        public virtual bool OnGetExecuting<T, TKey>(RepositoryGetContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnGetExecuted<T, TKey>(RepositoryGetContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnGetAllExecuting<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
            return true;
        }

        public virtual void OnGetAllExecuted<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
        }

        public virtual bool OnFindExecuting<T, TKey>(RepositoryQueryContext<T, TKey> context) where T : class
        {
            return true;
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

    public class RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryActionContext(IRepository<T, TKey> repository)
        {
            Repository = repository;
        }

        public IRepository<T, TKey> Repository { get; set; }
    }

    public class RepositoryQueryContext<T, TKey> : RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryQueryContext(IRepository<T, TKey> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, int numberOfResults = 0)
            : base(repository)
        {
            Specification = specification;
            QueryOptions = queryOptions;
            NumberOfResults = numberOfResults;
        }

        public ISpecification<T> Specification { get; set; }
        public IQueryOptions<T> QueryOptions { get; set; }
        public int NumberOfResults { get; set; }
    }

    public class RepositoryGetContext<T, TKey> : RepositoryActionContext<T, TKey> where T : class
    {
        public RepositoryGetContext(IRepository<T, TKey> repository, T id, int numberOfResults)
            : base(repository)
        {
            Id = id;
            NumberOfResults = numberOfResults;
        }

        public T Id { get; set; }
        public int NumberOfResults { get; set; }
    }
}
