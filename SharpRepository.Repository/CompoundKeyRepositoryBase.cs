using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Repository.Transactions;

namespace SharpRepository.Repository
{
    public abstract partial class CompoundKeyRepositoryBase<T> : CompoundKeyRepositoryBaseCommon<T>, ICompoundKeyRepository<T> 
        where T : class
    {
        // the caching strategy used
        private ICompoundKeyCachingStrategy<T> _cachingStrategy;

        // the query manager uses the caching strategy to determine if it should check the cache or run the query
        private CompoundKeyQueryManager<T> _queryManager;

        public override bool CacheUsed
        {
            get { return _queryManager.CacheUsed; }
        }

        public override IBatch<T> BeginBatch()
        {
            // Return the privately scoped batch via the publicly available interface. 
            // This ensures that a repository alone can initiate a new batch.
            return new Batch(this);
        }

        protected CompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T> cachingStrategy = null)
        {
            CachingStrategy = cachingStrategy ?? new NoCompoundKeyCachingStrategy<T>();
        }

        public ICompoundKeyCachingStrategy<T> CachingStrategy
        {
            get { return _cachingStrategy; }
            set
            {
                _cachingStrategy = value ?? new NoCompoundKeyCachingStrategy<T>();
                _queryManager = new CompoundKeyQueryManager<T>(_cachingStrategy);
            }
        }

