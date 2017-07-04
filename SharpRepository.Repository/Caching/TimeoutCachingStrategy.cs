namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Implements a simple timeout based caching strategy where all entities and queries are cached for a certain number of seconds.  This will allow for stale data to be served up if it hasn't timed out yet so please be aware of that.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository this is used with.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class TimeoutCachingStrategy<T, TKey> : TimeoutCachingStrategyBase<T, TKey> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutCachingStrategy&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <param name="cachingProvider">The caching provider.</param>
        public TimeoutCachingStrategy(int timeoutInSeconds, ICachingProvider cachingProvider)
            : base(timeoutInSeconds, null, cachingProvider)
        {
            
        }
    }

    /// <summary>
    /// Implements a simple timeout based caching strategy where all entities and queries are cached for a certain number of seconds.  This will allow for stale data to be served up if it hasn't timed out yet so please be aware of that.  The primary key of the entity is Int32 in this implementation.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository this is used with.</typeparam>
    public class TimeoutCachingStrategy<T> : TimeoutCachingStrategyBase<T, int> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutCachingStrategy&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="timeoutInSeconds">The cache timeout in seconds.</param>
        /// <param name="cachingProvider">The caching provider.</param>
        public TimeoutCachingStrategy(int timeoutInSeconds, ICachingProvider cachingProvider)
            : base(timeoutInSeconds, null, cachingProvider)
        {

        }
    }

    /// <summary>
    /// Implements a simple timeout based caching strategy where all entities and queries are cached for a certain number of seconds.  This will allow for stale data to be served up if it hasn't timed out yet so please be aware of that.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository this is used with.</typeparam>
    /// <typeparam name="TKey">The type of the first part of the compound primary key.</typeparam>
    /// <typeparam name="TKey2">The type of the second part of the compound primary key.</typeparam>
    public class TimeoutCachingStrategy<T, TKey, TKey2> : TimeoutCompoundKeyCachingStrategyBase<T, TKey, TKey2> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutCachingStrategy&lt;T, TKey, TKey2&gt;"/> class.
        /// </summary>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <param name="cachingProvider">The caching provider.</param>
        public TimeoutCachingStrategy(int timeoutInSeconds, ICachingProvider cachingProvider)
            : base(timeoutInSeconds, null, cachingProvider)
        {

        }
    }

    /// <summary>
    /// Implements a simple timeout based caching strategy where all entities and queries are cached for a certain number of seconds.  This will allow for stale data to be served up if it hasn't timed out yet so please be aware of that.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository this is used with.</typeparam>
    /// <typeparam name="TKey">The type of the first part of the compound primary key.</typeparam>
    /// <typeparam name="TKey2">The type of the second part of the compound primary key.</typeparam>
    /// <typeparam name="TKey3">The type of the third part of the compound primary key.</typeparam>
    public class TimeoutCachingStrategy<T, TKey, TKey2, TKey3> : TimeoutCompoundKeyCachingStrategyBase<T, TKey, TKey2, TKey3> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutCachingStrategy&lt;T, TKey, TKey2, TKey3&gt;"/> class.
        /// </summary>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <param name="cachingProvider">The caching provider.</param>
        public TimeoutCachingStrategy(int timeoutInSeconds, ICachingProvider cachingProvider)
            : base(timeoutInSeconds, null, cachingProvider)
        {

        }
    }
}
