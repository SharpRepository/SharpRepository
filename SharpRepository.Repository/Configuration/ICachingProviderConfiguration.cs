using System;
using System.Collections.Generic;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface ICachingProviderConfiguration
    {
        string Name { get; set; }
        Type Factory { get; set; }
        IDictionary<string, string> Attributes { get; set; }
        string this[string key] { get; }

        ICachingProvider GetInstance();
        bool ContainsKey(string key);
    }
}
