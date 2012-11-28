using System;
using System.Configuration;

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
        }
    }
}
