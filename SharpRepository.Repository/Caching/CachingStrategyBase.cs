using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
   public  abstract class CachingStrategyBase<T, TKey> : ICachingStrategy<T, TKey> where T : class
    {
       private ICachingProvider _cachingProvider;
       public string CachePrefix { get; set; }
       protected string TypeFullName { get; set; }
       public int? MaxResults { get; set; }

       internal CachingStrategyBase()
       {
       }

       internal CachingStrategyBase(int? maxResults, ICachingProvider cachingProvider)
       {
           CachePrefix = "#Repo";
           CachingProvider = cachingProvider;
           MaxResults = maxResults;

           TypeFullName = typeof(T).FullName ?? typeof(T).Name; // sometimes FullName returns null in certain derived type situations, so I added the check to use the Name property if FullName is null
       }

       public ICachingProvider CachingProvider
       {
           get { return _cachingProvider; }
           set { _cachingProvider = value ?? new InMemoryCachingProvider(); }
       }

       public virtual bool TryGetResult(TKey key,  out T result)
       {
           result = default(T);
           try
           {
               return IsInCache(GetWriteThroughCacheKey(key), out result);
           }
           catch (Exception)
           {
               // don't let an error trying to find results stop everything, it should continue and then just go get the results from the DB itself
               return false;
           }
       }

       public virtual void SaveGetResult(TKey key, T item)
       {
           try
           {
               SetCache(GetWriteThroughCacheKey(key), item);
           }
           catch (Exception)
           {
               // don't let an error saving cache stop everything else
           }
       }

       public virtual bool TryGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result)
       {
           result = null;
           try
           {
               var cacheKey = GetAllCacheKey(queryOptions, selector);
               if (!IsInCache(cacheKey, out result))
                   return false;

               // if there are no query options then we don't need to check for the cache for data to update them with
               return queryOptions == null || SetCachedQueryOptions(cacheKey, queryOptions);
           }
           catch (Exception)
           {
               // don't let an error trying to find results stop everything, it should continue and then just go get the results from the DB itself
               return false;
           }
       }

       public virtual void SaveGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result)
       {
           try
           {
                SetCache(GetAllCacheKey(queryOptions, selector), result, queryOptions);
           }
           catch (Exception)
           {
               // don't let an error saving cache stop everything else
           }
       }

       public virtual bool TryFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result)
       {
           result = null;
           try
           {
               var cacheKey = FindAllCacheKey(criteria, queryOptions, selector);
               if (!IsInCache(cacheKey, out result))
                   return false;

               // if there are no query options then we don't need to check for the cache for data to update them with
               return queryOptions == null || SetCachedQueryOptions(cacheKey, queryOptions);
           }
           catch (Exception)
           {
               // don't let an error trying to find results stop everything, it should continue and then just go get the results from the DB itself
               return false;
           }
       }

       public virtual void SaveFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result)
       {
           try
           {
                SetCache(FindAllCacheKey(criteria, queryOptions, selector), result, queryOptions);
           }
           catch (Exception)
           {
               // don't let an error saving cache stop everything else;
           }
       }

       public virtual bool TryFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out TResult result)
       {
           result = default(TResult);
           try
           {
               return IsInCache(FindCacheKey(criteria, queryOptions, selector), out result);
           }
           catch (Exception)
           {
               // don't let an error trying to find results stop everything, it should continue and then just go get the results from the DB itself
               return false;
           }
       }

       public virtual void SaveFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, TResult result)
       {
           try
           {
               SetCache(FindCacheKey(criteria, queryOptions, selector), result);
           }
           catch (Exception)
           {
               // don't let an error saving cache stop everything else
           }
       }

       public abstract void Add(TKey key, T result);

       public abstract void Update(TKey key, T result);

       public abstract void Delete(TKey key, T result);

       public abstract void Save();

       protected bool IsInCache<TCacheItem>(string cacheKey, out TCacheItem result)
       {
           result = default(TCacheItem);

           try
           {
               if (CachingProvider.Get(cacheKey, out result))
               {
                   return true;
               }
           }
           catch (Exception)
           {
               // don't let caching errors cause problems for the Repository
           }

           return false;
       }

       protected bool IsPagingTotalInCache(string cacheKey, out int totalItems)
       {
           totalItems = 0;
           try
           {
               if (CachingProvider.Get(cacheKey + "=>pagingTotal", out totalItems))
               {
                   //Trace.WriteLine(String.Format("Got item from cache: {0} - {1}", cacheKey, typeof(TCacheItem).Name));
                   return true;
               }
           }
           catch (Exception)
           {
               // don't let caching errors cause problems for the Repository
           }

           return false;
       }

       /// <summary>
       ///  This will repopualte the PagingOptions.TotalItems from the value stored in cache.  This should only be called if the results were already found in cache.
       /// </summary>
       /// <param name="cacheKey"></param>
       /// <param name="queryOptions"></param>
       /// <returns>True if it is not a PagingOptions query or if it is and the TotalItems is stored in cache as well</returns>
       protected bool SetCachedQueryOptions(string cacheKey, IQueryOptions<T> queryOptions)
       {
           // TODO: see if there is a better way that doesn't rely on checking for PagingOptions specifically
           //  originally was thinking doing a ref arg for queryOptions and setting it via cache but ran into an issue in QueryManager using a ref in a lamda expression

           // we only need to do this for PagingOptions because it has a TotalItems property that we need
           if (!(queryOptions is IPagingOptions))
               return true;

           int totalItems;
           // there is a PagingOptions passed in so we want to make sure that both the results and the queryOptions are in cache
           //      this is a safety in case the caching provider kicked one of them out
           if (IsPagingTotalInCache(cacheKey, out totalItems))
           {
               ((IPagingOptions)queryOptions).TotalItems = totalItems;
               return true;
           }

           // this was a PagingOptions query but the value wasn't in cache, so return false which will make the entire query be ran again so the results and TotalItems will get cached
           return false;
       }

       protected void ClearCache(string cacheKey)
       {
           try
           {
               CachingProvider.Clear(cacheKey);
           }
           catch (Exception)
           {
               // don't let caching errors mess with the repository
           }
       }

       protected void SetCache<TCacheItem>(string cacheKey, TCacheItem result, IQueryOptions<T> queryOptions = null)
       {
           try
           {
               CachingProvider.Set(cacheKey, result);

               if (queryOptions is IPagingOptions)
               {
                   CachingProvider.Set(cacheKey + "=>pagingTotal", ((IPagingOptions)queryOptions).TotalItems);
               }
               //Trace.WriteLine(String.Format("Write item to cache: {0} - {1}", cacheKey, typeof(TCacheItem).Name));
           }
           catch (Exception)
           {
               // don't let caching errors mess with the repository
           }
       }

       protected string GetWriteThroughCacheKey(TKey key)
       {
           return String.Format("{0}/{1}/{2}", CachePrefix, TypeFullName, key);
       }

       protected abstract string GetAllCacheKey<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector);

       protected abstract string FindAllCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector);

       protected abstract string FindCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector);
    }
}
