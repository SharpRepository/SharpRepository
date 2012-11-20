using System;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    public interface ISharpRepositoryConfiguration
    {
        // TODO: this should be changed to something that pople can build up themselves, then have a translation in the SectionGroup implementation to go from RepositoriesSectioNGroup to that property
        RepositoriesSectionGroup Repositories { get; }

        IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
    }

    public class SharpRepositoryConfiguration : ISharpRepositoryConfiguration
    {
        public RepositoriesSectionGroup Repositories { get; set; }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            return ConfigurationHelper.GetDefaultInstance<T, TKey>(Repositories);
        }
    }
}
