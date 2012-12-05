using System.Linq;
using SharpRepository.InMemoryRepository;
using NUnit.Framework;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class InMemoryCachingTests : TestBase
    {
        [Test]
        public void ExecuteGetAll_With_Selector_Should_Use_Cache_After_First_Call()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.Add(new Contact { Name = "Test1"});
            repos.Add(new Contact { Name = "Test2"});

            var items = repos.GetAll(x => x.Name);
            repos.CacheUsed.ShouldBeFalse();
            items.Count().ShouldEqual(2);

            items = repos.GetAll(x => x.Name);
            repos.CacheUsed.ShouldBeTrue();
            items.Count().ShouldEqual(2);
        }

        [Test]
        public void ExecuteGetAll_Should_Use_Cache_After_First_Call()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.Add(new Contact { Name = "Test1" });
            repos.Add(new Contact { Name = "Test2" });

            var items = repos.GetAll();
            repos.CacheUsed.ShouldBeFalse();
            items.Count().ShouldEqual(2);

            items = repos.GetAll();
            repos.CacheUsed.ShouldBeTrue();
            items.Count().ShouldEqual(2);
        }

        [Test]
        public void ExecuteFindAll_With_Selector_Should_Use_Cache_After_First_Call()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.Add(new Contact { Name = "Test1" });
            repos.Add(new Contact { Name = "Test2" });

            var items = repos.FindAll(x => x.ContactId < 3, x => x.Name);
            repos.CacheUsed.ShouldBeFalse();
            items.Count().ShouldEqual(2);

            items = repos.FindAll(x => x.ContactId < 3, x => x.Name);
            repos.CacheUsed.ShouldBeTrue();
            items.Count().ShouldEqual(2);
        }

        [Test]
        public void ExecuteFindAll_Should_Use_Cache_After_First_Call()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.Add(new Contact { Name = "Test1" });
            repos.Add(new Contact { Name = "Test2" });

            var items = repos.FindAll(x => x.ContactId < 3);
            repos.CacheUsed.ShouldBeFalse();
            items.Count().ShouldEqual(2);

            items = repos.FindAll(x => x.ContactId < 3);
            repos.CacheUsed.ShouldBeTrue();
            items.Count().ShouldEqual(2);
        }

        [Test]
        public void ExecuteFind_With_Selector_Should_Use_Cache_After_First_Call()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.Add(new Contact { Name = "Test1" });
            repos.Add(new Contact { Name = "Test2" });

            var item = repos.Find(x => x.ContactId == 1, x => x.Name);
            repos.CacheUsed.ShouldBeFalse();
            item.ShouldNotBeNull();

            item = repos.Find(x => x.ContactId ==1, x => x.Name);
            repos.CacheUsed.ShouldBeTrue();
            item.ShouldNotBeNull();
        }

        [Test]
        public void ExecuteFind_Should_Use_Cache_After_First_Call()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.Add(new Contact { Name = "Test1" });
            repos.Add(new Contact { Name = "Test2" });

            var item= repos.Find(x => x.ContactId == 1);
            repos.CacheUsed.ShouldBeFalse();
            item.ShouldNotBeNull();

            item = repos.Find(x => x.ContactId == 1);
            repos.CacheUsed.ShouldBeTrue();
            item.ShouldNotBeNull();
        }

        [Test]
        public void ExecuteGet_With_Selector_Should_Use_Cache_After_First_Call()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.Add(new Contact { Name = "Test1" });
            repos.Add(new Contact { Name = "Test2" });

            var item = repos.Get(1, x => x.Name);
            repos.CacheUsed.ShouldBeFalse();
            item.ShouldNotBeNull();

            item = repos.Get(1, x => x.Name);
            repos.CacheUsed.ShouldBeTrue();
            item.ShouldNotBeNull();
        }

        [Test]
        public void ExecuteGet_Should_Use_Cache_After_First_Call()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.Add(new Contact { Name = "Test1" });
            repos.Add(new Contact { Name = "Test2" });

            var item= repos.Get(1);
            repos.CacheUsed.ShouldBeTrue();
            item.ShouldNotBeNull();
        }
    }
}
