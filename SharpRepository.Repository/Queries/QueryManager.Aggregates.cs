using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Queries
{
    public partial class QueryManager<T, TKey>
    {
        public int ExecuteCount(Func<int> query, ISpecification<T> criteria)
        {
            int result;
            if (CacheEnabled && _cachingStrategy.TryCountResult(criteria, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveCountResult(criteria, result);

            return result;
        }

        public long ExecuteLongCount(Func<long> query, ISpecification<T> criteria)
        {
            long result;
            if (CacheEnabled && _cachingStrategy.TryLongCountResult(criteria, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveLongCountResult(criteria, result);

            return result;
        }

        public IDictionary<TGroupKey, int> ExecuteGroupCounts<TGroupKey>(Func<IDictionary<TGroupKey, int>> query, Func<T, TGroupKey> keySelector)
        {
            IDictionary<TGroupKey, int> result;
            if (CacheEnabled && _cachingStrategy.TryGroupCountsResult(keySelector, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGroupCountsResult(keySelector, result);

            return result;
        }

        public IDictionary<TGroupKey, long> ExecuteGroupLongCounts<TGroupKey>(Func<IDictionary<TGroupKey, long>> query, Func<T, TGroupKey> keySelector)
        {
            IDictionary<TGroupKey, long> result;
            if (CacheEnabled && _cachingStrategy.TryGroupLongCountsResult(keySelector, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGroupLongCountsResult(keySelector, result);

            return result;
        }

        public IEnumerable<GroupItem<TGroupKey, TGroupResult>> ExecuteGroupItems<TGroupKey, TGroupResult>(Func<IEnumerable<GroupItem<TGroupKey, TGroupResult>>> query, Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector)
        {
            IEnumerable<GroupItem<TGroupKey, TGroupResult>> result;
            if (CacheEnabled && _cachingStrategy.TryGroupItemsResult(keySelector, resultSelector, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGroupItemsResult(keySelector, resultSelector, result);

            return result;
        }

        public TResult ExecuteSum<TResult>(Func<TResult> query, Expression<Func<T, TResult>> selector, ISpecification<T> criteria)
        {
            TResult result;
            if (CacheEnabled && _cachingStrategy.TrySumResult(selector, criteria, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveSumResult(selector, criteria, result);

            return result;
        }

        public TResult ExecuteMin<TResult>(Func<TResult> query, Expression<Func<T, TResult>> selector, ISpecification<T> criteria)
        {
            TResult result;
            if (CacheEnabled && _cachingStrategy.TryMinResult(selector, criteria, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveMinResult(selector, criteria, result);

            return result;
        }

        public TResult ExecuteMax<TResult>(Func<TResult> query, Expression<Func<T, TResult>> selector, ISpecification<T> criteria)
        {
            TResult result;
            if (CacheEnabled && _cachingStrategy.TryMaxResult(selector, criteria, out result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveMaxResult(selector, criteria, result);

            return result;
        }
    }
}
