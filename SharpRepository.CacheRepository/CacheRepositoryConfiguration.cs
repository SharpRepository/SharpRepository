using SharpRepository.Repository.Configuration;

namespace SharpRepository.CacheRepository
{
    public class CacheRepositoryConfiguration : RepositoryConfiguration
    {
        public CacheRepositoryConfiguration(string name, string cachingStrategy = null, string cachingProvider = null)
            : this(name, "", cachingStrategy, cachingProvider)
        {  
        }

        public CacheRepositoryConfiguration(string name, string prefix, string cachingStrategy=null, string cachingProvider=null)
            : base(name)
        {
            Prefix = prefix;
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(CacheConfigRepositoryFactory);
        }

        public string Prefix
        {
            set { Attributes["prefix"] = value; }
        }
    }
}
