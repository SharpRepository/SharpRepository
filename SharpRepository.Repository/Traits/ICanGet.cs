using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;

namespace SharpRepository.Repository.Traits
{
    /// <summary>
    /// Based on the Interface Segregation Principle (ISP), the
    /// ICanGet interface exposes only the "Get" methods of the
    /// Repository.        
    /// <see cref="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/"/>  
    /// </summary>
    /// <typeparam name="T">Generic repository entity type</typeparam>
    /// <typeparam name="TKey">Generic repository entity key type</typeparam>
    public interface ICanGet<T, in TKey>
    {
        T Get(TKey key);
        T Get(TKey key, IFetchStrategy<T> fetchStrategy);
        T Get(TKey key, params string[] includePathes);
        T Get(TKey key, params Expression<Func<T, object>>[] includePathes);

        TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector);
        TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy);
        TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePathes);
        TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, params string[] includePathes);

        bool Exists(TKey key);
        bool TryGet(TKey key, out T entity);
        bool TryGet<TResult>(TKey key, Expression<Func<T, TResult>> selector, out TResult entity);

        IEnumerable<T> GetAll(IQueryOptions<T> queryOptions = null);
    }
}