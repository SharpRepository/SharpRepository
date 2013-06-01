using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class ClearCacheTests
    {
//        [Test]
//        public void ClearAllCache_Changes_FullCachePrefix()
//        {
//            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());
//
//            var fullCachePrefix = repos.CachingStrategy.FullCachePrefix;
//            
//            // shouldn't change if we call it again
//            repos.CachingStrategy.FullCachePrefix.ShouldEqual(fullCachePrefix);
//
//            // clear out all cached items across all repositories
//            Cache.ClearAll();
//
//            // this should have changed this time
//            repos.CachingStrategy.FullCachePrefix.ShouldNotEqual(fullCachePrefix);
//        }

        [Test]
        public void ClearCache_Changes_FullCachePrefix()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            var fullCachePrefix = repos.CachingStrategy.FullCachePrefix;

            // shouldn't change if we call it again
            repos.CachingStrategy.FullCachePrefix.ShouldEqual(fullCachePrefix);

            // clear out only the cache for this specific repository
            repos.ClearCache();

            // this should have changed this time
            repos.CachingStrategy.FullCachePrefix.ShouldNotEqual(fullCachePrefix);
        }
    }
}
