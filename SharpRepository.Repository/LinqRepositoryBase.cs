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
    public abstract class LinqRepositoryBase<T, TKey> : RepositoryBase<T, TKey> where T : class
    {
        protected LinqRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy)
        {   
        }

        public override IQueryable<T> AsQueryable()
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

        protected override IQueryable<T> GetAllQuery()
        {
            return BaseQuery();
        }

        protected override IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions)
        {
            if (queryOptions == null)
                return GetAllQuery();

            var query = BaseQuery();

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

        public override IEnumerator<T> GetEnumerator()
        {
            return BaseQuery().GetEnumerator();
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
