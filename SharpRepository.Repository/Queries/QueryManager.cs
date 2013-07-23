using System;
using System.Collections.Generic;
using System.Linq;
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
    public class QueryManager<T, TKey> where T : class
    {
        private readonly ICachingStrategy<T, TKey> _cachingStrategy;

        public QueryManager(ICachingStrategy<T, TKey> cachingStrategy)
        {
            CacheUsed = false;
            CacheEnabled = true;
            _cachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey>();
        }

        public bool CacheUsed { get; private set; }

        public bool CacheEnabled { get; set; }

        public T ExecuteGet(Func<T> query, TKey key)
        {
            T result;

            if (CacheEnabled && _cachingStrategy.TryGetResult(key, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGetResult(key, result);

            return result;
        }

        public IEnumerable<TResult> ExecuteGetAll<TResult>(Func<IEnumerable<TResult>> query, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            IEnumerable<TResult> result;
            if (CacheEnabled && _cachingStrategy.TryGetAllResult(queryOptions, selector, out result))
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
            IEnumerable<TResult> result;
            if (CacheEnabled && _cachingStrategy.TryFindAllResult(criteria, queryOptions, selector, out result))
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
            TResult result;
            if (CacheEnabled && _cachingStrategy.TryFindResult(criteria, queryOptions, selector, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveFindResult(criteria, queryOptions, selector, result);

            return result;
        }

        public int ExecuteCount(Func<int> query, ISpecification<T> criteria)
        {
            int result;
            if (CacheEnabled && _cachingStrategy.TryCountResult(criteria, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveCountResult(criteria, result);

            return result;
        }

        public long ExecuteLongCount(Func<long> query, ISpecification<T> criteria)
        {
            long result;
            if (CacheEnabled && _cachingStrategy.TryLongCountResult(criteria, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveLongCountResult(criteria, result);

            return result;
        }

        public IEnumerable<GroupCount<TGroupKey>> ExecuteGroupCounts<TGroupKey>(Func<IEnumerable<GroupCount<TGroupKey>>> query, Func<T, TGroupKey> keySelector)
        {
            IEnumerable<GroupCount<TGroupKey>> result;
            if (CacheEnabled && _cachingStrategy.TryGroupCountsResult(keySelector, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGroupCountsResult(keySelector, result);

            return result;
        }

        public IEnumerable<GroupItem<TGroupKey, TGroupResult>> ExecuteGroupItems<TGroupKey, TGroupResult>(Func<IEnumerable<GroupItem<TGroupKey, TGroupResult>>> query, Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector)
        {
            IEnumerable<GroupItem<TGroupKey, TGroupResult>> result;
            if (CacheEnabled && _cachingStrategy.TryGroupItemsResult(keySelector, resultSelector, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGroupItemsResult(keySelector, resultSelector, result);

            return result;
        }

        public void OnSaveExecuted()
        {
            if (CacheEnabled)
                _cachingStrategy.Save();
        }

        public void OnItemDeleted(TKey key, T item)
        {
            if (CacheEnabled)
                _cachingStrategy.Delete(key, item);
        }

        public void OnItemAdded(TKey key, T item)
        {
            if (CacheEnabled)
                _cachingStrategy.Add(key, item);
        }

        public void OnItemUpdated(TKey key, T item)
        {
            if (CacheEnabled)
                _cachingStrategy.Update(key, item);
        }
    }
}
