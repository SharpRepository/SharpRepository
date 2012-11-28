using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class NoCachingConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {
        public NoCachingConfigCachingStrategyFactory(CachingStrategyElement element)
            : base(element)
        {
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>()
        {
            return new NoCachingStrategy<T, TKey>();
        }
    }
}
