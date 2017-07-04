using System.Threading;
using NUnit.Framework;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Shouldly;
using SharpRepository.InMemoryRepository;
using Microsoft.Extensions.Caching.Memory;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class TimeoutCachingStrategyTests : TestBase
    {
        private ICachingProvider cacheProvider;

        [SetUp]
        public void Setup()
        {
            cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
        }

        [Test]
        public void Second_Get_Call_Should_Get_New_Item_From_Cache()
        {
            var repository = new InMemoryRepository<Contact, int>(new TimeoutCachingStrategy<Contact, int>(10, cacheProvider) { CachePrefix = "#RepoTimeoutCache"});

            repository.Add(new Contact() { Name = "Test User"});

            var item = repository.Get(1); // after this call it's in cache
            item.Name.ShouldBe("Test User");

            repository.Update(new Contact() { ContactId = 1, Name = "Test User EDITED" }); // does update cache

            var item2 = repository.Get(1); // should get from cache since the timeout hasn't happened
            item2.Name.ShouldBe("Test User EDITED");
        }

        [Test]
        public void Cache_Should_Timeout()
        {
            var repository = new InMemoryRepository<Contact, int>(new TimeoutCachingStrategy<Contact, int>(2, cacheProvider) { CachePrefix = "#RepoTimeoutCache" });
            repository.Add(new Contact() { Name = "Test User" });

            repository.Get(1); 
            repository.CacheUsed.ShouldBeTrue();

            Thread.Sleep(5000);

            repository.Get(1);
            repository.CacheUsed.ShouldBeFalse();
        }
    }
}
