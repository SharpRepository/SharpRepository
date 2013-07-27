using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public interface ICachingStrategy<T, TKey>
    {
        bool TryGetResult(TKey key, out T result);
        void SaveGetResult(TKey key, T result);

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

        bool TryGroupCountsResult<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria, out IDictionary<TGroupKey, int> result);
        void SaveGroupCountsResult<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria, IDictionary<TGroupKey, int> result);

        bool TryGroupLongCountsResult<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria, out IDictionary<TGroupKey, long> result);
        void SaveGroupLongCountsResult<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria, IDictionary<TGroupKey, long> result);

        bool TryGroupItemsResult<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector, ISpecification<T> criteria, out IEnumerable<GroupItem<TGroupKey, TGroupResult>> result);
        void SaveGroupItemsResult<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector, ISpecification<T> criteria, IEnumerable<GroupItem<TGroupKey, TGroupResult>> result);

        bool TrySumResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, out TResult sum);
        void SaveSumResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, TResult sum);
        
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
