using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Aspects;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Repository.Transactions;

namespace SharpRepository.Repository
{
    public abstract partial class RepositoryBase<T, TKey> : IRepository<T, TKey> where T : class
    {
        protected RepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null)
        {
            if (typeof(T) == typeof(TKey))
            {
                // this check is mainly because of the overloaded Delete methods Delete(T) and Delete(TKey), ambiguous reference if the generics are the same
                throw new InvalidOperationException("The repository type and the primary key type can not be the same.");
            }

            GenerateKeyOnAdd = true;
            Conventions = new RepositoryConventions();
            CachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey>(); // sets QueryManager as well
            // the CachePrefix is set to the default convention in the CachingStrategyBase class, the user to override when passing in an already created CachingStrategy class

            var entityType = typeof(T);
            _typeName = entityType.Name;
            _aspects = entityType.GetTypeInfo().GetAllAttributes<RepositoryActionBaseAttribute>(inherit: true)
                .OrderBy(x => x.Order)
                .ToDictionary(a => a.GetType().FullName, a => a);

            _repositoryActionContext = new RepositoryActionContext<T, TKey>(this);
            RunAspect(aspect => aspect.OnInitialized(_repositoryActionContext));
        }

        // the caching strategy used
        private ICachingStrategy<T, TKey> _cachingStrategy;

        // the query manager uses the caching strategy to determine if it should check the cache or run the query
        protected QueryManager<T, TKey> QueryManager;

        // conventions
        public IRepositoryConventions Conventions { get; set; }

        private readonly RepositoryActionContext<T, TKey> _repositoryActionContext;
        private readonly Dictionary<string, RepositoryActionBaseAttribute> _aspects;

        // For purposes of testing
        protected IEnumerable<RepositoryActionBaseAttribute> Aspects
        {
            get { return _aspects.Values; }
        }

        public Type EntityType
        {
            get { return typeof(T); }
        }

        public Type KeyType
        {
            get { return typeof(TKey); }
        }

        // just the type name, used to find the primary key if it is [TypeName]Id
        private readonly string _typeName;
        protected string TypeName
        {
            get { return _typeName; }
        }

