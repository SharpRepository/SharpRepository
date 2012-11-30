using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    [ConfigurationCollection(typeof(RepositoryElement), AddItemName = "cachingStrategy", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CachingStrategyCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CachingStrategyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((CachingStrategyElement)element).Name;
        }

        [ConfigurationProperty("default", IsRequired = false)]
        public string Default
        {
            get { return (string)base["default"]; }
            set { base["default"] = value; }
        }

        public IList<ICachingStrategyConfiguration> ToCachingStrategyConfigurationList()
        {
            return this.Cast<CachingStrategyElement>().Cast<ICachingStrategyConfiguration>().ToList();
        }
    }
}
