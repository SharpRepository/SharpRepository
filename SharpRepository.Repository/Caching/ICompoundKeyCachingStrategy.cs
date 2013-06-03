using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public interface ICompoundKeyCachingStrategyBase<T>
    {
        bool TryGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result);
        void SaveGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result);

        bool TryFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result);
        void SaveFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result);

        bool TryFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out TResult result);
        void SaveFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, TResult result);

        void Save();

        string FullCachePrefix { get; }
        void ClearAll();

        ICachingProvider CachingProvider { get; set; }
    }

    public interface ICompoundKeyCachingStrategy<T> : ICompoundKeyCachingStrategyBase<T>
    {
        bool TryGetResult<TResult>(object[] keys, Expression<Func<T, TResult>> selector, out TResult result);
        void SaveGetResult<TResult>(object[] keys, Expression<Func<T, TResult>> selector, TResult result);

        void Add(object[] keys, T result);
        void Update(object[] keys, T result);
        void Delete(object[] keys, T result);
    }

    public interface ICompoundKeyCachingStrategy<T, TKey, TKey2> : ICompoundKeyCachingStrategyBase<T>
    {
        bool TryGetResult<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, out TResult result);
        void SaveGetResult<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, TResult result);

        void Add(TKey key, TKey2 key2, T result);
        void Update(TKey key, TKey2 key2, T result);
        void Delete(TKey key, TKey2 key2, T result);
    }

    public interface ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> : ICompoundKeyCachingStrategyBase<T>
    {
        bool TryGetResult<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, out TResult result);
        void SaveGetResult<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, TResult result);

        void Add(TKey key, TKey2 key2, TKey3 key3, T result);
        void Update(TKey key, TKey2 key2, TKey3 key3, T result);
        void Delete(TKey key, TKey2 key2, TKey3 key3, T result);
    }
}
