using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public class CachingProviderElement : ConfigurationElement, ICachingProviderConfiguration
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string) base["name"]; }
            set { base["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the caching strategy factory.
        /// </summary>
        [ConfigurationProperty("factory", IsRequired = true), TypeConverter(typeof (TypeNameConverter))]
        public Type Factory
        {
            get { return (Type) base["factory"]; }
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof (IConfigCachingProviderFactory));

                base["factory"] = value;
            }
        }

        public ICachingProvider GetInstance()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingProviderFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance();
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
