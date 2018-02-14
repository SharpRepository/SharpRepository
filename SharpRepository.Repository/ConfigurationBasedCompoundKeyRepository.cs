using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Repository.Transactions;
using SharpRepository.Repository.Configuration;
using Microsoft.Extensions.Options;

namespace SharpRepository.Repository
{
    /// <summary>
    /// Inherit from this when you want to create a custom Repository and have the specific type based on the configuration file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    public class ConfigurationBasedCompoundKeyRepository<T, TKey, TKey2> : ICompoundKeyRepository<T, TKey, TKey2> where T : class, new()
    {
        protected readonly ICompoundKeyRepository<T, TKey, TKey2> Repository;
        
        public ConfigurationBasedCompoundKeyRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            Repository = RepositoryFactory.GetInstance<T, TKey, TKey2>(configuration, repositoryName);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }

        public IQueryable<T> AsQueryable()
        {
            return Repository.AsQueryable();
        }

        public IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector,
                                                                    Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class
        {
            return Repository.Join(innerRepository, outerKeySelector, innerKeySelector, resultSelector);
        }

        public IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(fetchStrategy);
        }

        public IEnumerable<T> GetAll(params string[] includePaths)
        {
            return Repository.GetAll(includePaths);
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(includePaths);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(queryOptions);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(queryOptions, fetchStrategy);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return Repository.GetAll(queryOptions, includePaths);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(queryOptions, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Repository.GetAll(selector);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(selector, queryOptions);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(selector, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths)
        {
            return Repository.GetAll(selector, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(selector, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(selector, queryOptions, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return Repository.GetAll(selector, queryOptions, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(selector, queryOptions, includePaths);
        }

        public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(predicate, queryOptions);
        }

        public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(predicate, selector, queryOptions);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return TryFind(predicate, out T entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, out T entity)
        {
            return Repository.TryFind(predicate, out entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity)
        {
            return Repository.TryFind(predicate, queryOptions, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryFind(predicate, selector, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            return Repository.TryFind(predicate, selector, queryOptions, out entity);
        }

        public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, queryOptions);
        }

        public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, selector, queryOptions);
        }

        public bool Exists(ISpecification<T> criteria)
        {
            return Repository.Exists(criteria);
        }

        public bool TryFind(ISpecification<T> criteria, out T entity)
        {
            return Repository.TryFind(criteria, out entity);
        }

        public bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity)
        {
            return Repository.TryFind(criteria, queryOptions, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryFind(criteria, selector, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            return Repository.TryFind(criteria, selector, queryOptions,  out entity);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(predicate, queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(predicate, selector, queryOptions);
        }

        public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(criteria, queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(criteria, selector, queryOptions);
        }

        public T Get(TKey key, TKey2 key2)
        {
            return Repository.Get(key, key2);
        }
        
        public TResult Get<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector)
        {
            return Repository.Get(key, key2, selector);
        }        

        public bool Exists(TKey key, TKey2 key2)
        {
            return TryGet(key, key2, out T entity);
        }

        public bool TryGet(TKey key, TKey2 key2, out T entity)
        {
            return Repository.TryGet(key, key2, out entity);
        }

        public bool TryGet<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryGet(key, key2, selector, out entity);
        }

        public void Add(T entity)
        {
            Repository.Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            Repository.Add(entities);
        }

        public void Update(T entity)
        {
            Repository.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            Repository.Update(entities);
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            Repository.Delete(entities);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Repository.Delete(predicate);
        }

        public void Delete(ISpecification<T> criteria)
        {
            Repository.Delete(criteria);
        }

        public void Delete(TKey key, TKey2 key2)
        {
            Repository.Delete(key, key2);
        }
       
        public IBatch<T> BeginBatch()
        {
            return Repository.BeginBatch();
        }

        public ICompoundKeyCachingStrategy<T, TKey, TKey2> CachingStrategy
        {
            get { return Repository.CachingStrategy; }
            set { Repository.CachingStrategy = value; }
        }
    }


    public class ConfigurationBasedCompoundKeyRepository<T, TKey, TKey2, TKey3> : ICompoundKeyRepository<T, TKey, TKey2, TKey3> where T : class, new()
    {
        protected readonly ICompoundKeyRepository<T, TKey, TKey2, TKey3> Repository;

        public ConfigurationBasedCompoundKeyRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            Repository = RepositoryFactory.GetInstance<T, TKey, TKey2, TKey3>(configuration, repositoryName);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }

        public IQueryable<T> AsQueryable()
        {
            return Repository.AsQueryable();
        }

        public IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector,
                                                                    Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class
        {
            return Repository.Join(innerRepository, outerKeySelector, innerKeySelector, resultSelector);
        }

        public IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(fetchStrategy);
        }

        public IEnumerable<T> GetAll(params string[] includePaths)
        {
            return Repository.GetAll(includePaths);
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(includePaths);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(queryOptions);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(queryOptions, fetchStrategy);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return Repository.GetAll(queryOptions, includePaths);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(queryOptions, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Repository.GetAll(selector);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(selector, queryOptions);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(selector, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths)
        {
            return Repository.GetAll(selector, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(selector, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(selector, queryOptions, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return Repository.GetAll(selector, queryOptions, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(selector, queryOptions, includePaths);
        }

        public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(predicate, queryOptions);
        }

        public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(predicate, selector, queryOptions);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return TryFind(predicate, out T entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, out T entity)
        {
            return Repository.TryFind(predicate, out entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity)
        {
            return Repository.TryFind(predicate, queryOptions, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryFind(predicate, selector, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            return Repository.TryFind(predicate, selector, queryOptions, out entity);
        }

        public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, queryOptions);
        }

        public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, selector, queryOptions);
        }

        public bool Exists(ISpecification<T> criteria)
        {
            return Repository.Exists(criteria);
        }

        public bool TryFind(ISpecification<T> criteria, out T entity)
        {
            return Repository.TryFind(criteria, out entity);
        }

        public bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity)
        {
            return Repository.TryFind(criteria, queryOptions, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryFind(criteria, selector, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            return Repository.TryFind(criteria, selector, queryOptions, out entity);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(predicate, queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(predicate, selector, queryOptions);
        }

        public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(criteria, queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(criteria, selector, queryOptions);
        }

        public T Get(TKey key, TKey2 key2, TKey3 key3)
        {
            return Repository.Get(key, key2, key3);
        }

        public TResult Get<TResult>(TKey key, TKey2 key2, TKey3 key3,Expression<Func<T, TResult>> selector)
        {
            return Repository.Get(key, key2, key3, selector);
        }

        public bool Exists(TKey key, TKey2 key2, TKey3 key3)
        {
            return TryGet(key, key2, key3, out T entity);
        }

        public bool TryGet(TKey key, TKey2 key2, TKey3 key3, out T entity)
        {
            return Repository.TryGet(key, key2, key3, out entity);
        }

        public bool TryGet<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryGet(key, key2, key3, selector, out entity);
        }

        public void Add(T entity)
        {
            Repository.Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            Repository.Add(entities);
        }

        public void Update(T entity)
        {
            Repository.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            Repository.Update(entities);
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            Repository.Delete(entities);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Repository.Delete(predicate);
        }

        public void Delete(ISpecification<T> criteria)
        {
            Repository.Delete(criteria);
        }

        public void Delete(TKey key, TKey2 key2, TKey3 key3)
        {
            Repository.Delete(key, key2, key3);
        }

        public IBatch<T> BeginBatch()
        {
            return Repository.BeginBatch();
        }

        public ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> CachingStrategy
        {
            get { return Repository.CachingStrategy; }
            set { Repository.CachingStrategy = value; }
        }
    }

    public class ConfigurationBasedCompoundKeyRepository<T> : ICompoundKeyRepository<T> where T : class, new()
    {
        protected readonly ICompoundKeyRepository<T> Repository;

        public ConfigurationBasedCompoundKeyRepository(ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            Repository = RepositoryFactory.GetCompoundKeyInstance<T>(configuration, repositoryName);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }

        public IQueryable<T> AsQueryable()
        {
            return Repository.AsQueryable();
        }

        public IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector,
                                                                    Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class
        {
            return Repository.Join(innerRepository, outerKeySelector, innerKeySelector, resultSelector);
        }

        public IEnumerable<T> GetAll()
        {
            return Repository.GetAll();
        }

        public IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(fetchStrategy);
        }

        public IEnumerable<T> GetAll(params string[] includePaths)
        {
            return Repository.GetAll(includePaths);
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(includePaths);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(queryOptions);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(queryOptions, fetchStrategy);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return Repository.GetAll(queryOptions, includePaths);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(queryOptions, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Repository.GetAll(selector);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(selector, queryOptions);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(selector, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths)
        {
            return Repository.GetAll(selector, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(selector, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return Repository.GetAll(selector, queryOptions, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return Repository.GetAll(selector, queryOptions, includePaths);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return Repository.GetAll(selector, queryOptions, includePaths);
        }

        public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(predicate, queryOptions);
        }

        public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(predicate, selector, queryOptions);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return TryFind(predicate, out T entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, out T entity)
        {
            return Repository.TryFind(predicate, out entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity)
        {
            return Repository.TryFind(predicate, queryOptions, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryFind(predicate, selector, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            return Repository.TryFind(predicate, selector, queryOptions, out entity);
        }

        public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, queryOptions);
        }

        public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, selector, queryOptions);
        }

        public bool Exists(ISpecification<T> criteria)
        {
            return Repository.Exists(criteria);
        }

        public bool TryFind(ISpecification<T> criteria, out T entity)
        {
            return Repository.TryFind(criteria, out entity);
        }

        public bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity)
        {
            return Repository.TryFind(criteria, queryOptions, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryFind(criteria, selector, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            return Repository.TryFind(criteria, selector, queryOptions, out entity);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(predicate, queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(predicate, selector, queryOptions);
        }

        public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(criteria, queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.FindAll(criteria, selector, queryOptions);
        }

        public T Get(params object[] keys)
        {
            return Repository.Get(keys);
        }

        public TResult Get<TResult>(Expression<Func<T, TResult>> selector, params object[] keys)
        {
            return Repository.Get(selector, keys);
        }

        public bool Exists(params object[] keys)
        {
            return TryGet(out T entity, keys);
        }

        public bool TryGet(out T entity, params object[] keys)
        {
            return Repository.TryGet(out entity, keys);
        }
        
        public void Add(T entity)
        {
            Repository.Add(entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            Repository.Add(entities);
        }

        public void Update(T entity)
        {
            Repository.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            Repository.Update(entities);
        }

        public void Delete(T entity)
        {
            Repository.Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            Repository.Delete(entities);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Repository.Delete(predicate);
        }

        public void Delete(ISpecification<T> criteria)
        {
            Repository.Delete(criteria);
        }

        public void Delete(params object[] keys)
        {
            Repository.Delete(keys);
        }

        public IBatch<T> BeginBatch()
        {
            return Repository.BeginBatch();
        }

        public ICompoundKeyCachingStrategy<T> CachingStrategy
        {
            get { return Repository.CachingStrategy; }
            set { Repository.CachingStrategy = value; }
        }
    }
}
