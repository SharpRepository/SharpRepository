using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class TimeoutCachingStrategyTests : TestBase
    {
        protected IRepository<Contact, int> Repository;

        [SetUp]
        public void Setup()
        {
            Repository = new InMemoryRepository<Contact, int>(new TimeoutCachingStrategy<Contact, int>(10) { CachePrefix = "#RepoTimeoutCache"});
        }

        [TearDown]
        public void Teardown()
        {
            Repository = null;
        }

        [Test]
        public void Second_Get_Call_Should_Get_Old_Item_From_Cache()
        {
            Repository.Add(new Contact() { Name = "Test User"}); // this does not add to cache yet

            var item = Repository.Get(1); // after this call it's in cache
            item.Name.ShouldEqual("Test User");

            Repository.Update(new Contact() { ContactId = 1, Name = "Test User EDITED" }); // does not update cache

            var item2 = Repository.Get(1); // should get from cache since the timeout hasn't happened
            item2.Name.ShouldEqual("Test User");
        }
    }
}
