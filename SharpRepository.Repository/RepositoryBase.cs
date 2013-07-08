using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Repository.Transactions;

namespace SharpRepository.Repository
{
    public abstract partial class RepositoryBase<T, TKey> : IRepository<T, TKey> where T : class
    {
        // the caching strategy used
        private ICachingStrategy<T, TKey> _cachingStrategy;

        // the query manager uses the caching strategy to determine if it should check the cache or run the query
        private QueryManager<T, TKey> _queryManager;

        private readonly Type _entityType;

        protected RepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null)
        {
            if (typeof(T) == typeof(TKey))
            {
                // this check is mainly because of the overloaded Delete methods Delete(T) and Delete(TKey), ambiguous reference if the generics are the same
                throw new InvalidOperationException("The repository type and the primary key type can not be the same.");
            }

            Conventions = new RepositoryConventions();
            CachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey>();
            // the CachePrefix is set to the default convention in the CachingStrategyBase class, the user to override when passing in an already created CachingStrategy class

            _entityType = typeof(T);
            _typeName = _entityType.Name;
            
        }

        // conventions
        public IRepositoryConventions Conventions { get; set; }

        // just the type name, used to find the primary key if it is [TypeName]Id
        private readonly string _typeName;
        protected string TypeName
        {
            get { return _typeName; }
        }
        
        public bool CacheUsed
        {
            get { return _queryManager.CacheUsed; }
        }

        public IBatch<T> BeginBatch()
        {
            // Return the privately scoped batch via the publicly available interface. 
            // This ensures that a repository alone can initiate a new batch.
            return new Batch(this);
        }

        public IDisabledCache DisableCaching()
        {
            // Return the privately scoped batch via the publicly available interface. 
            // This ensures that a repository alone can initiate a new batch.
            return new DisabledCache(this);
        }

        public void ClearCache()
        {
            CachingStrategy.ClearAll();
        }
      
        private bool BatchMode { get; set; }

        public ICachingStrategy<T, TKey> CachingStrategy 
        {
            get { return _cachingStrategy; } 
            set
            {
                _cachingStrategy = value ?? new NoCachingStrategy<T, TKey>();

                // make sure we keep the curent caching enabled status
                var cachingEnabled = _queryManager == null || _queryManager.CacheEnabled;
                _queryManager = new QueryManager<T, TKey>(_cachingStrategy) {CacheEnabled = cachingEnabled};
            }
        } 

        public bool CachingEnabled
        {
            get { return _queryManager.CacheEnabled; }
            set { _queryManager.CacheEnabled = value; }
        }

        public abstract IQueryable<T> AsQueryable();

