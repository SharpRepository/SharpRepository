using System;
using System.Collections.Generic;
#if NET451
using System.Configuration;
#elif NETSTANDARD1_4
using System.Collections.ObjectModel;
#endif
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
#if NET451
    [ConfigurationCollection(typeof(RepositoryElement), AddItemName = "cachingStrategy", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CachingStrategyCollection : ConfigurationElementCollection
#elif NETSTANDARD1_4
    public class CachingStrategyCollection : Collection<CachingStrategyElement>
#endif
    {
#if NET451
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
#endif

#if NET451
        [ConfigurationProperty("default", IsRequired = false)]
#endif
        public string Default
        {
#if NET451
            get { return (string)base["default"]; }
            set { base["default"] = value; }
#elif NETSTANDARD1_4
            get;
            set;
#endif
        }

        public IList<ICachingStrategyConfiguration> ToCachingStrategyConfigurationList()
        {
            return this.Cast<CachingStrategyElement>().Cast<ICachingStrategyConfiguration>().ToList();
        }
    }
}