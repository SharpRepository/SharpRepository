using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aspects
{
    public class RepositoryQueryMultipleContext<T, TKey> : RepositoryQueryMultipleContext<T, TKey, T> where T : class
    {
        public RepositoryQueryMultipleContext(IRepository<T, TKey> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
            : base(repository, specification, queryOptions, null)
        {
        }
    }

    public class RepositoryQueryMultipleContext<T, TKey, TResult> : RepositoryQueryContext<T, TKey, TResult> where T : class
    {
        public RepositoryQueryMultipleContext(IRepository<T, TKey> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector = null)
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
