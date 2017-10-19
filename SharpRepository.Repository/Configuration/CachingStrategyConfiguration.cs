using System;
using System.Collections.Generic;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public class CachingStrategyConfiguration : ICachingStrategyConfiguration
    {
        public CachingStrategyConfiguration()
        {
            Attributes = new Dictionary<string, string>();
        }

        public string Name { get; set; }

        public int? MaxResults { get; set; }

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

        public IDictionary<string, string> Attributes { get; set; }

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
        
        public string this[string key]
        {
            get { return Attributes[key]; }
        }
    }
}
