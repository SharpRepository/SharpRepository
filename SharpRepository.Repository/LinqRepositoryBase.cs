using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository
{
    public abstract class LinqRepositoryBase<T, TKey> : RepositoryBase<T, TKey> where T : class
    {
        protected LinqRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy)
        {
            
        }

        public override IQueryable<T> AsQueryable()
        {
            return BaseQuery();
        }

        protected override T GetQuery(TKey key)
        {
            return FindQuery(ByPrimaryKeySpecification(key));
        }

        protected override T FindQuery(ISpecification<T> criteria)
        {
            var query = BaseQuery(criteria.FetchStrategy);

            SetTraceInfo("Find", query);

            return criteria.SatisfyingEntityFrom(query);
        }

        protected override T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindQuery(criteria);

            var query = queryOptions.Apply(BaseQuery(criteria.FetchStrategy));

            SetTraceInfo("Find", query);

            return criteria.SatisfyingEntityFrom(query);
        }

        protected override IQueryable<T> GetAllQuery()
        {
            var query = BaseQuery();

            SetTraceInfo("GetAll", query);

            return query;
        }

        protected override IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return GetAllQuery();

            var query = BaseQuery();

            query = queryOptions.Apply(query);

            SetTraceInfo("GetAll", query);

            return query;
        }

        protected override IQueryable<T> FindAllQuery(ISpecification<T> criteria)
        {
            var query = BaseQuery(criteria.FetchStrategy);
            query = criteria.SatisfyingEntitiesFrom(query);

            SetTraceInfo("FindAll", query);

            return query;
        }

        protected override IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindAllQuery(criteria);

            var query = BaseQuery(criteria.FetchStrategy);
            
            query = criteria.SatisfyingEntitiesFrom(query);

            query = queryOptions.Apply(query);

            SetTraceInfo("FindAll", query);

            return query;
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
            }
            var query = outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector);

            SetTraceInfo("Join", query);

            return new CompositeRepository<TResult>(query);
        }
    }
}
