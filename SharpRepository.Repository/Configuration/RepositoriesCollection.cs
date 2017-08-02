using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    public class RepositoriesCollection : Collection<RepositoryElement>
    {
        public string Default
        {
            get;
            set;
        }

        public IList<IRepositoryConfiguration> ToRepositoryConfigurationList()
        {
            return this.Cast<RepositoryElement>().Cast<IRepositoryConfiguration>().ToList();
        }
    }
}