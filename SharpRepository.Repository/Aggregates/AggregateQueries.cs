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

            public IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria = null)
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

            public IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector, Expression<Func<T, bool>> predicate)
            {
                return GroupCounts(keySelector, predicate == null ? null : new Specification<T>(predicate));
            }

            public IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria = null)
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

            public IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Func<T, TGroupKey> keySelector, Expression<Func<T, bool>> predicate)
            {
                return GroupLongCounts(keySelector, predicate == null ? null : new Specification<T>(predicate));
            }

            public IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(
                Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector, ISpecification<T> criteria)
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

            public IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector, Expression<Func<T, bool>> predicate)
            {
                return GroupItems(keySelector, resultSelector, predicate == null ? null : new Specification<T>(predicate));
            }

            public long LongCount(ISpecification<T> criteria = null)
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

            public int Count(ISpecification<T> criteria = null)
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

            public int Sum(Expression<Func<T, int>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public int Sum(Expression<Func<T, int>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public int? Sum(Expression<Func<T, int?>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public int? Sum(Expression<Func<T, int?>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public long Sum(Expression<Func<T, long>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public long Sum(Expression<Func<T, long>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public long? Sum(Expression<Func<T, long?>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public long? Sum(Expression<Func<T, long?>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public decimal Sum(Expression<Func<T, decimal>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public decimal Sum(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public decimal? Sum(Expression<Func<T, decimal?>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public decimal? Sum(Expression<Func<T, decimal?>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public double Sum(Expression<Func<T, double>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public double Sum(Expression<Func<T, double>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public double? Sum(Expression<Func<T, double?>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public double? Sum(Expression<Func<T, double?>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public float Sum(Expression<Func<T, float>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public float Sum(Expression<Func<T, float>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public float? Sum(Expression<Func<T, float?>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteSum(
                    () => criteria == null ? _repository.AsQueryable().Sum(selector) : _repository.AsQueryable().Where(criteria.Predicate).Sum(selector),
                    selector,
                    criteria
                    );
            }

            public float? Sum(Expression<Func<T, float?>> selector, Expression<Func<T, bool>> predicate)
            {
                return Sum(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public TResult Min<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteMin(
                    () => criteria == null ? _repository.AsQueryable().Min(selector) : _repository.AsQueryable().Where(criteria.Predicate).Min(selector),
                    selector,
                    criteria
                    );
            }

            public TResult Min<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate)
            {
                return Min(selector, predicate == null ? null : new Specification<T>(predicate));
            }

            public TResult Max<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria = null)
            {
                return _queryManager.ExecuteMax(
                    () => criteria == null ? _repository.AsQueryable().Max(selector) : _repository.AsQueryable().Where(criteria.Predicate).Max(selector),
                    selector,
                    criteria
                    );
            }

            public TResult Max<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> predicate)
            {
                return Max(selector, predicate == null ? null : new Specification<T>(predicate));
            }
        }
}
