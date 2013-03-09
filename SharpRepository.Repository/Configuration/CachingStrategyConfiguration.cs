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

        public string this[string key]
        {
            get { return Attributes[key]; }
        }
    }
}
