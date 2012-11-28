using System;
using System.Configuration;

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
        }
    }
}
