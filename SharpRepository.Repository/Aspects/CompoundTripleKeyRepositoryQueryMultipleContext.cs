using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public class CompoundTripleKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TKey3> : CompoundTripleKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TKey3, T> where T : class
    {
        public CompoundTripleKeyRepositoryQueryMultipleContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions, null)
        {
        }
    }

    public class CompoundTripleKeyRepositoryQueryMultipleContext<T, TKey, TKey2, TKey3, TResult> : CompoundTripleKeyRepositoryQueryContext<T, TKey, TKey2, TKey3, TResult> where T : class
    {
        public CompoundTripleKeyRepositoryQueryMultipleContext(ICompoundKeyRepository<T, TKey, TKey2, TKey3> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
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
