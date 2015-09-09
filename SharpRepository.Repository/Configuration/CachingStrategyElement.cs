using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public class CachingStrategyElement : ConfigurationElement, ICachingStrategyConfiguration
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("maxResults", IsRequired = false, IsKey = false)]
        public int? MaxResults
        {
            get { return base["maxResults"] as int?; }
            set { base["maxResults"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the caching strategy factory.
        /// </summary>
        [ConfigurationProperty("factory", IsRequired = true), TypeConverter(typeof(TypeNameConverter))]
        public Type Factory
        {
            get { return (Type)base["factory"]; }
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof(IConfigCachingStrategyFactory));

                base["factory"] = value;
            }
        }

        public ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey>();
        }

        public ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2>();
        }
        public ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2, TKey3>();
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

        string ICachingStrategyConfiguration.Name
        {
            get { return this.Name;  }
            set { this.Name = value; }
        }

        int? ICachingStrategyConfiguration.MaxResults
        {
            get { return this.MaxResults; }
            set { this.MaxResults = value; }
        }

        Type ICachingStrategyConfiguration.Factory
        {
            get { return this.Factory; }
            set { this.Factory = value; }
        }

        IDictionary<string, string> ICachingStrategyConfiguration.Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }


        ICachingStrategy<T, TKey> ICachingStrategyConfiguration.GetInstance<T, TKey>()
        {
            return this.GetInstance<T, TKey>();
        }
    }
}
