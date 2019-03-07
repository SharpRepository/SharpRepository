using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Repository.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SharpRepository.Repository
{
    public abstract class CompoundKeyRepositoryBaseCommon<T> where T : class
    {
        protected string TypeName { get; }
        public abstract bool CacheUsed { get; }
        protected internal bool BatchMode { get; set; }

        public CompoundKeyRepositoryBaseCommon()
        {
            TypeName = this.GetType().Name;
        }

        public abstract IBatch<T> BeginBatch();
        public abstract IQueryable<T> AsQueryable();

        // These are the actual implementation that the derived class needs to implement
        protected abstract IQueryable<T> GetAllQuery(IFetchStrategy<T> fetchStrategy);
        protected abstract IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

        public IEnumerable<T> GetAll()
        {
            return GetAll((IQueryOptions<T>)null, (IFetchStrategy<T>)null);
        }

        public IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy)
        {
            return GetAll((IQueryOptions<T>)null, fetchStrategy);
        }

        public IEnumerable<T> GetAll(params string[] includePaths)
        {
            return GetAll(RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths)
        {
            return GetAll(RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        {
            return GetAll(queryOptions, (IFetchStrategy<T>)null);
        }

        public abstract IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return GetAll(queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return GetAll(queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
        {
            return GetAll(selector, (IQueryOptions<T>)null, (IFetchStrategy<T>)null);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
        {
            return GetAll(selector, queryOptions, (IFetchStrategy<T>)null);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
        {
            return GetAll(selector, null, fetchStrategy);
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths)
        {
            return GetAll(selector, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
        {
            return GetAll(selector, RepositoryHelper.BuildFetchStrategy(includePaths));
        }

        public abstract IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
        }

        public abstract IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class;
        
        // These are the actual implementation that the derived class needs to implement
        protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria);
        protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

        public abstract IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null);
        public abstract IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");

            return FindAll(new Specification<T>(predicate), queryOptions);
        }

        public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (selector == null) throw new ArgumentNullException("selector");

            return FindAll(new Specification<T>(predicate), selector, queryOptions);
        }

        // These are the actual implementation that the derived class needs to implement
        protected abstract T FindQuery(ISpecification<T> criteria);
        protected abstract T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

        public abstract T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null);

        public bool Exists(ISpecification<T> criteria)
        {
            return TryFind(criteria, out T entity);
        }

        public bool TryFind(ISpecification<T> criteria, out T entity)
        {
            return TryFind(criteria, (IQueryOptions<T>)null, out entity);
        }

        public bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity)
        {
            entity = null;

            try
            {
                entity = Find(criteria, queryOptions);
                return entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return TryFind(criteria, selector, null, out entity);
        }

        public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            entity = default(TResult);

            try
            {
                entity = Find(criteria, selector, queryOptions);
                return !entity.Equals(default(TResult));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public abstract TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);

        public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");

            return Find(new Specification<T>(predicate), queryOptions);
        }

        public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (selector == null) throw new ArgumentNullException("selector");

            return Find(new Specification<T>(predicate), selector, queryOptions);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return TryFind(predicate, out T entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, out T entity)
        {
            return TryFind(predicate, (IQueryOptions<T>)null, out entity);
        }

        public bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity)
        {
            entity = null;

            try
            {
                entity = Find(predicate, queryOptions);
                return entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            return TryFind(predicate, selector, null, out entity);
        }

        public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
        {
            entity = default(TResult);

            try
            {
                entity = Find(predicate, selector, queryOptions);
                return !entity.Equals(default(TResult));
            }
            catch (Exception)
            {
                return false;
            }
        }

        // This is the actual implementation that the derived class needs to implement
        protected abstract void AddItem(T entity);

        public abstract void Add(T entity);

        public void Add(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        // This is the actual implementation that the derived class needs to implement
        protected abstract void DeleteItem(T entity);

        public abstract void Delete(T entity);

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Delete(new Specification<T>(predicate));
        }

        public void Delete(ISpecification<T> criteria)
        {
            Delete(FindAll(criteria));
        }

        // This is the actual implementation that the derived class needs to implement
        protected abstract void UpdateItem(T entity);

        public void Update(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public abstract void Update(T entity);

        protected abstract void SaveChanges();

        public abstract void Dispose();

        protected virtual PropertyInfo[] GetPrimaryKeyPropertyInfo()
        {
            var type = typeof(T);
            var properties = type.GetTypeInfo().DeclaredProperties;

            return properties.Where(x => x.HasAttribute<RepositoryPrimaryKeyAttribute>()).OrderBy(x => x.GetOneAttribute<RepositoryPrimaryKeyAttribute>().Order).ToArray();
        }
    }
}
