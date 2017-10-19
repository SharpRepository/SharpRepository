using System;
using System.Collections.Generic;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{    public class CachingStrategyElement : ICachingStrategyConfiguration
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();
        
        public string Name { get; set; }
        
        public int? MaxResults { get; set; }

        /// <summary>
        /// Gets or sets the type of the caching strategy factory.
        /// </summary>
        private Type _factory;
        public Type Factory
        {
            get { return _factory; }
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof(IConfigCachingStrategyFactory));
                _factory = value;
            }
        }

        public ICachingStrategy<T, TKey> GetInstance<T, TKey>(ICachingProvider cachingProvider) where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey>(cachingProvider);
        }

        public ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(ICachingProvider cachingProvider) where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2>(cachingProvider);
        }

        public ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(ICachingProvider cachingProvider) where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2, TKey3>(cachingProvider);
        }

        public ICompoundKeyCachingStrategy<T> GetCompoundKeyInstance<T>(ICachingProvider cachingProvider) where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetCompoundKeyInstance<T>(cachingProvider);
        }

        ICachingStrategy<T, TKey> ICachingStrategyConfiguration.GetInstance<T, TKey>(ICachingProvider cachingProvider)
        {
            return GetInstance<T, TKey>(cachingProvider);
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
        
        string ICachingStrategyConfiguration.Name
        {
            get => Name; set => Name = value;
        }

        int? ICachingStrategyConfiguration.MaxResults
        {
            get => MaxResults; set => MaxResults = value;
        }

        Type ICachingStrategyConfiguration.Factory
        {
            get => Factory; set { Factory = value; }
        }

        IDictionary<string, string> ICachingStrategyConfiguration.Attributes
        {
            get => _attributes; set => _attributes = value;
        }
    }
}