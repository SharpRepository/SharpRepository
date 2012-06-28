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
    public abstract class LinqRepositoryBase<T, TKey> : RepositoryBase<T, TKey> where T : class, new()
    {
        protected LinqRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy)
        {   
        }

        public override IQueryable<T>  AsQueryable()
        {
            return BaseQuery();
        }

        protected abstract IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null);

        protected override T GetQuery(TKey key)
        {
            return FindQuery(ByPrimaryKeySpecification(key));
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

        protected override IEnumerable<T> GetAllQuery()
        {
            return BaseQuery().ToList();
        }

        protected override IEnumerable<T> GetAllQuery(IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return GetAllQuery();

            var query = BaseQuery();

            return queryOptions.Apply(query).ToList();
        }

        protected override IEnumerable<T> FindAllQuery(ISpecification<T> criteria)
        {
            var query = BaseQuery(criteria.FetchStrategy);
            return criteria.SatisfyingEntitiesFrom(query).ToList();
        }

        protected override IEnumerable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return FindAllQuery(criteria);

            var query = BaseQuery(criteria.FetchStrategy);
            
            query = criteria.SatisfyingEntitiesFrom(query);

            return queryOptions.Apply(query).ToList();
        }

        public override IRepositoryQueryable<TResult, TResultKey> Join<TOuterKey, TInner, TResult, TResultKey>(IRepositoryQueryable<TInner, TOuterKey> innerRepository, Expression<Func<T, TOuterKey>> outerKeySelector, Expression<Func<TInner, TOuterKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        {
            var innerQuery = innerRepository.AsQueryable();
            var outerQuery = BaseQuery();

            var resultQuery = outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector);

            return new CompositeRepository<TResult, TResultKey>(resultQuery);
        }

        //public override IRepositoryQueryable<TResult, TResultKey> Join<TOuterKey, TInner, TResult, TResultKey>(IRepositoryQueryable<TInner, TOuterKey> innerRepository, Expression<Func<T, TOuterKey>> outerKeySelector, Expression<Func<TInner, TOuterKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        //{
        //    var innerQuery = innerRepository.AsQueryable();
        //    var outerQuery = BaseQuery();

        //    var resultQuery = outerQuery.Join(innerQuery, outerKeySelector, innerKeySelector, resultSelector);

        //    return new CompositeRepository<TResult, TResultKey>(resultQuery);
        //}
    }
}
