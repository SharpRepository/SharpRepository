using System;
using System.Configuration;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    public interface ISharpRepositoryConfiguration
    {
        // TODO: this should be changed to something that pople can build up themselves, then have a translation in the SectionGroup implementation to go from RepositoriesSectioNGroup to that property
        RepositoriesSectionGroup Repositories { get; }
        ConfigurationSectionGroup CachingStrategies { get; }
        ConfigurationSectionGroup CachingProviders { get; }

        IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
    }

    public class SharpRepositoryConfiguration : ISharpRepositoryConfiguration
    {
        public RepositoriesSectionGroup Repositories { get; set; }
        public ConfigurationSectionGroup CachingStrategies { get; private set; }
        public ConfigurationSectionGroup CachingProviders { get; private set; }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            return ConfigurationHelper.GetDefaultInstance<T, TKey>(this);
        }
    }
}
