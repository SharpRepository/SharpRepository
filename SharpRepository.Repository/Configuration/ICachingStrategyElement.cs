using System;

namespace SharpRepository.Repository.Configuration
{
    public interface ICachingStrategyElement
    {
        string Name { get; set; }
        string CachingProvider { get; set; }
    }
}
