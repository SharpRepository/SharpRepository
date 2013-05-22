using System.Runtime.Caching;
using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class DisabledCacheTests
    {
        [SetUp]
        public void Setup()
        {
            // need to clear out the InMemory cache before each test is run so that each is independent and won't effect the next one
            var cache = MemoryCache.Default;
            foreach (var item in cache)
            {
                cache.Remove(item.Key);
            }
        }

        [TearDown]
        public void Teardown()
        {

        }

        [Test]
        public void Using_DisableCaching_Should_Disable_Cache_Inside_Using_Block()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.CachingEnabled.ShouldBeTrue();

            using (repos.DisableCaching())
            {
                repos.CachingEnabled.ShouldBeFalse();
            }

            repos.CachingEnabled.ShouldBeTrue();
        }
    }
}
