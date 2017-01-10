using System;
using System.Collections.Generic;

namespace SharpRepository.Repository.Configuration
{
    public interface IRepositoryConfiguration
    {
        string Name { get; set; }
        Type Factory { get; set; }
        string CachingStrategy { get; set; }
        string CachingProvider { get; set; }
        IDictionary<string, string> Attributes { get; set; }
        string this[string key] { get; }

        IRepository<T> GetInstance<T>() where T : class, new();
        IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
        ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new();
        ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>() where T : class, new();
        ICompoundKeyRepository<T> GetCompoundKeyInstance<T>() where T : class, new();
    }
}