        public bool CacheUsed
        {
            get { return QueryManager.CacheUsed; }
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
                QueryManager = new QueryManager<T, TKey>(_cachingStrategy)
                {
                    CacheEnabled = !(_cachingStrategy is NoCachingStrategy<T, TKey>)
                };
            }
        }

        public bool CachingEnabled
        {
            get { return QueryManager.CacheEnabled; }
            set { QueryManager.CacheEnabled = value; }
        }

        protected abstract IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null);

        public abstract IQueryable<T> AsQueryable();

        // These are the actual implementation that the derived class needs to implement
        protected abstract IQueryable<T> GetAllQuery(IFetchStrategy<T> fetchStrategy);
        protected abstract IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

        //Managing aspects
        protected void DisableAspect(Type aspectType)
        {
            ValidateArgument(aspectType);
            var aspect = _aspects[aspectType.FullName];
            aspect.Enabled = false;
        }

        protected void EnableAspect(Type aspectType)
        {
            ValidateArgument(aspectType);
            var aspect = _aspects[aspectType.FullName];
            aspect.Enabled = true;
        }

        private void ValidateArgument(Type aspectType)
        {
            var baseAttribute = typeof(RepositoryActionBaseAttribute);

            if (!baseAttribute.GetTypeInfo().IsAssignableFrom(aspectType.GetTypeInfo()))
                throw new ArgumentException(string.Format("Only aspects derived from a type {0} are valid arguments", baseAttribute.Name));

            if (!_aspects.ContainsKey(aspectType.FullName))
                throw new InvalidOperationException(string.Format("There is no aspect of a type {0}", aspectType.Name));
        }

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

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            try
            {
                var context = new RepositoryQueryMultipleContext<T, TKey>(this, null, queryOptions);
                if (!RunAspect(attribute => attribute.OnGetAllExecuting(context)))
                    return context.Results;

                // if the aspect altered the specificaiton then we need to run a FindAll with that specification
                IEnumerable<T> results;

                if (context.Specification == null)
                {
                    results = QueryManager.ExecuteGetAll(
                        () => GetAllQuery(context.QueryOptions, fetchStrategy).ToList(),
                        null,
                        context.QueryOptions
                        );
                }
                else
                {
                    context.Specification.FetchStrategy = fetchStrategy;

                    results = QueryManager.ExecuteFindAll(
                        () => FindAllQuery(context.Specification, context.QueryOptions).ToList(),
                        context.Specification,
                        null,
                        context.QueryOptions
                        );
                }

                context.Results = results;
                RunAspect(attribute => attribute.OnGetAllExecuted(context));

                return context.Results;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

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

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            try
            {
                if (selector == null) throw new ArgumentNullException("selector");

                var context = new RepositoryQueryMultipleContext<T, TKey, TResult>(this, null, queryOptions, selector);
                if (!RunAspect(attribute => attribute.OnGetAllExecuting(context)))
                    return context.Results;

                // if the aspect altered the specificaiton then we need to run a FindAll with that specification
                IEnumerable<TResult> results;
                var selectFunc = context.Selector.Compile();

                if (context.Specification == null)
                {
                    results = QueryManager.ExecuteGetAll(
                        () => GetAllQuery(context.QueryOptions, fetchStrategy).Select(selectFunc).ToList(),
                        context.Selector,
                        context.QueryOptions
                        );
                }
                else
                {
                    context.Specification.FetchStrategy = fetchStrategy;

                    results = QueryManager.ExecuteFindAll(
                        () => FindAllQuery(context.Specification, context.QueryOptions).Select(selectFunc).ToList(),
                        context.Specification,
                        context.Selector,
                        context.QueryOptions
                        );
                }

                context.Results = results;
                RunAspect(attribute => attribute.OnGetAllExecuted(context));

                return context.Results;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
        {
            return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
        {
            return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
        }



        // These are the actual implementation that the derived class needs to implement
        protected abstract T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy);

        public abstract IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class;

        public T Get(TKey key, params Expression<Func<T, object>>[] includePaths)
        {
            var fetchStrategy = new GenericFetchStrategy<T>();
            foreach (var path in includePaths)
            {
                fetchStrategy.Include(path);
            }

            return Get(key, fetchStrategy);
        }

        public T Get(TKey key, params string[] includePaths)
        {
            return Get(key, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public T Get(TKey key)
        {
            return Get(key, (IFetchStrategy<T>)null);
        }

        public T Get(TKey key, IFetchStrategy<T> fetchStrategy)
        {
            try
            {
                var context = new RepositoryGetContext<T, TKey>(this, key);
                if (!RunAspect(attribute => attribute.OnGetExecuting(context)))
                    return context.Result;

                var result = QueryManager.ExecuteGet(
                    () => GetQuery(context.Id, fetchStrategy),
                    context.Id
                    );

                context.Result = result;
                RunAspect(attribute => attribute.OnGetExecuted(context));

                return context.Result;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
        {
            return Get(key, selector, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, params string[] includePaths)
        {
            return Get(key, selector, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector)
        {
            return Get(key, selector, (IFetchStrategy<T>)null);
        }

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
        {
            try
            {
                if (selector == null) throw new ArgumentNullException("selector");

                var context = new RepositoryGetContext<T, TKey, TResult>(this, key, selector);
                if (!RunAspect(attribute => attribute.OnGetExecuting(context)))
                    return context.Result;

                // get the full entity, possibly from cache
                var result = QueryManager.ExecuteGet(
                    () => GetQuery(context.Id, fetchStrategy),
                    context.Id
                    );

                // return the entity with the selector applied to it
                var selectFunc = selector.Compile();
                var selectedResult = result == null
                    ? default(TResult)
                    : new[] { result }.AsEnumerable().Select(selectFunc).First();

                context.Result = selectedResult;
                RunAspect(attribute => attribute.OnGetExecuted(context));

                return context.Result;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public virtual IEnumerable<T> GetMany(params TKey[] keys)
        {
            return GetMany(keys.ToList());
        }

        public virtual IEnumerable<T> GetMany(IEnumerable<TKey> keys)
        {
            return FindAll(ByMultipleKeysSpecification(keys));
        }

        public virtual IEnumerable<TResult> GetMany<TResult>(Expression<Func<T, TResult>> selector, params TKey[] keys)
        {
            return GetMany(keys.ToList(), selector);
        }

        public virtual IEnumerable<TResult> GetMany<TResult>(IEnumerable<TKey> keys, Expression<Func<T, TResult>> selector)
        {
            return FindAll(ByMultipleKeysSpecification(keys), selector);
        }

        public virtual IDictionary<TKey, T> GetManyAsDictionary(params TKey[] keys)
        {
            return GetManyAsDictionary(keys.ToList());
        }

        public virtual IDictionary<TKey, T> GetManyAsDictionary(IEnumerable<TKey> keys)
        {
            return GetMany(keys).ToDictionary(GetPrimaryKey);
        }

        public bool Exists(TKey key)
        {
            return TryGet(key, out T entity);
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
            if (selector == null) throw new ArgumentNullException("selector");

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
            if (criteria == null) return GetAll(queryOptions);

            try
            {
                var context = new RepositoryQueryMultipleContext<T, TKey>(this, criteria, queryOptions);
                if (!RunAspect(attribute => attribute.OnFindAllExecuting(context)))
                    return context.Results;

                var results = QueryManager.ExecuteFindAll(
                    () => FindAllQuery(context.Specification, context.QueryOptions).ToList(),
                    context.Specification,
                    null,
                    context.QueryOptions
                    );

                context.Results = results;
                RunAspect(attribute => attribute.OnFindAllExecuted(context));

                return context.Results;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (criteria == null) return GetAll(selector, queryOptions);

            try
            {
                if (selector == null) throw new ArgumentNullException("selector");

                var context = new RepositoryQueryMultipleContext<T, TKey, TResult>(this, criteria, queryOptions, selector);
                if (!RunAspect(attribute => attribute.OnFindAllExecuting(context)))
                    return context.Results;

                var selectFunc = context.Selector.Compile();
                var results = QueryManager.ExecuteFindAll(
                    () => FindAllQuery(context.Specification, context.QueryOptions).Select(selectFunc).ToList(),
                    context.Specification,
                    context.Selector,
                    context.QueryOptions
                    );

                context.Results = results;
                RunAspect(attribute => attribute.OnFindAllExecuted(context));

                return context.Results;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
        {
            if (predicate == null) return GetAll(queryOptions);

            try
            {
                return FindAll(CreateSpecification(predicate), queryOptions);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (predicate == null) return GetAll(selector, queryOptions);

            try
            {
                return FindAll(CreateSpecification(predicate), selector, queryOptions);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        // These are the actual implementation that the derived class needs to implement
        protected abstract T FindQuery(ISpecification<T> criteria);
        protected abstract T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

        public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
        {
            try
            {
                if (criteria == null) throw new ArgumentNullException("criteria");

                var context = new RepositoryQuerySingleContext<T, TKey>(this, criteria, queryOptions);
                if (!RunAspect(attribute => attribute.OnFindExecuting(context)))
                    return context.Result;

                var item = QueryManager.ExecuteFind(
                    () => FindQuery(context.Specification, context.QueryOptions),
                    context.Specification,
                    null,
                    null
                    );

                context.Result = item;
                RunAspect(attribute => attribute.OnFindExecuted(context));

                return context.Result;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            try
            {
                if (criteria == null) throw new ArgumentNullException("criteria");
                if (selector == null) throw new ArgumentNullException("selector");

                var context = new RepositoryQuerySingleContext<T, TKey, TResult>(this, criteria, queryOptions, selector);
                if (!RunAspect(attribute => attribute.OnFindExecuting(context)))
                    return context.Result;

                var selectFunc = context.Selector.Compile();
                var item = QueryManager.ExecuteFind(
                    () =>
                        {
                            var result = FindQuery(context.Specification, context.QueryOptions);
                            if (result == null)
                                return default(TResult);

                            var results = new[] { result };
                            return results.AsEnumerable().Select(selectFunc).First();
                        },

                    context.Specification,
                    context.Selector,
                    null
                    );

                context.Result = item;
                RunAspect(attribute => attribute.OnFindExecuted(context));

                return context.Result;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

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
            if (selector == null) throw new ArgumentNullException("selector");

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
            try
            {
                if (predicate == null) throw new ArgumentNullException("predicate");

                return Find(CreateSpecification(predicate), queryOptions);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            try
            {
                if (predicate == null) throw new ArgumentNullException("predicate");
                if (selector == null) throw new ArgumentNullException("selector");

                return Find(CreateSpecification(predicate), selector, queryOptions);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
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
            if (selector == null) throw new ArgumentNullException("selector");

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

        // TODO: allowing ordering of grouped results
        public IEnumerable<TResult> GroupBy<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
        {
            return GroupBy((ISpecification<T>)null, keySelector, resultSelector);
        }

        public virtual IEnumerable<TResult> GroupBy<TGroupKey, TResult>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
        {
            var predicate = criteria?.Predicate?.Compile();
            var keySelectFunc = keySelector.Compile();
            var resultSelectFunc = resultSelector.Compile();
            return QueryManager.ExecuteGroup(
                () =>
                {
                    var query = criteria == null ? BaseQuery() : BaseQuery().Where(predicate);

                    //                            if (queryOptions != null)
                    //                                query = queryOptions.Apply(query);

                    return query.GroupBy(keySelectFunc).OrderBy(x => x.Key).Select(resultSelectFunc).ToList();
                },
                keySelector,
                resultSelector,
                criteria
                );
        }

        public IEnumerable<TResult> GroupBy<TGroupKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
        {
            return GroupBy(predicate == null ? null : CreateSpecification(predicate), keySelector, resultSelector);
        }

        public long LongCount()
        {
            return LongCount((ISpecification<T>)null);
        }

        public virtual long LongCount(ISpecification<T> criteria)
        {
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteLongCount(
                () => criteria == null ? BaseQuery().LongCount() : BaseQuery().LongCount(predicate),
                criteria
                );
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            return LongCount(predicate == null ? null : CreateSpecification(predicate));
        }

        public int Count()
        {
            return Count((ISpecification<T>)null);
        }

        public virtual int Count(ISpecification<T> criteria)
        {
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteCount(
                () => criteria == null ? BaseQuery().Count() : BaseQuery().Count(predicate),
                criteria
                );
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate == null ? null : CreateSpecification(predicate));
        }

        public int Sum(Expression<Func<T, int>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual int Sum(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public int Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public int? Sum(Expression<Func<T, int?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual int? Sum(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public int? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public long Sum(Expression<Func<T, long>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual long Sum(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public long Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public long? Sum(Expression<Func<T, long?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual long? Sum(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public long? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public decimal Sum(Expression<Func<T, decimal>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual decimal Sum(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public decimal Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public decimal? Sum(Expression<Func<T, decimal?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual decimal? Sum(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public decimal? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public double Sum(Expression<Func<T, double>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual double Sum(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public double Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public double? Sum(Expression<Func<T, double?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual double? Sum(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public double? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double?>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public float Sum(Expression<Func<T, float>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual float Sum(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public float Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public float? Sum(Expression<Func<T, float?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual float? Sum(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selectFunc) : BaseQuery().Where(predicate).Sum(selectFunc),
                selector,
                criteria
                );
        }

        public float? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
        {
            return Sum(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public double Average(Expression<Func<T, int>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double Average(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public double? Average(Expression<Func<T, int?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double? Average(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public double Average(Expression<Func<T, long>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double Average(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public double? Average(Expression<Func<T, long?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double? Average(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public decimal Average(Expression<Func<T, decimal>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual decimal Average(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public decimal Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public decimal? Average(Expression<Func<T, decimal?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual decimal? Average(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public decimal? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public double Average(Expression<Func<T, double>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double Average(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public double? Average(Expression<Func<T, double?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double? Average(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double?>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public float Average(Expression<Func<T, float>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual float Average(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public float Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public float? Average(Expression<Func<T, float?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual float? Average(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selectFunc) : BaseQuery().Where(predicate).Average(selectFunc),
                selector,
                criteria
                );
        }

        public float? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
        {
            return Average(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public TResult Min<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Min((ISpecification<T>)null, selector);
        }

        public virtual TResult Min<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteMin(
                () => criteria == null ? BaseQuery().Min(selectFunc) : BaseQuery().Where(predicate).Min(selectFunc),
                selector,
                criteria
                );
        }

        public TResult Min<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return Min(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public TResult Max<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Max((ISpecification<T>)null, selector);
        }

        public virtual TResult Max<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            var selectFunc = selector.Compile();
            var predicate = criteria?.Predicate?.Compile();
            return QueryManager.ExecuteMax(
                () => criteria == null ? BaseQuery().Max(selectFunc) : BaseQuery().Where(predicate).Max(selectFunc),
                selector,
                criteria
                );
        }

        public TResult Max<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return Max(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public IDictionary<TGroupKey, int> GroupCount<TGroupKey>(Expression<Func<T, TGroupKey>> selector)
        {
            return GroupCount((ISpecification<T>)null, selector);
        }

        public virtual IDictionary<TGroupKey, int> GroupCount<TGroupKey>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> selector)
        {
            return GroupBy(criteria, selector, x => new { x.Key, Count = x.Count() }).ToDictionary(x => x.Key, x => x.Count);
        }

        public IDictionary<TGroupKey, int> GroupCount<TGroupKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> selector)
        {
            return GroupCount(predicate == null ? null : CreateSpecification(predicate), selector);
        }

        public IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(Expression<Func<T, TGroupKey>> selector)
        {
            return GroupLongCount((ISpecification<T>)null, selector);
        }

        public virtual IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> selector)
        {
            return GroupBy(criteria, selector, x => new { x.Key, Count = x.LongCount() }).ToDictionary(x => x.Key, x => x.Count);
        }

        public IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> selector)
        {
            return GroupLongCount(predicate == null ? null : CreateSpecification(predicate), selector);
        }


        private bool RunAspect(Func<RepositoryActionBaseAttribute, bool> action)
        {
            return _aspects.Values
                .Where(a => a.Enabled)
                .OrderBy(a => a.Order)
                .All(action);
        }

        private void RunAspect(Action<RepositoryActionBaseAttribute> action)
        {
            var aspects = _aspects.Values
                .Where(a => a.Enabled)
                .OrderBy(a => a.Order);

            foreach (var attribute in aspects)
            {
                action(attribute);
            }
        }

        // This is the actual implementation that the derived class needs to implement
        protected abstract void AddItem(T entity);

        public void Add(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                ProcessAdd(entity, BatchMode);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        // used from the Add method above and the Save below for the batch save
        private void ProcessAdd(T entity, bool batchMode)
        {
            if (!RunAspect(attribute => attribute.OnAddExecuting(entity, _repositoryActionContext)))
                return;

            AddItem(entity);

            RunAspect(attribute => attribute.OnAddExecuted(entity, _repositoryActionContext));

            if (batchMode) return;

            Save();

            NotifyQueryManagerOfAddedEntity(entity);
        }

        private void NotifyQueryManagerOfAddedEntity(T entity)
        {
            if (GetPrimaryKey(entity, out TKey key))
                QueryManager.OnItemAdded(key, entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null) throw new ArgumentNullException("entities");

                using (var batch = BeginBatch())
                {
                    foreach (var entity in entities)
                    {
                        batch.Add(entity);
                    }

                    batch.Commit();
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        // This is the actual implementation that the derived class needs to implement
        protected abstract void DeleteItem(T entity);

        public void Delete(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                ProcessDelete(entity, BatchMode);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        // used from the Delete method above and the Save below for the batch save
        private void ProcessDelete(T entity, bool batchMode)
        {
            if (!RunAspect(attribute => attribute.OnDeleteExecuting(entity, _repositoryActionContext)))
                return;

            DeleteItem(entity);

            RunAspect(attribute => attribute.OnDeleteExecuted(entity, _repositoryActionContext));

            if (batchMode) return;

            Save();

            NotifyQueryManagerOfDeletedEntity(entity);
        }

        private void NotifyQueryManagerOfDeletedEntity(T entity)
        {
            if (GetPrimaryKey(entity, out TKey key))
                QueryManager.OnItemDeleted(key, entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public void Delete(IEnumerable<TKey> keys)
        {
            Delete(keys.ToArray());
        }

        public void Delete(params TKey[] keys)
        {
            try
            {
                using (var batch = BeginBatch())
                {
                    foreach (var key in keys)
                    {
                        var entity = Get(key);

                        if (entity == null) throw new ArgumentException("No entity exists with this key.", "key");

                        batch.Delete(entity);
                    }

                    batch.Commit();
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public void Delete(TKey key)
        {
            try
            {
                var entity = Get(key);

                if (entity == null) throw new ArgumentException("No entity exists with this key.", "key");

                Delete(entity);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Delete(CreateSpecification(predicate));
        }

        public void Delete(ISpecification<T> criteria)
        {
            Delete(FindAll(criteria));
        }

        // This is the actual implementation that the derived class needs to implement
        protected abstract void UpdateItem(T entity);

        public void Update(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException("entity");

                ProcessUpdate(entity, BatchMode);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        // used from the Update method above and the Save below for the batch save
        private void ProcessUpdate(T entity, bool batchMode)
        {
            if (!RunAspect(attribute => attribute.OnUpdateExecuting(entity, _repositoryActionContext)))
                return;

            UpdateItem(entity);

            RunAspect(attribute => attribute.OnUpdateExecuted(entity, _repositoryActionContext));

            if (batchMode) return;

            Save();

            NotifyQueryManagerOfUpdatedEntity(entity);
        }

        private void NotifyQueryManagerOfUpdatedEntity(T entity)
        {
            if (GetPrimaryKey(entity, out TKey key))
                QueryManager.OnItemUpdated(key, entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null) throw new ArgumentNullException("entities");

                using (var batch = BeginBatch())
                {
                    foreach (var entity in entities)
                    {
                        batch.Update(entity);
                    }

                    batch.Commit();
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        protected abstract void SaveChanges();

        private void Save()
        {
            try
            {
                if (!RunAspect(attribute => attribute.OnSaveExecuting(_repositoryActionContext)))
                    return;

                SaveChanges();

                QueryManager.OnSaveExecuted();

                RunAspect(attribute => attribute.OnSaveExecuted(_repositoryActionContext));
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public abstract void Dispose();

        protected virtual void SetTraceInfo(string caller, string info, bool append = false)
        {
            var formatted = String.Format("[{0}] {1}: {2}", this.GetType().FullName, caller, info);

            TraceInfo = append ? TraceInfo + "\n" + formatted : formatted;
        }

        protected virtual void SetTraceInfo(string caller, IQueryable query, bool append = false)
        {
            SetTraceInfo(caller, query.ToString(), append);
        }
        public string TraceInfo { get; protected set; }

        public virtual bool GenerateKeyOnAdd { get; set; }

        public TKey GetPrimaryKey(T entity)
        {
            if (GetPrimaryKey(entity, out TKey key))
            {
                return key;
            }

            return default(TKey);
        }

        protected virtual bool GetPrimaryKey(T entity, out TKey key)
        {
            key = default(TKey);

            var propInfo = GetPrimaryKeyPropertyInfo();

            // if there is no property that matches then return false
            if (propInfo == null)
                return false;

            key = (TKey)propInfo.GetValue(entity, null);

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

        protected virtual ISpecification<T> ByPrimaryKeySpecification(TKey key, IFetchStrategy<T> fetchStrategy = null)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();

            var parameter = Expression.Parameter(typeof(T), "x");
            var lambda = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, propInfo.Name),
                        Expression.Constant(key)
                    ),
                    parameter
                );

            var spec = CreateSpecification(lambda);

            if (fetchStrategy != null)
            {
                spec.FetchStrategy = fetchStrategy;
            }

            return spec;
        }

        protected virtual Specification<T> CreateSpecification(Expression<Func<T, bool>> lambda)
        {
            return new Specification<T>(lambda);
        }

        protected virtual ISpecification<T> ByMultipleKeysSpecification(IEnumerable<TKey> keys)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();
            if (propInfo == null || keys == null)
                return null;

            var parameter = Expression.Parameter(typeof(T), "x");

            return keys.Select(key =>
                    Expression.Lambda<Func<T, bool>>(
                        Expression.Equal(
                            Expression.PropertyOrField(parameter, propInfo.Name),
                            Expression.Constant(key)
                        ), parameter
                    )
                )
                .Aggregate<Expression<Func<T, bool>>, ISpecification<T>>(null,
                    (current, lambda) => current == null ? CreateSpecification(lambda) : current.Or(lambda)
                );
        }

        protected virtual PropertyInfo GetPrimaryKeyPropertyInfo()
        {
            // checks for properties in this order that match TKey type
            //  1) RepositoryPrimaryKeyAttribute
            //  2) Id
            //  3) [Type Name]Id
            var type = typeof(T);
            var pkType = typeof(TKey);
            var tupleKey = Tuple.Create(type, pkType);

            // check the static cache, this means that the reflection is only done the first time this repository type is used
            //  big performance gain from this - over 3 times faster after first load
            if (InternalCache.PrimaryKeyMapping.ContainsKey(tupleKey))
            {
                return InternalCache.PrimaryKeyMapping[tupleKey];
            }

            var propertyName = Conventions.GetPrimaryKeyName(type);

            if (String.IsNullOrEmpty(propertyName)) return null;

            var propInfo = type.GetTypeInfo().GetDeclaredProperty(propertyName);
            while (propInfo == null && type.GetTypeInfo().BaseType != null)
            {
                type = type.GetTypeInfo().BaseType;
                propInfo = type.GetTypeInfo().GetDeclaredProperty(propertyName);
            }
            propInfo = propInfo == null || propInfo.PropertyType != pkType ? null : propInfo;

            InternalCache.PrimaryKeyMapping[tupleKey] = propInfo;
            return propInfo;
        }

        private void Error(Exception ex)
        {
            RunAspect(aspect => aspect.OnError(new RepositoryActionContext<T, TKey>(this), ex));
        }
    }
}
