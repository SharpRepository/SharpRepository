using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public abstract class CompoundKeyRepositoryQueryContext<T, TKey, TKey2> : CompoundKeyRepositoryQueryContext<T, TKey, TKey2, T> where T : class
    {
        protected CompoundKeyRepositoryQueryContext(ICompoundKeyRepository<T, TKey, TKey2> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }
    }

    public abstract class CompoundKeyRepositoryQueryContext<T> : CompoundKeyRepositoryQueryContext<T, T> where T : class
    {
        protected CompoundKeyRepositoryQueryContext(ICompoundKeyRepository<T> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }
    }

    public abstract class CompoundKeyRepositoryQueryContext<T, TKey, TKey2, TResult> : RepositoryActionContext<T, TKey, TKey2> where T : class
    {
        protected CompoundKeyRepositoryQueryContext(ICompoundKeyRepository<T, TKey, TKey2> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
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

    public abstract class CompoundKeyRepositoryQueryContext<T, TResult> : RepositoryActionContext<T> where T : class
    {
        protected CompoundKeyRepositoryQueryContext(ICompoundKeyRepository<T> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
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