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
            private readonly IRepository<T, TKey> _repository;
            private readonly QueryManager<T, TKey> _queryManager;

            public AggregateQueries(IRepository<T, TKey> repository, QueryManager<T, TKey> queryManager)
            {
                _repository = repository;
                _queryManager = queryManager;
            }

            public IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector)
            {
                return GroupCounts((ISpecification<T>) null, keySelector);
            }

            public IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(ISpecification<T> criteria, Func<T, TGroupKey> keySelector)
            {
                return _queryManager.ExecuteGroupCounts(
                    () => criteria == null ? 
                        _repository.AsQueryable().GroupBy(keySelector).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Count())
                        :
                        _repository.AsQueryable().Where(criteria.Predicate).GroupBy(keySelector).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Count()),
                    keySelector,
                    criteria
                    );
            }

            public IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Expression<Func<T, bool>> predicate, Func<T, TGroupKey> keySelector)
            {
                return GroupCounts(predicate == null ? null : new Specification<T>(predicate), keySelector);
            }

            public IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Func<T, TGroupKey> keySelector)
            {
                return GroupLongCounts((ISpecification<T>)null, keySelector);
            }

            public IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(ISpecification<T> criteria, Func<T, TGroupKey> keySelector)
            {
                return _queryManager.ExecuteGroupLongCounts(
                    () => criteria == null ?
                        _repository.AsQueryable().GroupBy(keySelector).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.LongCount())
                        :
                        _repository.AsQueryable().Where(criteria.Predicate).GroupBy(keySelector).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.LongCount()),
                    keySelector,
                    criteria
                    );
            }

            public IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Expression<Func<T, bool>> predicate, Func<T, TGroupKey> keySelector)
            {
                return GroupLongCounts(predicate == null ? null : new Specification<T>(predicate), keySelector);
            }

            public IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(
                Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector)
            {
                return GroupItems((ISpecification<T>) null, keySelector, resultSelector);
            }

            public IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(
                ISpecification<T> criteria, Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector)
            {
                return _queryManager.ExecuteGroupItems(
                    () => criteria == null ?
                            _repository.AsQueryable()
                            .GroupBy(keySelector, resultSelector)
                            .Select(g => new GroupItem<TGroupKey, TGroupResult> { Key = g.Key, Items = g.Select(x => x) }).OrderBy(x => x.Key).ToList()
                        :
                            _repository.AsQueryable()
                            .Where(criteria.Predicate)
                            .GroupBy(keySelector, resultSelector)
                            .Select(g => new GroupItem<TGroupKey, TGroupResult> { Key = g.Key, Items = g.Select(x => x) }).OrderBy(x => x.Key).ToList(),
                    keySelector,
                    resultSelector,
                    criteria
                    );
            }

            public IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(
                Expression<Func<T, bool>> predicate, Func<T, TGroupKey> keySelector,
                Func<T, TGroupResult> resultSelector)
            {
                return GroupItems(predicate == null ? null : new Specification<T>(predicate), keySelector, resultSelector);
            }

            public long LongCount()
            {
                return LongCount((ISpecification<T>) null);
            }

            public long LongCount(ISpecification<T> criteria)
            {                
                return _queryManager.ExecuteLongCount(
                    () => criteria == null ? _repository.AsQueryable().LongCount() : _repository.AsQueryable().LongCount(criteria.Predicate),
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
                return _queryManager.ExecuteCount(
                    () => criteria == null ? _repository.AsQueryable().Count() : _repository.AsQueryable().Count(criteria.Predicate),
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

            public int Sum(ISpecification<T> criteria, Expression<Func<T, int>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public int? Sum(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public long Sum(ISpecification<T> criteria, Expression<Func<T, long>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public long? Sum(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public decimal Sum(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public decimal? Sum(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public double Sum(ISpecification<T> criteria, Expression<Func<T, double>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public double? Sum(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public float Sum(ISpecification<T> criteria, Expression<Func<T, float>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
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

            public float? Sum(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public float? Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, float?>> selector)
            {
                return Sum(predicate == null ? null : new Specification<T>(predicate), selector);
            }

            public TResult Min<TResult>(Expression<Func<T, TResult>> selector)
            {
                return Min((ISpecification<T>)null, selector);
            }

            public TResult Min<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
            {
                return _queryManager.ExecuteMin(
                    () => criteria == null ? _repository.AsQueryable().Min(selector) : _repository.AsQueryable().Where(criteria.Predicate).Min(selector),
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

            public TResult Max<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
            {
                return _queryManager.ExecuteMax(
                    () => criteria == null ? _repository.AsQueryable().Max(selector) : _repository.AsQueryable().Where(criteria.Predicate).Max(selector),
                    selector,
                    criteria
                    );
            }

            public TResult Max<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
            {
                return Max(predicate == null ? null : new Specification<T>(predicate), selector);
            }
        }
}
