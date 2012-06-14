using System.Collections.Generic;

namespace SharpRepository.Repository.Traits
{
    /// <summary>
    /// Based on the Interface Segregation Principle (ISP), the
    /// ICanAdd interface exposes only the "Add" methods of the
    /// Repository.         
    /// <see cref="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/"/> 
    /// </summary>
    /// <typeparam name="T">Generic repository entity type</typeparam>
    public interface ICanAdd<in T>
    {
        void Add(T entity);
        void Add(IEnumerable<T> entities);
    }
}