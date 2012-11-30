using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    [ConfigurationCollection(typeof(RepositoryElement), AddItemName = "repository", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class RepositoriesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RepositoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return ((RepositoryElement)element).Name;
        }

        [ConfigurationProperty("default", IsRequired = false)]
        public string Default
        {
            get { return (string) base["default"]; }
            set { base["default"] = value; }
        }

        public IList<IRepositoryConfiguration> ToRepositoryConfigurationList()
        {
            return this.Cast<RepositoryElement>().Cast<IRepositoryConfiguration>().ToList();
        }
    }
}
