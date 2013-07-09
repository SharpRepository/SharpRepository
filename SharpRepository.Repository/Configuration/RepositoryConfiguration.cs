using System;
using System.Collections.Generic;

namespace SharpRepository.Repository.Configuration
{
    public class RepositoryConfiguration : IRepositoryConfiguration
    {
        public RepositoryConfiguration()
        {
            Attributes = new Dictionary<string, string>();
        }

        public RepositoryConfiguration(string name)
        {
            Name = name;
            Attributes = new Dictionary<string, string>();
        }

        public string Name { get; set; }

        private Type _factory;
        public Type Factory
        {
            get { return _factory; }
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof(IConfigRepositoryFactory));

                _factory = value;
            }
        }
        public string CachingStrategy { get; set; }
        public string CachingProvider { get; set; }

        public IDictionary<string, string> Attributes { get; set; }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey>();
        }

        public ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2>();
        }

        public string this[string key]
        {
            get { return Attributes.ContainsKey(key) ? Attributes[key] : null; }
        }
    }
}
