using System.Configuration;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.InMemoryRepository
{
    public class Config : ConfigurationSection, IRepositoryElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            return new InMemoryRepository<T, TKey>();
        }
    }
}
