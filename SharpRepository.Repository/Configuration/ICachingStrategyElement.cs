using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface ICachingStrategyElement
    {
        string Name { get; set; }
        string CachingProvider { get; set; }

        ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class;
    }
}
