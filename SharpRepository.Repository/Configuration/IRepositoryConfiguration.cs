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

        IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
    }
}
