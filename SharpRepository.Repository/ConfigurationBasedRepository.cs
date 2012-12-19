using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        // no need to new this up directly, should be used as the base class for a custom repository implementation
        protected ConfigurationBasedRepository()
        {
            // Load up the repository based on the configuration file
            Repository = RepositoryFactory.GetInstance<T, TKey>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Repository.GetEnumerator();
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

        public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, queryOptions);
        }

        public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            return Repository.Find(criteria, selector, queryOptions);
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

        public T Get(TKey key)
        {
            return Repository.Get(key);
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector)
        {
            return Repository.Get(key, selector);
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

        public ICachingStrategy<T, TKey> CachingStrategy
        {
            get { return Repository.CachingStrategy; }
            set { Repository.CachingStrategy = value; }
        }
    }
}
