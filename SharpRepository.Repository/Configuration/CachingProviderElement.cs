using System;
using System.Collections.Generic;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public class CachingProviderElement : ICachingProviderConfiguration
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();
        
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the caching strategy factory.
        /// </summary>
        private Type _factory;
        public Type Factory
        {
            get { return _factory; }
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof (IConfigCachingProviderFactory));
                _factory = value;
            }
        }

        public ICachingProvider GetInstance()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingProviderFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance();
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
        
        string ICachingProviderConfiguration.Name
        {
            get => Name; set => Name = value;
        }

        Type ICachingProviderConfiguration.Factory
        {
            get => Factory; set => Factory = value;
        }

        IDictionary<string, string> ICachingProviderConfiguration.Attributes
        {
            get => _attributes; set => _attributes = value;
        }

        ICachingProvider ICachingProviderConfiguration.GetInstance()
        {
            return GetInstance();
        }
    }
}
