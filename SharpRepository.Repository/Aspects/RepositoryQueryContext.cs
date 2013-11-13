using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public abstract class RepositoryQueryContext<T, TKey> : RepositoryQueryContext<T, TKey, T> where T : class
    {
        protected RepositoryQueryContext(IRepository<T, TKey> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }
    }

    public abstract class RepositoryQueryContext<T, TKey, TResult> : RepositoryActionContext<T, TKey> where T : class
    {
        protected RepositoryQueryContext(IRepository<T, TKey> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
            : base(repository)
        {
            Specification = specification;
            QueryOptions = queryOptions;
            Selector = selector;
        }

        public ISpecification<T> Specification { get; set; }
        public IQueryOptions<T> QueryOptions { get; set; }
        public virtual int NumberOfResults { get; internal set; }
        public Expression<Func<T, TResult>> Selector { get; set; }
    }
}