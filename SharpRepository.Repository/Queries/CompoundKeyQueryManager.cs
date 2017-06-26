using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Queries
{
    /// <summary>
    /// The QueryManager is the middle man between the repository and the caching strategy.
    /// It receives a query that should be run, checks the cache for valid results to return, and if none are found runs the query and caches the results according to the caching strategy.
    /// It also notifies the caching strategy of CRUD operations in case the caching strategy needs to act as a result of a certain action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class CompoundKeyQueryManager<T> where T : class
    {
        private readonly ICompoundKeyCachingStrategy<T> _cachingStrategy;

        public CompoundKeyQueryManager(ICompoundKeyCachingStrategy<T> cachingStrategy)
        {
            CacheUsed = false;
            _cachingStrategy = cachingStrategy ?? new NoCompoundKeyCachingStrategy<T>();
        }

        public bool CacheUsed { get; private set; }

        public TResult ExecuteGet<TResult>(Func<TResult> query, Expression<Func<T, TResult>> selector, object[] keys)
        {
            if (_cachingStrategy.TryGetResult(keys, selector, out TResult result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGetResult(keys, selector, result);

            return result;
        }

        public IEnumerable<TResult> ExecuteGetAll<TResult>(Func<IEnumerable<TResult>> query, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryGetAllResult(queryOptions, selector, out IEnumerable<TResult> result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGetAllResult(queryOptions, selector, result);

            return result;
        }

        public IEnumerable<TResult> ExecuteFindAll<TResult>(Func<IEnumerable<TResult>> query, ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryFindAllResult(criteria, queryOptions, selector, out IEnumerable<TResult> result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveFindAllResult(criteria, queryOptions, selector, result);

            return result;
        }

        public TResult ExecuteFind<TResult>(Func<TResult> query, ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryFindResult(criteria, queryOptions, selector, out TResult result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveFindResult(criteria, queryOptions, selector, result);

            return result;
        }

        public void OnSaveExecuted()
        {
            _cachingStrategy.Save();
        }

        public void OnItemDeleted(object[] keys, T item)
        {
            _cachingStrategy.Delete(keys, item);
        }

        public void OnItemAdded(object[] keys, T item)
        {
            _cachingStrategy.Add(keys, item);
        }

        public void OnItemUpdated(object[] keys, T item)
        {
            _cachingStrategy.Update(keys, item);
        }
    }

    /// <summary>
    /// The QueryManager is the middle man between the repository and the caching strategy.
    /// It receives a query that should be run, checks the cache for valid results to return, and if none are found runs the query and caches the results according to the caching strategy.
    /// It also notifies the caching strategy of CRUD operations in case the caching strategy needs to act as a result of a certain action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">The type of the first part of the compound primary key.</typeparam>
    /// <typeparam name="TKey2">The type of the second part of the compound primary key.</typeparam>
    public class CompoundKeyQueryManager<T, TKey, TKey2> where T : class
    {
        private readonly ICompoundKeyCachingStrategy<T, TKey, TKey2> _cachingStrategy;

        public CompoundKeyQueryManager(ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy)
        {
            CacheUsed = false;
            _cachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey, TKey2>();
        }

        public bool CacheUsed { get; private set; }

        public TResult ExecuteGet<TResult>(Func<TResult> query, Expression<Func<T, TResult>> selector, TKey key, TKey2 key2)
        {
            if (_cachingStrategy.TryGetResult(key, key2, selector, out TResult result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGetResult(key, key2, selector, result);

            return result;
        }

        public IEnumerable<TResult> ExecuteGetAll<TResult>(Func<IEnumerable<TResult>> query, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryGetAllResult(queryOptions, selector, out IEnumerable<TResult> result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGetAllResult(queryOptions, selector, result);

            return result;
        }

        public IEnumerable<TResult> ExecuteFindAll<TResult>(Func<IEnumerable<TResult>> query, ISpecification<T> criteria, Expression<Func<T, TResult>> selector,  IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryFindAllResult(criteria, queryOptions, selector, out IEnumerable<TResult> result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveFindAllResult(criteria, queryOptions, selector, result);

            return result;
        }

        public TResult ExecuteFind<TResult>(Func<TResult> query, ISpecification<T> criteria, Expression<Func<T, TResult>> selector,  IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryFindResult(criteria, queryOptions, selector, out TResult result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveFindResult(criteria, queryOptions, selector, result);

            return result;
        }

        public void OnSaveExecuted()
        {
            _cachingStrategy.Save();
        }

        public void OnItemDeleted(TKey key, TKey2 key2, T item)
        {
            _cachingStrategy.Delete(key, key2, item);
        }

        public void OnItemAdded(TKey key, TKey2 key2, T item)
        {
            _cachingStrategy.Add(key, key2, item);
        }

        public void OnItemUpdated(TKey key, TKey2 key2, T item)
        {
            _cachingStrategy.Update(key, key2, item);
        }
    }

    /// <summary>
    /// The QueryManager is the middle man between the repository and the caching strategy.
    /// It receives a query that should be run, checks the cache for valid results to return, and if none are found runs the query and caches the results according to the caching strategy.
    /// It also notifies the caching strategy of CRUD operations in case the caching strategy needs to act as a result of a certain action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">The type of the first part of the compound primary key.</typeparam>
    /// <typeparam name="TKey2">The type of the second part of the compound primary key.</typeparam>
    /// <typeparam name="TKey3">The type of the third part of the compound primary key.</typeparam>
    public class CompoundKeyQueryManager<T, TKey, TKey2, TKey3> where T : class
    {
        private readonly ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> _cachingStrategy;

        public CompoundKeyQueryManager(ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy)
        {
            CacheUsed = false;
            _cachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey, TKey2, TKey3>();
        }

        public bool CacheUsed { get; private set; }

        public TResult ExecuteGet<TResult>(Func<TResult> query, Expression<Func<T, TResult>> selector, TKey key, TKey2 key2, TKey3 key3)
        {
            if (_cachingStrategy.TryGetResult(key, key2, key3, selector, out TResult result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGetResult(key, key2, key3, selector, result);

            return result;
        }

        public IEnumerable<TResult> ExecuteGetAll<TResult>(Func<IEnumerable<TResult>> query, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryGetAllResult(queryOptions, selector, out IEnumerable<TResult> result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGetAllResult(queryOptions, selector, result);

            return result;
        }

        public IEnumerable<TResult> ExecuteFindAll<TResult>(Func<IEnumerable<TResult>> query, ISpecification<T> criteria, Expression<Func<T, TResult>> selector,  IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryFindAllResult(criteria, queryOptions, selector, out IEnumerable<TResult> result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveFindAllResult(criteria, queryOptions, selector, result);

            return result;
        }

        public TResult ExecuteFind<TResult>(Func<TResult> query, ISpecification<T> criteria, Expression<Func<T, TResult>> selector,  IQueryOptions<T> queryOptions)
        {
            if (_cachingStrategy.TryFindResult(criteria, queryOptions, selector, out TResult result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveFindResult(criteria, queryOptions, selector, result);

            return result;
        }

        public void OnSaveExecuted()
        {
            _cachingStrategy.Save();
        }

        public void OnItemDeleted(TKey key, TKey2 key2, TKey3 key3, T item)
        {
            _cachingStrategy.Delete(key, key2, key3, item);
        }

        public void OnItemAdded(TKey key, TKey2 key2, TKey3 key3, T item)
        {
            _cachingStrategy.Add(key, key2, key3, item);
        }

        public void OnItemUpdated(TKey key, TKey2 key2, TKey3 key3, T item)
        {
            _cachingStrategy.Update(key, key2, key3, item);
        }
    }
}
