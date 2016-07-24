using System;
using System.Collections.Generic;
#if NET451
using System.ComponentModel;
using System.Configuration;
#endif
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
#if NET451
    public class CachingProviderElement : ConfigurationElement, ICachingProviderConfiguration
#elif NETSTANDARD1_6
    public class CachingProviderElement : ICachingProviderConfiguration
#endif
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();

#if NET451
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
#endif
        public string Name
        {
#if NET451
            get { return (string) base["name"]; }
            set { base["name"] = value; }
#elif NETSTANDARD1_6
            get;
            set;
#endif
        }

        /// <summary>
        /// Gets or sets the type of the caching strategy factory.
        /// </summary>
#if NET451
        [ConfigurationProperty("factory", IsRequired = true), TypeConverter(typeof (TypeNameConverter))]
#elif NETSTANDARD1_6
        private Type _factory;
#endif
        public Type Factory
        {
#if NET451
            get { return (Type) base["factory"]; }
#elif NETSTANDARD1_6
            get { return _factory; }
#endif
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof (IConfigCachingProviderFactory));
#if NET451
                base["factory"] = value;
#elif NETSTANDARD1_6
                _factory = value;
#endif
            }
        }

        public ICachingProvider GetInstance()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingProviderFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance();
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

        string ICachingProviderConfiguration.Name
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        Type ICachingProviderConfiguration.Factory
        {
            get { return this.Factory; }
            set { this.Factory = value; }
        }

        IDictionary<string, string> ICachingProviderConfiguration.Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }


        ICachingProvider ICachingProviderConfiguration.GetInstance()
        {
            return this.GetInstance();
        }
    }
}
