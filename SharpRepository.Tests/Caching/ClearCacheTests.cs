using System;
using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Shouldly;
using Microsoft.Extensions.Caching.Memory;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class ClearCacheTests
    {
        private ICachingProvider cacheProvider;

        [SetUp]
        public void Setup()
        {
            cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
        }

        [Test]
        public void ClearAllCache_Throws_Exception_WIthout_OutOfBox()
        {
            try
            {
                Cache.ClearAll();
                Assert.Fail("No exception was thrown when it should have been");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void ClearAllCache_Changes_FullCachePrefix_When_Configured()
        {
            Cache.CachePrefixManager = new SingleServerCachePrefixManager();

            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>(cacheProvider));

            var fullCachePrefix = repos.CachingStrategy.FullCachePrefix;
            
            // shouldn't change if we call it again
            repos.CachingStrategy.FullCachePrefix.ShouldBe(fullCachePrefix);

            // clear out all cached items across all repositories
            Cache.ClearAll();

            // this should have changed this time
            repos.CachingStrategy.FullCachePrefix.ShouldNotBe(fullCachePrefix);
        }

        [Test]
        public void ClearCache_Changes_FullCachePrefix()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>(cacheProvider));

            var fullCachePrefix = repos.CachingStrategy.FullCachePrefix;

            // shouldn't change if we call it again
            repos.CachingStrategy.FullCachePrefix.ShouldBe(fullCachePrefix);

            // clear out only the cache for this specific repository
            repos.ClearCache();

            // this should have changed this time
            repos.CachingStrategy.FullCachePrefix.ShouldNotBe(fullCachePrefix);
        }
    }
}
