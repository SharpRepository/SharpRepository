using System;
using System.Collections.Generic;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface ICachingStrategyConfiguration
    {
        string Name { get; set; }
        int? MaxResults { get; set; }
        Type Factory { get; set; }
        IDictionary<string, string> Attributes { get; set; }
        ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class, new();
        ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new();
        ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>() where T : class, new();
        string this[string key] { get; }
    }
}
