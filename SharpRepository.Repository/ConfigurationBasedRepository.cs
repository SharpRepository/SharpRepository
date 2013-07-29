using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Aggregates;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Repository.Transactions;

namespace SharpRepository.Repository
{
    /// <summary>
    /// Inherit from this when you want to create a custom Repository and have the specific type based on the configuration file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class ConfigurationBasedRepository<T, TKey> : IRepository<T, TKey> where T : class, new()
    {
        protected readonly IRepository<T, TKey> Repository;

        // protected constructors so user can't instantiate directly
        // we have 2 constructors so you can use the defualt sharpRepository section or specify a config section
        //  you can also provide the repository name from the config file instead of whatever the default is if needed
        protected ConfigurationBasedRepository(string configSection, string repositoryName)
        {
            Repository = RepositoryFactory.GetInstance<T, TKey>(configSection, repositoryName);
        }

        protected ConfigurationBasedRepository(string repositoryName = null)
        {
            // Load up the repository based on the default configuration file
            Repository = RepositoryFactory.GetInstance<T, TKey>(repositoryName);
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

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        {
            return Repository.GetAll(queryOptions);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.GetAll(selector, queryOptions);
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
            T entity;
            return TryFind(predicate, out entity);
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

        public IRepositoryConventions Conventions
        {
            get { return Repository.Conventions; }
            set { Repository.Conventions = value;  }
        }

        public IAggregateQueries<T> Aggregates { get { return Repository.Aggregates; } }

        public T Get(TKey key)
        {
            return Repository.Get(key);
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector)
        {
            return Repository.Get(key, selector);
        }

        public bool Exists(TKey key)
        {
            T entity;
            return TryGet(key, out entity);
        }

        public bool TryGet(TKey key, out T entity)
        {
            return Repository.TryGet(key, out entity);
        }

        public bool TryGet<TResult>(TKey key, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return Repository.TryGet(key, selector, out entity);
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

        public void Delete(TKey key)
        {
            Repository.Delete(key);
        }

        public IBatch<T> BeginBatch()
        {
            return Repository.BeginBatch();
        }

        public IDisabledCache DisableCaching()
        {
            return Repository.DisableCaching();
        }

        public void ClearCache()
        {
            Repository.ClearCache();
        }

        public ICachingStrategy<T, TKey> CachingStrategy
        {
            get { return Repository.CachingStrategy; }
            set { Repository.CachingStrategy = value; }
        }

        public bool CachingEnabled
        {
            get { return Repository.CachingEnabled; }
            set { Repository.CachingEnabled = value; }
        }

        public bool CacheUsed
        {
            get { return Repository.CacheUsed; }
        }
    }
}
