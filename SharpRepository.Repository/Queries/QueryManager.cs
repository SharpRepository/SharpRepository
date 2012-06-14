using System;
using System.Collections.Generic;
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
            _cachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey>();
        }

        public T ExecuteGet(Func<T> query, TKey key)
        {
            T result;
            if (_cachingStrategy.TryGetResult(key, out result))
            {
                return result;
            }

            result = query.Invoke();

            _cachingStrategy.SaveGetResult(key, result);

            return result;
        }

        public IEnumerable<T> ExecuteGetAll(Func<IEnumerable<T>> query, IQueryOptions<T> queryOptions)
        {
            IEnumerable<T> result;
            if (_cachingStrategy.TryGetAllResult(queryOptions, out result))
            {
                return result;
            }

            result = query.Invoke();

            _cachingStrategy.SaveGetAllResult(queryOptions, result);

            return result;
        }

        public IEnumerable<T> ExecuteFindAll(Func<IEnumerable<T>> query, ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            IEnumerable<T> result;
            if (_cachingStrategy.TryFindAllResult(criteria, queryOptions, out result))
            {
                return result;
            }

            result = query.Invoke();

            _cachingStrategy.SaveFindAllResult(criteria, queryOptions, result);

            return result;
        }

        public T ExecuteFind(Func<T> query, ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            T result;
            if (_cachingStrategy.TryFindResult(criteria, queryOptions, out result))
            {
                return result;
            }

            result = query.Invoke();

            _cachingStrategy.SaveFindResult(criteria, queryOptions, result);

            return result;
        }

        public void OnSaveExecuted()
        {
            _cachingStrategy.Save();
        }

        public void OnItemDeleted(TKey key, T item)
        {
            _cachingStrategy.Delete(key, item);
        }

        public void OnItemAdded(TKey key, T item)
        {
            _cachingStrategy.Add(key, item);
        }

        public void OnItemUpdated(TKey key, T item)
        {
            _cachingStrategy.Update(key, item);
        }
    }
}
