using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    public class CachingProviderCollection : Collection<CachingProviderElement>
    {
        public string Default { get; set; }

        public IList<ICachingProviderConfiguration> ToCachingProviderConfigurationList()
        {
            return this.Cast<CachingProviderElement>().Cast<ICachingProviderConfiguration>().ToList();
        }        
    }
}