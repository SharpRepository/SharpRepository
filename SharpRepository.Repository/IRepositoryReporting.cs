using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository
{
    public interface IRepositoryReporting<T> where T : class
    {
        IEnumerable<GroupCount<TGroupKey>> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector);

        IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector);

        long LongCount(ISpecification<T> criteria = null);
        long LongCount(Expression<Func<T, bool>> predicate);
        int Count(ISpecification<T> criteria = null);
        int Count(Expression<Func<T, bool>> predicate);
    }
    
    public class GroupCount<TGroupKey>
    {
        public TGroupKey Key { get; set; }
        public int Count { get; set; }
    }
    
    public class GroupItem<TGroupKey, TGroupResult>
    {
        public TGroupKey Key { get; set; }
        public IEnumerable<TGroupResult> Items { get; set; }
    }
}
