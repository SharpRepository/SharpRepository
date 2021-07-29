using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public class CompoundKeyRepositoryQueryMultipleContext<T, TKey, TKey2> : CompoundKeyRepositoryQueryMultipleContext<T, TKey, TKey2, T> where T : class
    {
        public CompoundKeyRepositoryQueryMultipleContext(ICompoundKeyRepository<T, TKey, TKey2> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions, null)
        {
        }
    }

    public class CompoundKeyRepositoryQueryMultipleContext<T> : CompoundKeyRepositoryQueryMultipleContext<T, T> where T : class
    {
        public CompoundKeyRepositoryQueryMultipleContext(ICompoundKeyRepository<T> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions, null)
        {
        }
    }

    public class CompoundKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TResult> : CompoundKeyRepositoryQueryContext<T, TKey, TKey2, TResult> where T : class
    {
        public CompoundKeyRepositoryQueryMultipleContext(ICompoundKeyRepository<T, TKey, TKey2> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
            : base(repository, specification, queryOptions, selector)
        {
        }

        public IEnumerable<TResult> Results { get; set; }
        public override int NumberOfResults
        {
            get { return Results == null ? 0 : Results.Count(); }
        }        
    }

    public class CompoundKeyRepositoryQueryMultipleContext<T, TResult> : CompoundKeyRepositoryQueryContext<T, TResult> where T : class
    {
        public CompoundKeyRepositoryQueryMultipleContext(ICompoundKeyRepository<T> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
            : base(repository, specification, queryOptions, selector)
        {
        }

        public IEnumerable<TResult> Results { get; set; }
        public override int NumberOfResults
        {
            get { return Results == null ? 0 : Results.Count(); }
        }
    }
}