        public override IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return _queryManager.ExecuteGetAll(
                () => GetAllQuery(queryOptions, fetchStrategy).ToList(),
                null,
                queryOptions
                );
        }

        public override IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return _queryManager.ExecuteGetAll(
                () => GetAllQuery(queryOptions, fetchStrategy).Select(selector).ToList(),
                selector,
                queryOptions
                );
        }

        // These are the actual implementation that the derived class needs to implement
        protected abstract T GetQuery(params object[] keys);

        public bool Exists(params object[] keys)
        {
            return TryGet(out T entity, keys);
        }

        public bool TryGet(out T entity, params object[] keys)
        {
            entity = default(T);

            try
            {
                entity = Get(keys);
                return entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public T Get(params object[] keys)
        {
            return _queryManager.ExecuteGet(
                () => GetQuery(keys),
                null,
                keys
                );
        }

        public TResult Get<TResult>(Expression<Func<T, TResult>> selector, params object[] keys)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return _queryManager.ExecuteGet(
                () =>
                {
                    var result = GetQuery(keys);
                    if (result == null)
                        return default(TResult);

                    var results = new[] { result };
                    return results.AsQueryable().Select(selector).First();
                },
                selector,
                keys
                );
        }

        public override IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFindAll(
                () => FindAllQuery(criteria, queryOptions).ToList(),
                criteria,
                null,
                queryOptions
                );
        }

        public override IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFindAll(
                () => FindAllQuery(criteria, queryOptions).Select(selector).ToList(),
                criteria,
                selector,
                queryOptions
                );
        }
        
        public override T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFind(
                () => FindQuery(criteria, queryOptions),
                criteria,
                null,
                null
                );
        }

        public override TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
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

        public override void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            AddItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKeys(entity, out object[] keys))
                _queryManager.OnItemAdded(keys, entity);
        }

        public void Delete(params object[] keys)
        {
            var entity = Get(keys);

            if (entity == null) throw new ArgumentException("No entity exists with these keys.", "keys");

            Delete(entity);
        }

        public override void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            DeleteItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKeys(entity, out object[] keys))
                _queryManager.OnItemDeleted(keys, entity);
        }

        public override void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            UpdateItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKeys(entity, out object[] keys))
                _queryManager.OnItemUpdated(keys, entity);
        }

        protected virtual bool GetPrimaryKeys(T entity, out object[] keys)
        {
            keys = null;
            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null)
                return false;

            keys = propInfo.Select(info => info.GetValue(entity, null)).ToArray();

            return true;
        }

        protected virtual bool SetPrimaryKey(T entity, object[] keys)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null || keys == null || propInfo.Length != keys.Length)
                return false;

            var i = 0;
            foreach (var key in keys)
            {
                propInfo[i].SetValue(entity, key, null);
                i++;
            }

            return true;
        }

        protected virtual ISpecification<T> ByPrimaryKeySpecification(object[] keys)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();
            if (propInfo == null || keys == null || propInfo.Length != keys.Length)
                return null;

            ISpecification<T> specification = null;
            var parameter = Expression.Parameter(typeof(T), "x");

            var i = 0;
            foreach (var lambda in keys.Select(key => Expression.Lambda<Func<T, bool>>(
                        Expression.Equal(
                            Expression.PropertyOrField(parameter, propInfo[i].Name),
                            Expression.Constant(key)
                        ),
                        parameter
                    ))
                )
            {
                specification = specification == null ? new Specification<T>(lambda) : specification.And(lambda);
                i++;
            }

            return specification;
        }

        private void Save()
        {
            SaveChanges();

            _queryManager.OnSaveExecuted();
        }
    }

    public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2> 
        : CompoundKeyRepositoryBaseCommon<T>, ICompoundKeyRepository<T, TKey, TKey2> where T : class
    {
        // the caching strategy used
        private ICompoundKeyCachingStrategy<T, TKey, TKey2> _cachingStrategy;

        // the query manager uses the caching strategy to determine if it should check the cache or run the query
        private CompoundKeyQueryManager<T, TKey, TKey2> _queryManager;
                
        public override bool CacheUsed
        {
            get { return _queryManager.CacheUsed; }
        }

        public override IBatch<T> BeginBatch()
        {
            // Return the privately scoped batch via the publicly available interface. 
            // This ensures that a repository alone can initiate a new batch.
            return new Batch(this);
        }

        protected CompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
        {
            if (typeof(T) == typeof(TKey))
            {
                // this check is mainly because of the overloaded Delete methods Delete(T) and Delete(TKey), ambiguous reference if the generics are the same
                throw new InvalidOperationException("The repository type and the primary key type can not be the same.");
            }

            CachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey, TKey2>();
        }

        public ICompoundKeyCachingStrategy<T, TKey, TKey2> CachingStrategy 
        {
            get { return _cachingStrategy; } 
            set
            {
                _cachingStrategy = value ?? new NoCachingStrategy<T, TKey, TKey2>();
                _queryManager = new CompoundKeyQueryManager<T, TKey, TKey2>(_cachingStrategy);
            }
        } 
        
        public override IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return _queryManager.ExecuteGetAll(
                () => GetAllQuery(queryOptions, fetchStrategy).ToList(),
                null,
                queryOptions
                );
        }

        public override IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return _queryManager.ExecuteGetAll(
                () => GetAllQuery(queryOptions, fetchStrategy).Select(selector).ToList(),
                selector,
                queryOptions
                );
        }

        // These are the actual implementation that the derived class needs to implement
        protected abstract T GetQuery(TKey key, TKey2 key2);

        public T Get(TKey key, TKey2 key2)
        {
            return _queryManager.ExecuteGet(
                () => GetQuery(key, key2),
                null,
                key,
                key2
                );
        }

        public TResult Get<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return _queryManager.ExecuteGet(
                () =>
                {
                    var result = GetQuery(key, key2);
                    if (result == null)
                        return default(TResult);

                    var results = new[] { result };
                    return results.AsQueryable().Select(selector).First();
                },
                selector,
                key,
                key2
                );
        }

        public bool Exists(TKey key, TKey2 key2)
        {
            return TryGet(key, key2, out T entity);
        }

        public bool TryGet(TKey key, TKey2 key2, out T entity)
        {
            entity = default(T);

            try
            {
                entity = Get(key, key2);
                return entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryGet<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            entity = default(TResult);

            try
            {
                entity = Get(key, key2, selector);
                return !entity.Equals(default(TResult));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFindAll(
                () => FindAllQuery(criteria, queryOptions).ToList(),
                criteria,
                null,
                queryOptions
                );
        }

        public override IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFindAll(
                () => FindAllQuery(criteria, queryOptions).Select(selector).ToList(),
                criteria,
                selector,
                queryOptions
                );
        }

        public override T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFind(
                () => FindQuery(criteria, queryOptions),
                criteria,
                null,
                null
                );
        }

        public override TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
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

        public override void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            AddItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKey(entity, out TKey key, out TKey2 key2))
                _queryManager.OnItemAdded(key, key2, entity);
        }

        public override void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            DeleteItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKey(entity, out TKey key, out TKey2 key2))
                _queryManager.OnItemDeleted(key, key2, entity);
        }

        public void Delete(TKey key, TKey2 key2)
        {
            var entity = Get(key, key2);

            if (entity == null) throw new ArgumentException("No entity exists with this key.", "key");

            Delete(entity);
        }

        public override void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            UpdateItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKey(entity, out TKey key, out TKey2 key2))
                _queryManager.OnItemUpdated(key, key2, entity);
        }

        private void Save()
        {
            SaveChanges();
            
            _queryManager.OnSaveExecuted(); 
        }

        protected virtual bool GetPrimaryKey(T entity, out TKey key, out TKey2 key2) 
        {
            key = default(TKey);
            key2 = default(TKey2);

            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null || propInfo.Length != 2)
                return false;

            key = (TKey) propInfo[0].GetValue(entity, null);
            key2 = (TKey2) propInfo[1].GetValue(entity, null);
           
           return true;
        }

        protected virtual bool SetPrimaryKey(T entity, TKey key, TKey2 key2)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null || propInfo.Length != 2)
                return false;

            propInfo[0].SetValue(entity, key, null);
            propInfo[1].SetValue(entity, key2, null);

            return true;
        }

        protected virtual ISpecification<T> ByPrimaryKeySpecification(TKey key, TKey2 key2)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();
            if (propInfo == null || propInfo.Length != 2)
                return null;

            var parameter = Expression.Parameter(typeof(T), "x");
            var lambda = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, propInfo[0].Name),
                        Expression.Constant(key)
                    ),
                    parameter
                );
            var lambda2 = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, propInfo[1].Name),
                        Expression.Constant(key2)
                    ),
                    parameter
                );

            return new Specification<T>(lambda).And(lambda2);
        }
    }

    public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> 
        : CompoundKeyRepositoryBaseCommon<T>, ICompoundKeyRepository<T, TKey, TKey2, TKey3> where T : class
    {
        // the caching strategy used
        private ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> _cachingStrategy;

        // the query manager uses the caching strategy to determine if it should check the cache or run the query
        private CompoundKeyQueryManager<T, TKey, TKey2, TKey3> _queryManager;
                
        public override bool CacheUsed
        {
            get { return _queryManager.CacheUsed; }
        }

        public override IBatch<T> BeginBatch()
        {
            // Return the privately scoped batch via the publicly available interface. 
            // This ensures that a repository alone can initiate a new batch.
            return new Batch(this);
        }

        protected CompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
        {
            if (typeof(T) == typeof(TKey))
            {
                // this check is mainly because of the overloaded Delete methods Delete(T) and Delete(TKey), ambiguous reference if the generics are the same
                throw new InvalidOperationException("The repository type and the primary key type can not be the same.");
            }

            CachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey, TKey2, TKey3>();
        }

        public ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> CachingStrategy 
        {
            get { return _cachingStrategy; } 
            set
            {
                _cachingStrategy = value ?? new NoCachingStrategy<T, TKey, TKey2, TKey3>();
                _queryManager = new CompoundKeyQueryManager<T, TKey, TKey2, TKey3>(_cachingStrategy);
            }
        }

        public override IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            return _queryManager.ExecuteGetAll(
                () => GetAllQuery(queryOptions, fetchStrategy).ToList(),
                null,
                queryOptions
                );
        }

        public override IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return _queryManager.ExecuteGetAll(
                () => GetAllQuery(queryOptions, fetchStrategy).Select(selector).ToList(),
                selector,
                queryOptions
                );
        }

        protected abstract T GetQuery(TKey key, TKey2 key2, TKey3 key3);

        public T Get(TKey key, TKey2 key2, TKey3 key3)
        {
            return _queryManager.ExecuteGet(
                () => GetQuery(key, key2, key3),
                null,
                key,
                key2,
                key3
                );
        }

        public TResult Get<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return _queryManager.ExecuteGet(
                () =>
                {
                    var result = GetQuery(key, key2, key3);
                    if (result == null)
                        return default(TResult);

                    var results = new[] { result };
                    return results.AsQueryable().Select(selector).First();
                },
                selector,
                key,
                key2,
                key3
                );
        }

        public bool Exists(TKey key, TKey2 key2, TKey3 key3)
        {
            return TryGet(key, key2, key3, out T entity);
        }

        public bool TryGet(TKey key, TKey2 key2, TKey3 key3, out T entity)
        {
            entity = default(T);

            try
            {
                entity = Get(key, key2, key3);
                return entity != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryGet<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, out TResult entity)
        {
            entity = default(TResult);

            try
            {
                entity = Get(key, key2, key3, selector);
                return !entity.Equals(default(TResult));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFindAll(
                () => FindAllQuery(criteria, queryOptions).ToList(),
                criteria,
                null,
                queryOptions
                );
        }

        public override IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFindAll(
                () => FindAllQuery(criteria, queryOptions).Select(selector).ToList(),
                criteria,
                selector,
                queryOptions
                );
        }

        public override T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) throw new ArgumentNullException("criteria");

            return _queryManager.ExecuteFind(
                () => FindQuery(criteria, queryOptions),
                criteria,
                null,
                null
                );
        }

        public override TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
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

        public override void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            AddItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3))
                _queryManager.OnItemAdded(key, key2, key3, entity);
        }

        public override void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            DeleteItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3))
                _queryManager.OnItemDeleted(key, key2, key3, entity);
        }

        public void Delete(TKey key, TKey2 key2, TKey3 key3)
        {
            var entity = Get(key, key2, key3);

            if (entity == null) throw new ArgumentException("No entity exists with these keys.", "key");

            Delete(entity);
        }

        public override void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            UpdateItem(entity);
            if (BatchMode) return;

            Save();

            if (GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3))
                _queryManager.OnItemUpdated(key, key2, key3, entity);
        }

        private void Save()
        {
            SaveChanges();
            
            _queryManager.OnSaveExecuted(); 
        }

        protected virtual bool GetPrimaryKey(T entity, out TKey key, out TKey2 key2, out TKey3 key3) 
        {
            key = default(TKey);
            key2 = default(TKey2);
            key3 = default(TKey3);

            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null || propInfo.Length != 3)
                return false;

            key = (TKey) propInfo[0].GetValue(entity, null);
            key2 = (TKey2) propInfo[1].GetValue(entity, null);
            key3 = (TKey3) propInfo[2].GetValue(entity, null);
           
           return true;
        }

        protected virtual bool SetPrimaryKey(T entity, TKey key, TKey2 key2, TKey3 key3)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null || propInfo.Length != 3)
                return false;

            propInfo[0].SetValue(entity, key, null);
            propInfo[1].SetValue(entity, key2, null);
            propInfo[2].SetValue(entity, key3, null);

            return true;
        }

        protected virtual ISpecification<T> ByPrimaryKeySpecification(TKey key, TKey2 key2, TKey3 key3)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();
            if (propInfo == null || propInfo.Length != 3)
                return null;

            var parameter = Expression.Parameter(typeof(T), "x");
            var lambda = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, propInfo[0].Name),
                        Expression.Constant(key)
                    ),
                    parameter
                );
            var lambda2 = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, propInfo[1].Name),
                        Expression.Constant(key2)
                    ),
                    parameter
                );
            var lambda3 = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, propInfo[2].Name),
                        Expression.Constant(key3)
                    ),
                    parameter
                );

            return new Specification<T>(lambda).And(lambda2).And(lambda3);
        }
    }
}
