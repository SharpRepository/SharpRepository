using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class DisabledCacheTests
    {
        [Test]
        public void Using_DisableCaching_Should_Disable_Cache_Inside_Using_Block()
        {
            var cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>(cacheProvider));

            repos.CachingEnabled.ShouldBeTrue();

            using (repos.DisableCaching())
            {
                repos.CachingEnabled.ShouldBeFalse();
            }

            repos.CachingEnabled.ShouldBeTrue();
        }
    }
}
