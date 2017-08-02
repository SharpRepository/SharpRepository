using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace SharpRepository.Repository.Configuration
{
    public class RepositoryElement : IRepositoryConfiguration
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();
        
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the repository factory.
        /// </summary>
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

        public IRepository<T> GetInstance<T>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory) Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T>();
        }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory) Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey>();
        }

        public ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory) Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2>();
        }

        public ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2, TKey3>();
        }
        public ICompoundKeyRepository<T> GetCompoundKeyInstance<T>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetCompoundKeyInstance<T>();
        }
        public string this[string key]
        {
            get
            {
                return !_attributes.ContainsKey(key) ? null : _attributes[key];
            }
            private set
            {
                _attributes[key] = value;
            }
        }

#region IRepositoryConfiguration

        string IRepositoryConfiguration.Name
        {
            get => Name; set => Name = value;
        }

        Type IRepositoryConfiguration.Factory
        {
            get => Factory; set => Factory = value;
        }

        string IRepositoryConfiguration.CachingStrategy
        {
            get => CachingStrategy; set => CachingStrategy = value;
        }

        string IRepositoryConfiguration.CachingProvider
        {
            get => CachingProvider; set => CachingProvider = value;
        }

        IDictionary<string, string> IRepositoryConfiguration.Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        IRepository<T, TKey> IRepositoryConfiguration.GetInstance<T, TKey>()
        {
            return GetInstance<T, TKey>();
        }

#endregion

    }
}
