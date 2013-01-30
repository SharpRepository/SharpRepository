using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    [ConfigurationCollection(typeof(RepositoryElement), AddItemName = "cachingProvider", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CachingProviderCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CachingProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((CachingProviderElement)element).Name;
        }

        [ConfigurationProperty("default", IsRequired = false)]
        public string Default
        {
            get { return (string)base["default"]; }
            set { base["default"] = value; }
        }

        public IList<ICachingProviderConfiguration> ToCachingProviderConfigurationList()
        {
            return this.Cast<CachingProviderElement>().Cast<ICachingProviderConfiguration>().ToList();
        }
    }
}
