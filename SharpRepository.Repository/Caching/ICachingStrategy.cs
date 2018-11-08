using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public interface ICachingStrategy<T, TKey>
    {
        bool TryGetResult(TKey key, out T result);
        void SaveGetResult(TKey key, T result);

        bool TryGetResult<TResult>(TKey key, Expression<Func<T, TResult>> selector, out TResult result);
        void SaveGetResult<TResult>(TKey key, Expression<Func<T, TResult>> selector, TResult result);

        bool TryGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result);
        void SaveGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result);

        bool TryFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result);
        void SaveFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result);

        bool TryFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out TResult result);
        void SaveFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, TResult result);

        bool TryCountResult(ISpecification<T> criteria, out int count);
        void SaveCountResult(ISpecification<T> criteria, int count);

        bool TryLongCountResult(ISpecification<T> criteria, out long count);
        void SaveLongCountResult(ISpecification<T> criteria, long count);

        bool TryGroupResult<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector, ISpecification<T> criteria, out IEnumerable<TResult> result);
        void SaveGroupResult<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector, ISpecification<T> criteria, IEnumerable<TResult> result);

        bool TrySumResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, out TResult sum);
        void SaveSumResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, TResult sum);

        bool TryAverageResult<TSelector, TResult>(Expression<Func<T, TSelector>> selector, ISpecification<T> criteria, out TResult sum);
        void SaveAverageResult<TSelector, TResult>(Expression<Func<T, TSelector>> selector, ISpecification<T> criteria, TResult sum);
        
        bool TryMinResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, out TResult sum);
        void SaveMinResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, TResult sum);
        
        bool TryMaxResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, out TResult sum);
        void SaveMaxResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, TResult sum);

        void Add(TKey key, T result);
        void Update(TKey key, T result);
        void Delete(TKey key, T result);
        void Save();

        string CachePrefix { set; }
        string FullCachePrefix { get; }
        void ClearAll();

        ICachingProvider CachingProvider { get; set; }
        
    }
}
