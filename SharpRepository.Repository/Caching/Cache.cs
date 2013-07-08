using System;

namespace SharpRepository.Repository.Caching
{
    public static class Cache
    {
        public static ICachePrefixManager CachePrefixManager { get; set; }

        internal static int GlobalCachingPrefixCounter
        {
            get
            {
                return CachePrefixManager == null ? 1 : CachePrefixManager.Counter;
            }
        }

        /// <summary>
        /// This will clear out all SharpRepository related caching across all of the repositories.  You must configure the Cache.CachePrefixManager property based on if your code is on a single server or in a multiple server farm or cloud environment.
        /// </summary>
        public static void ClearAll()
        {
            if (CachePrefixManager == null)
                throw new Exception("You must configure the Cache.CachePrefixManager in order to handle clearing the global cache.  You can use the SingleServerCachePrefixManager if you are on a single server, and the MultiServerCachePrefixManager if you are in the cloud or on multiple servers and use a caching provider like Memcached or Redis.");

            // this increments a static counter by 1
            //  the static counter is used for all of the cache keys as part of the prefix                                                                                                                                                                      
            CachePrefixManager.IncrementCounter();
        }
    }
}
