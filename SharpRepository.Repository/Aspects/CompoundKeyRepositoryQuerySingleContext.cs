using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public class CompoundKeyRepositoryQuerySingleContext<T, TKey, TKey2> : CompoundKeyRepositoryQuerySingleContext<T, TKey, TKey2, T> where T : class
    {
        public CompoundKeyRepositoryQuerySingleContext(ICompoundKeyRepository<T, TKey, TKey2> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }
    }
    public class CompoundKeyRepositoryQuerySingleContext<T> : CompoundKeyRepositoryQuerySingleContext<T, T> where T : class
    {
        public CompoundKeyRepositoryQuerySingleContext(ICompoundKeyRepository<T> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions)
        {
        }
    }
    public class CompoundKeyRepositoryQuerySingleContext<T, TKey, TKey2, TResult> : CompoundKeyRepositoryQueryContext<T, TKey, TKey2, TResult> where T : class
    {
        public CompoundKeyRepositoryQuerySingleContext(ICompoundKeyRepository<T, TKey, TKey2> repository, ISpecification<T> specification,
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
    public class CompoundKeyRepositoryQuerySingleContext<T, TResult> : CompoundKeyRepositoryQueryContext<T, TResult> where T : class
    {
        public CompoundKeyRepositoryQuerySingleContext(ICompoundKeyRepository<T> repository, ISpecification<T> specification,
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
