using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    public class CachingStrategyCollection : Collection<CachingStrategyElement>
    {
        public string Default { get; set; }

        public IList<ICachingStrategyConfiguration> ToCachingStrategyConfigurationList()
        {
            return this.Cast<CachingStrategyElement>().Cast<ICachingStrategyConfiguration>().ToList();
        }
    }
}