        // These are the actual implementation that the derived class needs to implement
        protected abstract IQueryable<T> GetAllQuery();
        protected abstract IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions);

        public IEnumerable<T> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        {
            return _queryManager.ExecuteGetAll(
                () => GetAllQuery(queryOptions).ToList(),
                null,
                queryOptions
                );
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return _queryManager.ExecuteGetAll(
                () =>  GetAllQuery(queryOptions).Select(selector).ToList(),
                selector,
                queryOptions
                );
        }

        // These are the actual implementation that the derived class needs to implement
        protected abstract T GetQuery(TKey key);

        public abstract IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class;

        public T Get(TKey key)
        {
            return _queryManager.ExecuteGet(
                () => GetQuery(key),
                key
                );
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            // get the full entity, possibly from cache
            var result = _queryManager.ExecuteGet(
                () => GetQuery(key),
                key
                );

            // return the entity with the selector applied to it
            return result == null ? default(TResult) : new[] { result }.AsQueryable().Select(selector).First();
        }

        public bool Exists(TKey key)
        {
            T entity;
            return TryGet(key, out entity);
        }

        public bool TryGet(TKey key, out T entity)
        {
            entity = default(T);

            try
            {
                entity = Get(key);
                return entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryGet<TResult>(TKey key, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            entity = default(TResult);

            try
            {
                entity = Get(key, selector);
                return entity.Equals(default(TResult));
            }
            catch (Exception)
            {
                return false;
            }
        }

        // These are the actual implementation that the derived class needs to implement
        protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria);
        protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

        public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFindAll(
                () => FindAllQuery(criteria, queryOptions).ToList(),
                criteria,
                null,
                queryOptions
                );
        }

        public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFindAll(
                () => FindAllQuery(criteria, queryOptions).Select(selector).ToList(),
                criteria,
                selector,
                queryOptions
                );
        }

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

        public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFind(
                () => FindQuery(criteria, queryOptions),
                criteria,
                null,
                null
                );
        }

        public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");
            if (selector == null) throw new ArgumentNullException("selector");

            return _queryManager.ExecuteFind(
                () =>
                    {
                        var result = FindQuery(criteria, queryOptions);
                        if (result == null)
                            return default(TResult);

                        var results = new[] { result };
                        return results.AsQueryable().Select(selector).First();
                    },
                criteria,
                selector,
                null
                );
        }

        public bool Exists(ISpecification<T> criteria)
        {
            T entity;
            return TryFind(criteria, out entity);
        }

        public bool TryFind(ISpecification<T> criteria, out T entity)
        {
            return TryFind(criteria, ( IQueryOptions<T>)null, out entity);
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
            T entity;
            return TryFind(predicate, out entity);
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

        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            ProcessAdd(entity, BatchMode);
        }

        // used from the Add method above and the Save below for the batch save
        private void ProcessAdd(T entity, bool batchMode)
        {
            AddItem(entity);
            if (batchMode) return;

            Save();

            TKey key;
            if (GetPrimaryKey(entity, out key))
                _queryManager.OnItemAdded(key, entity);
        }

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

        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            ProcessDelete(entity, BatchMode);
        }

        // used from the Delete method above and the Save below for the batch save
        private void ProcessDelete(T entity, bool batchMode)
        {
            DeleteItem(entity);
            if (batchMode) return;

            Save();

            TKey key;
            if (GetPrimaryKey(entity, out key))
                _queryManager.OnItemDeleted(key, entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Delete(TKey key)
        {
            var entity = Get(key);

            if (entity == null) throw new ArgumentException("No entity exists with this key.", "key");

            Delete(entity);
        }

        // This is the actual implementation that the derived class needs to implement
        protected abstract void UpdateItem(T entity);

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            ProcessUpdate(entity, BatchMode);
        }

        // used from the Update method above and the Save below for the batch save
        private void ProcessUpdate(T entity, bool batchMode)
        {
            UpdateItem(entity);
            if (batchMode) return;

            Save();

            TKey key;
            if (GetPrimaryKey(entity, out key))
                _queryManager.OnItemUpdated(key, entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        protected abstract void SaveChanges();

        private void Save()
        {
            SaveChanges();
            
            _queryManager.OnSaveExecuted(); 
        }

        
        public abstract void Dispose();

        protected virtual bool GetPrimaryKey(T entity, out TKey key) 
        {
            key = default(TKey);

            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null)
                return false;

           key = (TKey) propInfo.GetValue(entity, null);
           
           return true;
        }

        protected virtual bool SetPrimaryKey(T entity, TKey key)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null)
                return false;

            propInfo.SetValue(entity, key, null);

            return true;
        }

        protected virtual ISpecification<T> ByPrimaryKeySpecification(TKey key)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();

            var parameter = Expression.Parameter(typeof (T), "x");
            var lambda = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, propInfo.Name), 
                        Expression.Constant(key)
                    ),
                    parameter
                );

            return new Specification<T>(lambda);
        }

        // TODO: cache this call so it's done on the first loading only
        protected virtual PropertyInfo GetPrimaryKeyPropertyInfo()
        {
            // checks for properties in this order that match TKey type
            //  1) RepositoryPrimaryKeyAttribute
            //  2) Id
            //  3) [Type Name]Id
            var type = typeof(T);
            var pkType = typeof (TKey);

            var propertyName = Conventions.GetPrimaryKeyName(type);

            if (String.IsNullOrEmpty(propertyName)) return null;

            var propInfo = type.GetProperty(propertyName);


            return propInfo == null || propInfo.PropertyType != pkType ? null : propInfo;
        }

//        private static PropertyInfo GetPropertyCaseInsensitive(IReflect type, string propertyName, Type propertyType)
//        {
//            // make the property reflection lookup case insensitive
//            const BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
//
//            return type.GetProperty(propertyName, bindingFlags, null, propertyType, new Type[0], new ParameterModifier[0]);
//        }

        public abstract IEnumerator<T> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract IRepositoryReporting<T> Reporting { get; }
    }
}
