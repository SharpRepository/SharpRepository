namespace SharpRepository.Repository.Caching
{
    public static class Cache
    {
        // TODO: figure out how to do a global clear cache that clears for all repositories

        // global counter used to clear all caches across all repositories
        private static int _globalCachingPrefixCounter = 1;
        internal static int GlobalCachingPrefixCounter
        {
            get { return _globalCachingPrefixCounter; }
        }

        private static readonly object LockObject = new object();

//        /// <summary>
//        /// This will clear out all SharpRepository related caching across all of the repositories
//        /// </summary>
//        public static void ClearAll()
//        {
//            // this increments a static counter by 1
//            //  the static counter is used for all of the cache keys as part of the prefix
//            IncrementGlobalCachingPrefixCounter();
//        }
//
//        internal static void IncrementGlobalCachingPrefixCounter()
//        {
//            Interlocked.Increment(ref _globalCachingPrefixCounter);
//        }
    }
}
