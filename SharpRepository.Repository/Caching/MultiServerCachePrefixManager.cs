
namespace SharpRepository.Repository.Caching
{
    public class MultiServerCachePrefixManager : ICachePrefixManager
    {
        private readonly ICachingProvider _cachingProvider;
        private readonly string _key;

        public MultiServerCachePrefixManager(ICachingProvider cachingProvider, string key = "#Repos/GlobalCacheCounter")
        {
            _cachingProvider = cachingProvider;
            _key = key;
        }

        public int Counter
        {
            get 
            { 
                int counter;
                if (!_cachingProvider.Get(_key, out counter))
                {
                    counter = 1;
                }

                return counter;
            }
        }
        public void IncrementCounter()
        {
            _cachingProvider.Increment(_key, 1, 1);
        }
    }
}
