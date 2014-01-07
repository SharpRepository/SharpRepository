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

            Conventions = new RepositoryConventions();
            CachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey>(); // sets QueryManager as well
            // the CachePrefix is set to the default convention in the CachingStrategyBase class, the user to override when passing in an already created CachingStrategy class

            var entityType = typeof(T);
            _typeName = entityType.Name;

            _aspects = entityType.GetAllAttributes<RepositoryActionBaseAttribute>(inherit: true)
                .Where(x => x.Enabled)
                .OrderBy(x => x.Order)
                .ToArray();

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
        private readonly RepositoryActionBaseAttribute[] _aspects;

        public Type EntityType
        {
            get { return typeof (T); }
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

                // make sure we keep the curent caching enabled status
                var cachingEnabled = QueryManager == null || QueryManager.CacheEnabled;
                QueryManager = new QueryManager<T, TKey>(_cachingStrategy) {CacheEnabled = cachingEnabled};
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
        protected abstract IQueryable<T> GetAllQuery();
        protected abstract IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions);

        public IEnumerable<T> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
        {
            try
            {
                var context = new RepositoryQueryMultipleContext<T, TKey>(this, null, queryOptions);
                RunAspect(attribute => attribute.OnGetAllExecuting(context));

                var results = QueryManager.ExecuteGetAll(
                    () => GetAllQuery(queryOptions).ToList(),
                    null,
                    queryOptions
                    );

                context.Results = results;
                RunAspect(attribute => attribute.OnGetAllExecuted(context));

                return results;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
        {
            try
            {
                if (selector == null) throw new ArgumentNullException("selector");

                var context = new RepositoryQueryMultipleContext<T, TKey, TResult>(this, null, queryOptions, selector);
                RunAspect(attribute => attribute.OnGetAllExecuting(context));

                var results = QueryManager.ExecuteGetAll(
                    () =>  GetAllQuery(queryOptions).Select(selector).ToList(),
                    selector,
                    queryOptions
                    );

                context.Results = results;
                RunAspect(attribute => attribute.OnGetAllExecuted(context));

                return results;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
        }

        // These are the actual implementation that the derived class needs to implement
        protected abstract T GetQuery(TKey key);

        public abstract IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class;

        public T Get(TKey key)
        {

            try
            {
                var context = new RepositoryGetContext<T, TKey>(this, key);
                RunAspect(attribute => attribute.OnGetExecuting(context));

                var result = QueryManager.ExecuteGet(
                    () => GetQuery(key),
                    key
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

        public TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector)
        {

            try
            {
                if (selector == null) throw new ArgumentNullException("selector");


                var context = new RepositoryGetContext<T, TKey, TResult>(this, key, selector);
                RunAspect(attribute => attribute.OnGetExecuting(context));



                // get the full entity, possibly from cache
                var result = QueryManager.ExecuteGet(
                    () => GetQuery(key),
                    key
                    );

                // return the entity with the selector applied to it
                var selectedResult = result == null
                    ? default(TResult)
                    : new[] {result}.AsQueryable().Select(selector).First();

                context.Result = selectedResult;
                RunAspect(attribute => attribute.OnGetExecuted(context));

                return selectedResult;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
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
                if (criteria == null) throw new ArgumentNullException("criteria");


                var context = new RepositoryQueryMultipleContext<T, TKey>(this, criteria, queryOptions);
                RunAspect(attribute => attribute.OnFindAllExecuting(context));

                var results = QueryManager.ExecuteFindAll(
                    () => FindAllQuery(criteria, queryOptions).ToList(),
                    criteria,
                    null,
                    queryOptions
                    );

                context.Results = results;
                RunAspect(attribute => attribute.OnFindAllExecuted(context));

                return results;
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
                if (criteria == null) throw new ArgumentNullException("criteria");
                if (selector == null) throw new ArgumentNullException("selector");


                var context = new RepositoryQueryMultipleContext<T, TKey, TResult>(this, criteria, queryOptions, selector);
                RunAspect(attribute => attribute.OnFindAllExecuting(context));

                var results = QueryManager.ExecuteFindAll(
                    () => FindAllQuery(criteria, queryOptions).Select(selector).ToList(),
                    criteria,
                    selector,
                    queryOptions
                    );

                context.Results = results;
                RunAspect(attribute => attribute.OnFindAllExecuted(context));

                return results;
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
                if (predicate == null) throw new ArgumentNullException("predicate");


                return FindAll(new Specification<T>(predicate), queryOptions);
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
                if (predicate == null) throw new ArgumentNullException("predicate");
                if (selector == null) throw new ArgumentNullException("selector");

            	if (predicate == null) return GetAll(selector, queryOptions);

                return FindAll(new Specification<T>(predicate), selector, queryOptions);
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
                RunAspect(attribute => attribute.OnFindExecuting(context));

                var item = QueryManager.ExecuteFind(
                    () => FindQuery(criteria, queryOptions),
                    criteria,
                    null,
                    null
                    );

                context.Result = item;
                RunAspect(attribute => attribute.OnFindExecuted(context));

                return item;
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
                RunAspect(attribute => attribute.OnFindExecuting(context));

                var item = QueryManager.ExecuteFind(
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

                context.Result = item;
                RunAspect(attribute => attribute.OnFindExecuted(context));

                return item;
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
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

                return Find(new Specification<T>(predicate), queryOptions);
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

                return Find(new Specification<T>(predicate), selector, queryOptions);
            }
            catch (Exception ex)
            {
                Error(ex);
                throw;
            }
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
            return QueryManager.ExecuteGroup(
                () =>
                {
                    var query = criteria == null ? BaseQuery() : BaseQuery().Where(criteria.Predicate);

                    //                            if (queryOptions != null)
                    //                                query = queryOptions.Apply(query);

                    return query.GroupBy(keySelector).OrderBy(x => x.Key).Select(resultSelector).ToList();
                },
                keySelector,
                resultSelector,
                criteria
                );
        }

        public IEnumerable<TResult> GroupBy<TGroupKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
        {
            return GroupBy(predicate == null ? null : new Specification<T>(predicate), keySelector, resultSelector);
        }

        public long LongCount()
        {
            return LongCount((ISpecification<T>)null);
        }

        public virtual long LongCount(ISpecification<T> criteria)
        {
            return QueryManager.ExecuteLongCount(
                () => criteria == null ? BaseQuery().LongCount() : BaseQuery().LongCount(criteria.Predicate),
                criteria
                );
        }

        public long LongCount(Expression<Func<T, bool>> predicate)
        {
            return LongCount(predicate == null ? null : new Specification<T>(predicate));
        }

        public int Count()
        {
            return Count((ISpecification<T>)null);
        }

        public virtual int Count(ISpecification<T> criteria)
        {
            return QueryManager.ExecuteCount(
                () => criteria == null ? BaseQuery().Count() : BaseQuery().Count(criteria.Predicate),
                criteria
                );
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate == null ? null : new Specification<T>(predicate));
        }

        public int Sum(Expression<Func<T, int>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual int Sum(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public int Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public int? Sum(Expression<Func<T, int?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual int? Sum(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public int? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public long Sum(Expression<Func<T, long>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual long Sum(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public long Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public long? Sum(Expression<Func<T, long?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual long? Sum(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public long? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public decimal Sum(Expression<Func<T, decimal>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual decimal Sum(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public decimal Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public decimal? Sum(Expression<Func<T, decimal?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual decimal? Sum(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public decimal? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public double Sum(Expression<Func<T, double>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual double Sum(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public double Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public double? Sum(Expression<Func<T, double?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual double? Sum(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public double? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, double?>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public float Sum(Expression<Func<T, float>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual float Sum(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public float Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public float? Sum(Expression<Func<T, float?>> selector)
        {
            return Sum((ISpecification<T>)null, selector);
        }

        public virtual float? Sum(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            return QueryManager.ExecuteSum(
                () => criteria == null ? BaseQuery().Sum(selector) : BaseQuery().Where(criteria.Predicate).Sum(selector),
                selector,
                criteria
                );
        }

        public float? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
        {
            return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public double Average(Expression<Func<T, int>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double Average(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public double? Average(Expression<Func<T, int?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double? Average(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, int?>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public double Average(Expression<Func<T, long>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double Average(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public double? Average(Expression<Func<T, long?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double? Average(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public decimal Average(Expression<Func<T, decimal>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual decimal Average(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public decimal Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public decimal? Average(Expression<Func<T, decimal?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual decimal? Average(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public decimal? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public double Average(Expression<Func<T, double>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double Average(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public double Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public double? Average(Expression<Func<T, double?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual double? Average(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public double? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, double?>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public float Average(Expression<Func<T, float>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual float Average(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public float Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public float? Average(Expression<Func<T, float?>> selector)
        {
            return Average((ISpecification<T>)null, selector);
        }

        public virtual float? Average(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            return QueryManager.ExecuteAverage(
                () => criteria == null ? BaseQuery().Average(selector) : BaseQuery().Where(criteria.Predicate).Average(selector),
                selector,
                criteria
                );
        }

        public float? Average(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
        {
            return Average(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public TResult Min<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Min((ISpecification<T>)null, selector);
        }

        public virtual TResult Min<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            return QueryManager.ExecuteMin(
                () => criteria == null ? BaseQuery().Min(selector) : BaseQuery().Where(criteria.Predicate).Min(selector),
                selector,
                criteria
                );
        }

        public TResult Min<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return Min(predicate == null ? null : new Specification<T>(predicate), selector);
        }

        public TResult Max<TResult>(Expression<Func<T, TResult>> selector)
        {
            return Max((ISpecification<T>)null, selector);
        }

        public virtual TResult Max<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            return QueryManager.ExecuteMax(
                () => criteria == null ? BaseQuery().Max(selector) : BaseQuery().Where(criteria.Predicate).Max(selector),
                selector,
                criteria
                );
        }

        public TResult Max<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return Max(predicate == null ? null : new Specification<T>(predicate), selector);
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
            return GroupCount(predicate == null ? null : new Specification<T>(predicate), selector);
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
            return GroupLongCount(predicate == null ? null : new Specification<T>(predicate), selector);
        }
        

        private bool RunAspect(Func<RepositoryActionBaseAttribute, bool> action)
        {
            return _aspects.All(action);
        }

        private void RunAspect(Action<RepositoryActionBaseAttribute> action)
        {
            foreach (var attribute in _aspects)
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

                if (!RunAspect(attribute => attribute.OnAddExecuting(entity, _repositoryActionContext)))
                    return;

                ProcessAdd(entity, BatchMode);

                RunAspect(attribute => attribute.OnAddExecuted(entity, _repositoryActionContext));
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
            AddItem(entity);
            if (batchMode) return;

            Save();

            TKey key;
            if (GetPrimaryKey(entity, out key))
                QueryManager.OnItemAdded(key, entity);
        }

        public void Add(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null) throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    Add(entity);
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

                if (!RunAspect(attribute => attribute.OnDeleteExecuting(entity, _repositoryActionContext)))
                    return;

                ProcessDelete(entity, BatchMode);

                RunAspect(attribute => attribute.OnDeleteExecuted(entity, _repositoryActionContext));
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
            DeleteItem(entity);
            if (batchMode) return;

            Save();

            TKey key;
            if (GetPrimaryKey(entity, out key))
                QueryManager.OnItemDeleted(key, entity);
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
            Delete(new Specification<T>(predicate));
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

                if (!RunAspect(attribute => attribute.OnUpdateExecuting(entity, _repositoryActionContext)))
                    return;

                ProcessUpdate(entity, BatchMode);

                RunAspect(attribute => attribute.OnUpdateExecuted(entity, _repositoryActionContext));
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
            UpdateItem(entity);
            if (batchMode) return;

            Save();

            TKey key;
            if (GetPrimaryKey(entity, out key))
                QueryManager.OnItemUpdated(key, entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null) throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                {
                    Update(entity);
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

            var propInfo = type.GetProperty(propertyName);
            propInfo = propInfo == null || propInfo.PropertyType != pkType ? null : propInfo;

            InternalCache.PrimaryKeyMapping[tupleKey] = propInfo;
            return propInfo;
        }

        private void Error(Exception ex)
        {
            RunAspect(aspect => aspect.OnError(new RepositoryActionContext<T, TKey>(this), ex));
        }

//        private static PropertyInfo GetPropertyCaseInsensitive(IReflect type, string propertyName, Type propertyType)
//        {
//            // make the property reflection lookup case insensitive
//            const BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
//
//            return type.GetProperty(propertyName, bindingFlags, null, propertyType, new Type[0], new ParameterModifier[0]);
//        }

//        public abstract IEnumerator<T> GetEnumerator();
//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
    }
}
