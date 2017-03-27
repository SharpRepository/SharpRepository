using System;
using System.Collections.Generic;
#if NET451
using System.Configuration;
#elif NETSTANDARD1_6
using System.Collections.ObjectModel;
#endif
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
#if NET451
    [ConfigurationCollection(typeof(RepositoryElement), AddItemName = "cachingProvider", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CachingProviderCollection : ConfigurationElementCollection
#elif NETSTANDARD1_6
    public class CachingProviderCollection : Collection<CachingProviderElement>
#endif
    {
#if NET451
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
#endif

#if NET451
        [ConfigurationProperty("default", IsRequired = false)]
#endif
        public string Default
        {
#if NET451
            get { return (string)base["default"]; }
            set { base["default"] = value; }
#elif NETSTANDARD1_6
            get;
            set;
#endif
        }

        public IList<ICachingProviderConfiguration> ToCachingProviderConfigurationList()
        {
            return this.Cast<CachingProviderElement>().Cast<ICachingProviderConfiguration>().ToList();
        }        
    }
}