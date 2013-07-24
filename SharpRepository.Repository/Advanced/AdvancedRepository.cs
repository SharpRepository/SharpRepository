using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Advanced
{
        public class AdvancedRepository<T, TKey> : IAdvancedRepository<T> where T : class
        {
            private readonly IRepository<T, TKey> _repository;
            private readonly QueryManager<T, TKey> _queryManager;

            public AdvancedRepository(IRepository<T, TKey> repository, QueryManager<T, TKey> queryManager)
            {
                _repository = repository;
                _queryManager = queryManager;
            }

            public IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector)
            {
                return _queryManager.ExecuteGroupCounts(
                    () => _repository.AsQueryable().GroupBy(keySelector).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Count()),
                    keySelector
                    );
            }

            public IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Func<T, TGroupKey> keySelector)
            {
                return _queryManager.ExecuteGroupLongCounts(
                    () => _repository.AsQueryable().GroupBy(keySelector).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.LongCount()),
                    keySelector
                    );
            }

            public IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(
                Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector)
            {
                return _queryManager.ExecuteGroupItems(
                    () => _repository.AsQueryable()
                        .GroupBy(keySelector, resultSelector)
                        .Select(g => new GroupItem<TGroupKey, TGroupResult> { Key = g.Key, Items = g.Select(x => x) }).OrderBy(x => x.Key).ToList(),
                    keySelector,
                    resultSelector
                    );
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
        }
}
