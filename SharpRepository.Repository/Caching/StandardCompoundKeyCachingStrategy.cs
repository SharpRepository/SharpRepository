
namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Implements Write-Through caching for all CRUD operations (writing to the database and cache at the same time), and Generational caching for all queries (FindAll, GetAll, Find) with the option to partition the Generational Cache based on a specific entity property for better performance in certain situations.
    /// </summary>
    /// <typeparam name="T">Type of the entity the corresponding repository queries against.</typeparam>
    public class StandardCompoundKeyCachingStrategy<T> : StandardCompoundKeyCachingStrategyBase<T, int> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardCachingStrategy&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="cachingProvider">The caching provider to use (e.g. <see cref="InMemoryCachingProvider"/>, <see cref="MemcachedCachingProvider"/>, etc.).  Defaults to <see cref="InMemoryCachingProvider"/>.</param>
        public StandardCompoundKeyCachingStrategy(ICachingProvider cachingProvider)
            : base(null, cachingProvider)
        {
            Partition = null;
        }
    }
}
