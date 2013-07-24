using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Advanced
{
    public interface IAdvancedRepository<T> where T : class
    {
        IDictionary<TGroupKey, int> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector);
        IDictionary<TGroupKey, long> GroupLongCounts<TGroupKey>(Func<T, TGroupKey> keySelector);

        IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector);

        long LongCount(ISpecification<T> criteria = null);
        long LongCount(Expression<Func<T, bool>> predicate);
        int Count(ISpecification<T> criteria = null);
        int Count(Expression<Func<T, bool>> predicate);
    }
}