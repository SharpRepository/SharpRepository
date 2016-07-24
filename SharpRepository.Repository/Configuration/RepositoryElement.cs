using System;
using System.Collections.Generic;
using System.ComponentModel;
#if NET451
using System.Configuration;
#elif NETSTANDARD1_6
using Microsoft.Extensions.Configuration;
#endif

namespace SharpRepository.Repository.Configuration
{
#if NET451
    public class RepositoryElement : ConfigurationElement, IRepositoryConfiguration
#elif NETSTANDARD1_6
    public class RepositoryElement : IRepositoryConfiguration
#endif
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();

#if NET451
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
#endif
        public string Name
        {
#if NET451
            get { return (string)base["name"]; }
            set { base["name"] = value; }
#elif NETSTANDARD1_6
            get;
            set;
#endif
        }

        /// <summary>
        /// Gets or sets the type of the repository factory.
        /// </summary>
#if NET451
        [ConfigurationProperty("factory", IsRequired = true), TypeConverter(typeof(TypeNameConverter))]
#elif NETSTANDARD1_6
        private Type _factory;
#endif
        public Type Factory
        {
#if NET451
            get { return (Type)base["factory"]; }
#elif NETSTANDARD1_6
            get { return _factory; }
#endif
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof(IConfigRepositoryFactory));
#if NET451
                base["factory"] = value;
#elif NETSTANDARD1_6
                _factory = value;
#endif
            }
        }

#if NET451
        [ConfigurationProperty("cachingStrategy", IsRequired = false)]
#endif
        public string CachingStrategy
        {
#if NET451
            get { return (string)base["cachingStrategy"]; }
            set { base["cachingStrategy"] = value; }
#elif NETSTANDARD1_6
            get;
            set;
#endif
        }

#if NET451
        [ConfigurationProperty("cachingProvider", IsRequired = false)]
#endif
        public string CachingProvider
        {
#if NET451
            get { return (string)base["cachingProvider"]; }
            set { base["cachingProvider"] = value; }
#elif NETSTANDARD1_6
            get;
            set;
#endif
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


#if NET451
        public new string this[string key]
#elif NETSTANDARD1_6
        public string this[string key]
#endif
        {
            get
            {
                return !_attributes.ContainsKey(key) ? null : _attributes[key];
            }
#if NETSTANDARD1_6
            private set
            {
                _attributes[key] = value;
            }
#endif
        }

#if NET451
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            var property = new ConfigurationProperty(name, typeof(string), value);
            base[property] = value;

            _attributes[name] = value;

            return true;
        }
#endif

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
