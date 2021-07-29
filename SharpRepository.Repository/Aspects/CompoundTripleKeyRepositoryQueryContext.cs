using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public abstract class CompoundTripleKeyRepositoryQueryContext<T, TKey, TKey2, TKey3> : CompoundTripleKeyRepositoryQueryContext<T, TKey, TKey2, TKey3, T> where T : class
    {
        protected CompoundTripleKeyRepositoryQueryContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }
    }

    public abstract class CompoundTripleKeyRepositoryQueryContext<T, TKey, TKey2, TKey3, TResult> : RepositoryActionContext<T, TKey, TKey2, TKey3> where T : class
    {
        protected CompoundTripleKeyRepositoryQueryContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
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