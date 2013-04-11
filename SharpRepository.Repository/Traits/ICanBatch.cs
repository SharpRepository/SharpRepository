using SharpRepository.Repository.Transactions;

namespace SharpRepository.Repository.Traits
{
    /// <summary>
    /// Based on the Interface Segregation Principle (ISP), the
    /// ICanBatch interface exposes only the "Batch" methods of the
    /// Repository.         
    /// <see cref="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/"/> 
    /// </summary>
    /// <typeparam name="T">Generic repository entity type</typeparam>
    public interface ICanBatch<T>
    {
        /// <summary>
        /// Begins a batch mode process.  This allows multiple operations against the repository with the ability to commit or rollback.
        /// </summary>
        /// <returns></returns>
        IBatch<T> BeginBatch();
    }
}