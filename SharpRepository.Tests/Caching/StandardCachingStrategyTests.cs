using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.TestObjects;
using Shouldly;
using Microsoft.Extensions.Caching.Memory;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class StandardCachingStrategyTests : TestBase
    {
        protected ICachingStrategy<Contact, int> CachingStrategy;
            
        [SetUp]
        public void Setup()
        {
            // need to clear out the InMemory cache before each test is run so that each is independent and won't effect the next one
            var provider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
            CachingStrategy = new StandardCachingStrategy<Contact, int>(provider) { CachePrefix = "#RepoStandardCache" };
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void TryGetResult_First_Call_Should_Return_False()
        {
            CachingStrategy.TryGetResult(1, out Contact result).ShouldBe(false);
            result.ShouldBe(null);
        }

        [Test]
        public void SaveGetResult_Should_Set_Cache()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetResult(1, contact);
            CachingStrategy.TryGetResult(1, out Contact result).ShouldBe(true);

            result.ContactId.ShouldBe(contact.ContactId);
            result.Name.ShouldBe(contact.Name);
        }

        [Test]
        public void SaveGetResult_With_WriteThrough_Disabled_Should_Not_Set_Cache()
        {
            ((StandardCachingStrategy<Contact, int>)CachingStrategy).WriteThroughCachingEnabled = false;

            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetResult(1, contact);
            CachingStrategy.TryGetResult(1, out Contact result).ShouldBe(false);
        }

        [Test]
        public void TryGetAllResult_First_Call_Should_Return_False()
        {
            CachingStrategy.TryGetAllResult(null, null, out IEnumerable<Contact> result).ShouldBe(false);
            result.ShouldBe(null);
        }

        [Test]
        public void SaveGetAllResult_Should_Set_Cache()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetAllResult(null, null, new [] { contact });
            CachingStrategy.TryGetAllResult(null, null, out IEnumerable<Contact> result).ShouldBe(true);

            result.Count().ShouldBe(1);
        }

        [Test]
        public void SaveGetAllResult_With_Generational_Disabled_Should_Not_Set_Cache()
        {
            ((StandardCachingStrategy<Contact, int>)CachingStrategy).GenerationalCachingEnabled = false;

            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetAllResult(null, null, new[] { contact });
            CachingStrategy.TryGetAllResult(null, null, out IEnumerable<Contact> result).ShouldBe(false);
        }

        [Test]
        public void TryGetAllResult_With_Different_QueryOptions_Should_Return_False()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetAllResult(new SortingOptions<Contact>("Name"), null, new[] { contact });
            CachingStrategy.TryGetAllResult(null, null, out IEnumerable<Contact> result).ShouldBe(false);
        }

        [Test]
        public void TryGetAllResult_With_Same_QueryOptions_Should_Return_True()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var sorting = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveGetAllResult(sorting, null, new[] { contact });
            CachingStrategy.TryGetAllResult(sorting, null, out IEnumerable<Contact> result).ShouldBe(true);

            result.Count().ShouldBe(1);
        }

        [Test]
        public void TryFindAllResult_First_Call_Should_Return_False()
        {
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = null;

            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out IEnumerable<Contact> result).ShouldBe(false);
            result.ShouldBe(null);
        }

        [Test]
        public void SaveFindAllResult_Should_Set_Cache()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = null;

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out IEnumerable<Contact> result).ShouldBe(true);
            result.Count().ShouldBe(1);

            queryOptions = new SortingOptions<Contact>("Name");
            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldBe(true);
            result.Count().ShouldBe(1);
        }

        [Test]
        public void TryFindAllResult_With_Different_QueryOptions_Should_Return_False()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, null, null, out IEnumerable<Contact> result).ShouldBe(false);


            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, new SortingOptions<Contact>("Name", true), null, out result).ShouldBe(false);
        }

        [Test]
        public void TryFindAllResult_With_Same_QueryOptions_Should_Return_True()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out IEnumerable<Contact> result).ShouldBe(true);

            result.Count().ShouldBe(1);
        }

        [Test]
        public void TryFindResult_First_Call_Should_Return_False()
        {
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = null;

            CachingStrategy.TryFindResult(specification, queryOptions, null, out Contact result).ShouldBe(false);
            result.ShouldBe(null);
        }

        [Test]
        public void SaveFindResult_Should_Set_Cache()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = null;

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out Contact result).ShouldBe(true);

            queryOptions = new SortingOptions<Contact>("Name");
            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldBe(true);
        }

        [Test]
        public void TryFindResult_With_Different_QueryOptions_Should_Return_False()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, null, null, out Contact result).ShouldBe(false);


            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, new SortingOptions<Contact>("Name", true), null, out result).ShouldBe(false);
        }

        [Test]
        public void TryFindResult_With_Same_QueryOptions_Should_Return_True()
        {
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out Contact result).ShouldBe(true);
        }
    }
}
