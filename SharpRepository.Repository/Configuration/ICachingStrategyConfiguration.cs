using System;
using System.Collections.Generic;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface ICachingStrategyConfiguration
    {
        string Name { get; set; }
        Type Factory { get; set; }
        IDictionary<string, string> Attributes { get; set; }
        ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class, new();
        string this[string key] { get; }
    }
}
