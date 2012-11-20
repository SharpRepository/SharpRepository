using System;
using System.Configuration;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.MongoDbRepository
{
    public class Config : ConfigurationSection, IRepositoryElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("url")]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            return !String.IsNullOrEmpty(Url) ? new MongoDbRepository<T, TKey>(Url) : new MongoDbRepository<T, TKey>();
        }
    }
}
