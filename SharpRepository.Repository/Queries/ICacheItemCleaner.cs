using System.Collections.Generic;

namespace SharpRepository.Repository.Queries
{
    public interface ICacheItemCleaner
    {
        TCacheItem CleanItem<TCacheItem>(TCacheItem item);
        IEnumerable<TCacheItem> CleanItems<TCacheItem>(IEnumerable<TCacheItem> items);
    }
}
