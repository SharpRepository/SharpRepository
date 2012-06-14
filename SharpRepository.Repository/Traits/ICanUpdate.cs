using System.Collections.Generic;

namespace SharpRepository.Repository.Traits
{
    /// <summary>
    /// Based on the Interface Segregation Principle (ISP), the
    /// ICanUpdate interface exposes only the "Update" methods of the
    /// Repository.        
    /// <see cref="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/"/>  
    /// </summary>
    /// <typeparam name="T">Generic repository entity type</typeparam>
    public interface ICanUpdate<in T>
    {
        void Update(T entity);
        void Update(IEnumerable<T> entities);
    }
}