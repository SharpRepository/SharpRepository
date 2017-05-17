using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository
{
    // TODO: everything is the same as LinqRepositoryBase except the caching strategy and the GetQuery
    public abstract class LinqCompoundKeyRepositoryBase<T> : CompoundKeyRepositoryBase<T> where T : class
    {
        protected LinqCompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }

        public override IQueryable<T> AsQueryable()
        {
            return BaseQuery();
        }

        protected abstract IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null);

        protected override T GetQuery(params object[] keys)
        {
            return FindQuery(ByPrimaryKeySpecification(keys));
        }

        protected override T FindQuery(ISpecification<T> criteria)
        {
            return criteria.SatisfyingEntityFrom(BaseQuery());
        }

        protected override T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindQuery(criteria);

            var query = queryOptions.Apply(BaseQuery());

            return criteria.SatisfyingEntityFrom(query);
        }

        protected override IQueryable<T> GetAllQuery(IFetchStrategy<T> fetchStrategy)
        {
            return BaseQuery(fetchStrategy);
        }

        protected override IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            if (queryOptions == null)
                return GetAllQuery(fetchStrategy);

            var query = BaseQuery(fetchStrategy);

            return queryOptions.Apply(query);
        }

        protected override IQueryable<T> FindAllQuery(ISpecification<T> criteria)
        {
            var query = BaseQuery(criteria.FetchStrategy);
            return criteria.SatisfyingEntitiesFrom(query);
        }

        protected override IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindAllQuery(criteria);

            var query = BaseQuery(criteria.FetchStrategy);

            query = criteria.SatisfyingEntitiesFrom(query);

            return queryOptions.Apply(query);
        }

        public override IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        {
            var innerQuery = innerRepository.AsQueryable();
            var outerQuery = BaseQuery();

            var innerType = innerRepository.GetType();
            var outerType = GetType();

            // if these are 2 different Repository types then let's bring down each query into memory so that they can be joined
            // if they are the same type then they will use the native IQueryable and take advantage of the back-end side join if possible
            if (innerType.Name != outerType.Name)
            {
                innerQuery = innerQuery.ToList().AsQueryable();
                outerQuery = outerQuery.ToList().AsQueryable();
                return new CompositeRepository<TResult>(outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector));
            }

            return new CompositeRepository<TResult>(outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector));
        }
    }

    // TODO: everything is the same as LinqRepositoryBase except the caching strategy and the GetQuery
    public abstract class LinqCompoundKeyRepositoryBase<T, TKey, TKey2> : CompoundKeyRepositoryBase<T, TKey, TKey2> where T : class
    {
        protected LinqCompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(cachingStrategy)
        {   
        }

        public override IQueryable<T> AsQueryable()
        {
            return BaseQuery();
        }

        protected abstract IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null);

        protected override T GetQuery(TKey key, TKey2 key2)
        {
            return FindQuery(ByPrimaryKeySpecification(key, key2));
        }

        protected override T FindQuery(ISpecification<T> criteria)
        {
            return criteria.SatisfyingEntityFrom(BaseQuery());
        }

        protected override T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindQuery(criteria);

            var query = queryOptions.Apply(BaseQuery());

            return criteria.SatisfyingEntityFrom(query);
        }

        protected override IQueryable<T> GetAllQuery(IFetchStrategy<T> fetchStrategy)
        {
            return BaseQuery(fetchStrategy);
        }

        protected override IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            if (queryOptions == null)
                return GetAllQuery(fetchStrategy);

            var query = BaseQuery(fetchStrategy);

            return queryOptions.Apply(query);
        }

        protected override IQueryable<T> FindAllQuery(ISpecification<T> criteria)
        {
            var query = BaseQuery(criteria.FetchStrategy);
            return criteria.SatisfyingEntitiesFrom(query);
        }

        protected override IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindAllQuery(criteria);

            var query = BaseQuery(criteria.FetchStrategy);
            
            query = criteria.SatisfyingEntitiesFrom(query);

            return queryOptions.Apply(query);
        }
		
        public override IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        {
            var innerQuery = innerRepository.AsQueryable();
            var outerQuery = BaseQuery();

            var innerType = innerRepository.GetType();
            var outerType = GetType();

            // if these are 2 different Repository types then let's bring down each query into memory so that they can be joined
            // if they are the same type then they will use the native IQueryable and take advantage of the back-end side join if possible
            if (innerType.Name != outerType.Name)
            {
                innerQuery = innerQuery.ToList().AsQueryable();
                outerQuery = outerQuery.ToList().AsQueryable();
                return new CompositeRepository<TResult>(outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector));
            }

            return new CompositeRepository<TResult>(outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector));
        }
    }

    // TODO: everything is the same as LinqRepositoryBase except the caching strategy and the GetQuery
    public abstract class LinqCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> : CompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> where T : class
    {
        protected LinqCompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
            : base(cachingStrategy)
        {   
        }

        public override IQueryable<T> AsQueryable()
        {
            return BaseQuery();
        }

        protected abstract IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null);

        protected override T GetQuery(TKey key, TKey2 key2, TKey3 key3)
        {
            return FindQuery(ByPrimaryKeySpecification(key, key2, key3));
        }

        protected override T FindQuery(ISpecification<T> criteria)
        {
            return criteria.SatisfyingEntityFrom(BaseQuery());
        }

        protected override T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindQuery(criteria);

            var query = queryOptions.Apply(BaseQuery());

            return criteria.SatisfyingEntityFrom(query);
        }

        protected override IQueryable<T> GetAllQuery(IFetchStrategy<T> fetchStrategy)
        {
            return BaseQuery(fetchStrategy);
        }

        protected override IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
        {
            if (queryOptions == null)
                return GetAllQuery(fetchStrategy);

            var query = BaseQuery(fetchStrategy);

            return queryOptions.Apply(query);
        }

        protected override IQueryable<T> FindAllQuery(ISpecification<T> criteria)
        {
            var query = BaseQuery(criteria.FetchStrategy);
            return criteria.SatisfyingEntitiesFrom(query);
        }

        protected override IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindAllQuery(criteria);

            var query = BaseQuery(criteria.FetchStrategy);
            
            query = criteria.SatisfyingEntitiesFrom(query);

            return queryOptions.Apply(query);
        }
		
        public override IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        {
            var innerQuery = innerRepository.AsQueryable();
            var outerQuery = BaseQuery();

            var innerType = innerRepository.GetType();
            var outerType = GetType();

            // if these are 2 different Repository types then let's bring down each query into memory so that they can be joined
            // if they are the same type then they will use the native IQueryable and take advantage of the back-end side join if possible
            if (innerType.Name != outerType.Name)
            {
                innerQuery = innerQuery.ToList().AsQueryable();
                outerQuery = outerQuery.ToList().AsQueryable();
                return new CompositeRepository<TResult>(outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector));
            }

            return new CompositeRepository<TResult>(outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector));
        }
    }
}
