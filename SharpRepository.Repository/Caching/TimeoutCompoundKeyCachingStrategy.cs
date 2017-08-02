
namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Implements a simple timeout based caching strategy where all entities and queries are cached for a certain number of seconds.  This will allow for stale data to be served up if it hasn't timed out yet so please be aware of that.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository this is used with.</typeparam>
    public class TimeoutCompoundKeyCachingStrategy<T> : TimeoutCompoundKeyCachingStrategyBase<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutCachingStrategy&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <param name="cachingProvider">The caching provider.</param>
        public TimeoutCompoundKeyCachingStrategy(int timeoutInSeconds, ICachingProvider cachingProvider)
            : base(timeoutInSeconds, null, cachingProvider)
        {

        }
    }
}
