using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Queries
{
    public partial class QueryManager<T, TKey>
    {
        public int ExecuteCount(Func<int> query, ISpecification<T> criteria)
        {
            if (CacheEnabled && _cachingStrategy.TryCountResult(criteria, out int result))
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
            if (CacheEnabled && _cachingStrategy.TryLongCountResult(criteria, out long result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveLongCountResult(criteria, result);

            return result;
        }

        public IEnumerable<TResult> ExecuteGroup<TGroupKey, TResult>(Func<IEnumerable<TResult>> query, Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector, ISpecification<T> criteria)
        {
            if (CacheEnabled && _cachingStrategy.TryGroupResult(keySelector, resultSelector, criteria, out IEnumerable<TResult> result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveGroupResult(keySelector, resultSelector, criteria, result);

            return result;
        }

        public TResult ExecuteSum<TResult>(Func<TResult> query, Expression<Func<T, TResult>> selector, ISpecification<T> criteria)
        {
            if (CacheEnabled && _cachingStrategy.TrySumResult(selector, criteria, out TResult result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveSumResult(selector, criteria, result);

            return result;
        }

        public TResult ExecuteAverage<TSelector, TResult>(Func<TResult> query, Expression<Func<T, TSelector>> selector, ISpecification<T> criteria)
        {
            if (CacheEnabled && _cachingStrategy.TryAverageResult(selector, criteria, out TResult result))
            {
                CacheUsed = true;
                return result;
            }

            CacheUsed = false;
            result = query.Invoke();

            _cachingStrategy.SaveAverageResult(selector, criteria, result);

            return result;
        }

        public TResult ExecuteMin<TResult>(Func<TResult> query, Expression<Func<T, TResult>> selector, ISpecification<T> criteria)
        {
            if (CacheEnabled && _cachingStrategy.TryMinResult(selector, criteria, out TResult result))
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
            if (CacheEnabled && _cachingStrategy.TryMaxResult(selector, criteria, out TResult result))
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
