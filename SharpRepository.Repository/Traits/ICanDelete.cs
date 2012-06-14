using System.Collections.Generic;

namespace SharpRepository.Repository.Traits
{
    /// <summary>
    /// Based on the Interface Segregation Principle (ISP), the
    /// ICanDelete interface exposes only the "Delete" methods of the
    /// Repository.    
    /// <see cref="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/"/>      
    /// </summary>
    /// <typeparam name="T">Generic repository entity type</typeparam>
    /// <typeparam name="TKey">Generic repository entity key type</typeparam>
    public interface ICanDelete<in T, in TKey>
    {
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void Delete(TKey key);
    }
}