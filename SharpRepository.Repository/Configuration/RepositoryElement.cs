using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace SharpRepository.Repository.Configuration
{
    public class RepositoryElement : ConfigurationElement, IRepositoryConfiguration
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the repository factory.
        /// </summary>
        [ConfigurationProperty("factory", IsRequired = true), TypeConverter(typeof(TypeNameConverter))]
        public Type Factory
        {
            get { return (Type)base["factory"]; }
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof(IConfigRepositoryFactory));

                base["factory"] = value;
            }
        }

        [ConfigurationProperty("cachingStrategy", IsRequired = false)]
        public string CachingStrategy
        {
            get { return (string)base["cachingStrategy"]; }
            set { base["cachingStrategy"] = value; }
        }

        [ConfigurationProperty("cachingProvider", IsRequired = false)]
        public string CachingProvider
        {
            get { return (string)base["cachingProvider"]; }
            set { base["cachingProvider"] = value; }
        }

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

        public new string this[string key]
        {
            get
            {
                return !_attributes.ContainsKey(key) ? null : _attributes[key];
            }
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            var property = new ConfigurationProperty(name, typeof(string), value);
            base[property] = value;

            _attributes[name] = value;

            return true;
        }

        #region IRepositoryConfiguration

        string IRepositoryConfiguration.Name
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        Type IRepositoryConfiguration.Factory
        {
            get { return this.Factory; }
            set { this.Factory = value; }
        }

        string IRepositoryConfiguration.CachingStrategy
        {
            get { return this.CachingStrategy; }
            set { this.CachingStrategy = value; }
        }

        string IRepositoryConfiguration.CachingProvider
        {
            get { return this.CachingProvider; }
            set { this.CachingProvider = value; }
        }

        IDictionary<string, string> IRepositoryConfiguration.Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        IRepository<T, TKey> IRepositoryConfiguration.GetInstance<T, TKey>()
        {
            return this.GetInstance<T, TKey>();
        }

    #endregion

    }
}
