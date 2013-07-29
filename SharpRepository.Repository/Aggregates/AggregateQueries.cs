using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Aggregates
{
        public class AggregateQueries<T, TKey> : IAggregateQueries<T> where T : class
        {
            protected readonly IRepository<T, TKey> Repository;
            protected readonly QueryManager<T, TKey> QueryManager;

            public AggregateQueries(IRepository<T, TKey> repository, QueryManager<T, TKey> queryManager)
            {
                Repository = repository;
                QueryManager = queryManager;
            }

            // TODO: allowing ordering of grouped results
            public IEnumerable<TResult> Group<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
            {
                return Group((ISpecification<T>)null, keySelector, resultSelector);
            }

            public IEnumerable<TResult> Group<TGroupKey, TResult>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
            {
                return QueryManager.ExecuteGroup(
                    () =>
                        {
                            var query = criteria == null ? Repository.AsQueryable() : Repository.AsQueryable().Where(criteria.Predicate);

//                            if (queryOptions != null)
//                                query = queryOptions.Apply(query);

                            return query.GroupBy(keySelector).OrderBy(x => x.Key).Select(resultSelector).ToList();
                        },
                    keySelector,
                    resultSelector,
                    criteria
                    );
            }

            public IEnumerable<TResult> Group<TGroupKey, TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector)
            {
                return Group(predicate == null ? null : new Specification<T>(predicate), keySelector, resultSelector);
            }

            public long LongCount()
            {
                return LongCount((ISpecification<T>) null);
            }

            public long LongCount(ISpecification<T> criteria)
            {                
                return QueryManager.ExecuteLongCount(
                    () => criteria == null ? Repository.AsQueryable().LongCount() : Repository.AsQueryable().LongCount(criteria.Predicate),
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

            public int Count(ISpecification<T> criteria)
            {
                return QueryManager.ExecuteCount(
                    () => criteria == null ? Repository.AsQueryable().Count() : Repository.AsQueryable().Count(criteria.Predicate),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Sum(selector) : Repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Average(selector) : Repository.AsQueryable().Where(criteria.Predicate).Average(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Min(selector) : Repository.AsQueryable().Where(criteria.Predicate).Min(selector),
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
                    () => criteria == null ? Repository.AsQueryable().Max(selector) : Repository.AsQueryable().Where(criteria.Predicate).Max(selector),
                    selector,
                    criteria
                    );
            }

            public TResult Max<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            {
                return Max(predicate == null ? null : new Specification<T>(predicate), selector);
            }

            public IDictionary<TGroupKey, int> GroupCount<TGroupKey>(Expression<Func<T, TGroupKey>> groupSelector)
            {
                return GroupCount((ISpecification<T>)null, groupSelector);
            }

            public IDictionary<TGroupKey, int> GroupCount<TGroupKey>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> groupSelector)
            {
                return Group(criteria, groupSelector, x => new { x.Key, Count = x.Count() }).ToDictionary(x => x.Key, x => x.Count);
            }

            public IDictionary<TGroupKey, int> GroupCount<TGroupKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> groupSelector)
            {
                return GroupCount(predicate == null ? null : new Specification<T>(predicate), groupSelector);
            }

            public IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(Expression<Func<T, TGroupKey>> groupSelector)
            {
                return GroupLongCount((ISpecification<T>)null, groupSelector);
            }

            public IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(ISpecification<T> criteria, Expression<Func<T, TGroupKey>> groupSelector)
            {
                return Group(criteria, groupSelector, x => new { x.Key, Count = x.LongCount() }).ToDictionary(x => x.Key, x => x.Count);
            }

            public IDictionary<TGroupKey, long> GroupLongCount<TGroupKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TGroupKey>> groupSelector)
            {
                return GroupLongCount(predicate == null ? null : new Specification<T>(predicate), groupSelector);
            }

            public IDictionary<TGroupKey, TResult> GroupMin<TGroupKey, TResult>(
                Expression<Func<T, TGroupKey>> groupSelector, Func<T, TResult> selector)
            {
                return GroupMin((ISpecification<T>)null, groupSelector, selector);
            }

            public IDictionary<TGroupKey, TResult> GroupMin<TGroupKey, TResult>(ISpecification<T> criteria,
                                                                                Expression<Func<T, TGroupKey>> groupSelector,
                                                                                Func<T, TResult> selector)
            {
                return Group(criteria, groupSelector, x => new { x.Key, Min = x.Min(selector) }).ToDictionary(x => x.Key, x => x.Min);
            }

            public IDictionary<TGroupKey, TResult> GroupMin<TGroupKey, TResult>(Expression<Func<T, bool>> predicate,
                                                                                Expression<Func<T, TGroupKey>>groupSelector,
                                                                                Func<T, TResult> selector)
            {
                return GroupMin(predicate == null ? null : new Specification<T>(predicate), groupSelector, selector);
            }

            public IDictionary<TGroupKey, TResult> GroupMax<TGroupKey, TResult>(
                Expression<Func<T, TGroupKey>> groupSelector, Func<T, TResult> selector)
            {
                return GroupMax((ISpecification<T>)null, groupSelector, selector);
            }

            public IDictionary<TGroupKey, TResult> GroupMax<TGroupKey, TResult>(ISpecification<T> criteria,
                                                                                Expression<Func<T, TGroupKey>> groupSelector,
                                                                                Func<T, TResult> selector)
            {
                return Group(criteria, groupSelector, x => new { x.Key, Max = x.Max(selector) }).ToDictionary(x => x.Key, x => x.Max);
            }

            public IDictionary<TGroupKey, TResult> GroupMax<TGroupKey, TResult>(Expression<Func<T, bool>> predicate,
                                                                                Expression<Func<T, TGroupKey>>groupSelector,
                                                                                Func<T, TResult> selector)
            {
                return GroupMax(predicate == null ? null : new Specification<T>(predicate), groupSelector, selector);
            }
        }
}
