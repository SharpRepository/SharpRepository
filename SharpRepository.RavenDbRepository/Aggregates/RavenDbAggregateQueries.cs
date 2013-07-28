using System;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository;
using SharpRepository.Repository.Aggregates;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.RavenDbRepository.Aggregates
{
    public class RavenDbAggregateQueries<T, TKey> : AggregateQueries<T, TKey> where T : class
    {
        public RavenDbAggregateQueries(IRepository<T, TKey> repository, QueryManager<T, TKey> queryManager)
            : base(repository, queryManager)
        {
        }

        public override TResult Min<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            var pagingOptions = new PagingOptions<T, TResult>(1, 1, selector);

            return QueryManager.ExecuteMin(
                () => Repository.FindAll(criteria, selector, pagingOptions).ToList().First(),
                selector,
                criteria
                );
        }

        public override TResult Max<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            var pagingOptions = new PagingOptions<T, TResult>(1, 1, selector, isDescending: true);

            return QueryManager.ExecuteMin(
                () => Repository.FindAll(criteria, selector, pagingOptions).ToList().First(),
                selector,
                criteria
                );
        }

        public override int Sum(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override decimal? Sum(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override decimal Sum(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override double? Sum(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override double Sum(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override float? Sum(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override float Sum(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override int? Sum(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override long? Sum(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override long Sum(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => Repository.FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override double Average(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override decimal? Average(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override decimal Average(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double? Average(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double Average(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override float? Average(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override float Average(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double? Average(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double? Average(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double Average(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => Repository.FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }
    }
}
