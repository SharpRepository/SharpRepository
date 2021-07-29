using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public class CompoundTripleKeyRepositoryQuerySingleContext<T, TKey, TKey2, TKey3> : CompoundTripleKeyRepositoryQuerySingleContext<T, TKey, TKey2, TKey3, T> where T : class
    {
        public CompoundTripleKeyRepositoryQuerySingleContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }
    }

    public class CompoundTripleKeyRepositoryQuerySingleContext<T, TKey, TKey2, TKey3, TResult> : CompoundTripleKeyRepositoryQueryContext<T, TKey, TKey2, TKey3, TResult> where T : class
    {
        public CompoundTripleKeyRepositoryQuerySingleContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, ISpecification<T> specification,
                                         IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
            : base(repository, specification, queryOptions, selector)
        {
        }

        public TResult Result { get; set; }

        public bool HasResult
        {
            get { return NumberOfResults != 0; }
        }
        public override int NumberOfResults
        {
            get 
            { 
                return Result == null || Result.Equals(default(TResult)) ? 0 : 1; 
            }
        }
    }
}
