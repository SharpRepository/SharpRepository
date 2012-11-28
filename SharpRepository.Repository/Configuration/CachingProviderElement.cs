using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public class CachingProviderElement : ConfigurationElement
    {
        private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string) base["name"]; }
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
            // load up the factory if it exists and use it, if not then load up the type directly
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
    }
}
