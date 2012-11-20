using System.Configuration;
using System.Data.Entity;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ef5Repository.Configuration
{
    public class Ef5RepositorySection : ConfigurationSection, IRepositoryElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)base["connectionString"]; }
            set { base["connectionString"] = value; }
        }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            return new Ef5Repository<T, TKey>(new DbContext(ConnectionString));
        }
    }
}
