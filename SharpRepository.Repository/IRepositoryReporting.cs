using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpRepository.Repository
{
    public interface IRepositoryReporting<T> where T : class
    {
        IEnumerable<GroupCount<TGroupKey>> GroupCounts<TGroupKey>(Func<T, TGroupKey> keySelector);

        IEnumerable<GroupItem<TGroupKey, TGroupResult>> GroupItems<TGroupKey, TGroupResult>(Func<T, TGroupKey> keySelector, Func<T, TGroupResult> resultSelector);

        int Count(Expression<Func<T, bool>> predicate = null);
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
