using System.Collections.Generic;

namespace SharpRepository.Repository
{
    public class GroupItem<TGroupKey, TGroupResult>
    {
        public TGroupKey Key { get; set; }
        public IEnumerable<TGroupResult> Items { get; set; }
    }
}
