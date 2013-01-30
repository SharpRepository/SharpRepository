using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using NUnit.Framework;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class StandardCachingWithPartitionStrategyTests : TestBase
    {
        protected ICachingStrategy<Contact, int> CachingStrategy;
            
        [SetUp]
        public void Setup()
        {
            // need to clear out the InMemory cache before each test is run so that each is independent and won't effect the next one
            var cache = MemoryCache.Default;
            foreach (var item in cache)
            {
                cache.Remove(item.Key);
            }

            CachingStrategy = new StandardCachingStrategy<Contact, int, int>(c => c.ContactTypeId) { CachePrefix = "#RepoStandardCacheWithPartition" };
        }

        [TearDown]
        public void Teardown()
        {
            //Repository = null;
        }

        [Test]
        public void TryGetResult_First_Call_Should_Return_False()
        {
            Contact result;
            CachingStrategy.TryGetResult(1, null, out result).ShouldEqual(false);
            result.ShouldEqual(null);
        }

        [Test]
        public void SaveGetResult_Should_Set_Cache()
        {
            Contact result;
            var contact = new Contact() {ContactId = 1, Name = "Test User"};

            CachingStrategy.SaveGetResult(1, null, contact);
            CachingStrategy.TryGetResult(1, null, out result).ShouldEqual(true);

            result.ContactId.ShouldEqual(contact.ContactId);
            result.Name.ShouldEqual(contact.Name);
        }

        [Test]
        public void SaveGetResult_With_WriteThrough_Disabled_Should_Not_Set_Cache()
        {
            ((StandardCachingStrategy<Contact, int, int>)CachingStrategy).WriteThroughCachingEnabled = false;

            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetResult(1, null, contact);
            CachingStrategy.TryGetResult(1, null, out result).ShouldEqual(false);
        }

        [Test]
        public void TryGetAllResult_First_Call_Should_Return_False()
        {
            IEnumerable<Contact> result;
            CachingStrategy.TryGetAllResult(null, null, out result).ShouldEqual(false);
            result.ShouldEqual(null);
        }

        [Test]
        public void SaveGetAllResult_Should_Set_Cache()
        {
            IEnumerable<Contact> result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetAllResult(null, null, new[] { contact });
            CachingStrategy.TryGetAllResult(null, null, out result).ShouldEqual(true);

            result.Count().ShouldEqual(1);
        }

        [Test]
        public void SaveGetAllResult_With_Generational_Disabled_Should_Not_Set_Cache()
        {
            ((StandardCachingStrategy<Contact, int, int>)CachingStrategy).GenerationalCachingEnabled = false;

            IEnumerable<Contact> result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetAllResult(null, null, new[] { contact });
            CachingStrategy.TryGetAllResult(null, null, out result).ShouldEqual(false);
        }

        [Test]
        public void TryGetAllResult_With_Different_QueryOptions_Should_Return_False()
        {
            IEnumerable<Contact> result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };

            CachingStrategy.SaveGetAllResult(new SortingOptions<Contact>("Name"), null, new[] { contact });
            CachingStrategy.TryGetAllResult(null, null, out result).ShouldEqual(false);
        }

        [Test]
        public void TryGetAllResult_With_Same_QueryOptions_Should_Return_True()
        {
            IEnumerable<Contact> result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var sorting = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveGetAllResult(sorting, null, new[] { contact });
            CachingStrategy.TryGetAllResult(sorting, null, out result).ShouldEqual(true);

            result.Count().ShouldEqual(1);
        }

        [Test]
        public void TryFindAllResult_First_Call_Should_Return_False()
        {
            IEnumerable<Contact> result;
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = null;

            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(false);
            result.ShouldEqual(null);
        }

        [Test]
        public void SaveFindAllResult_Should_Set_Cache()
        {
            IEnumerable<Contact> result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = null;

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);
            result.Count().ShouldEqual(1);

            queryOptions = new SortingOptions<Contact>("Name");
            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);
            result.Count().ShouldEqual(1);
        }

        [Test]
        public void TryFindAllResult_With_Different_QueryOptions_Should_Return_False()
        {
            IEnumerable<Contact> result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, null, null, out result).ShouldEqual(false);


            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, new SortingOptions<Contact>("Name", true), null, out result).ShouldEqual(false);
        }

        [Test]
        public void TryFindAllResult_With_Same_QueryOptions_Should_Return_True()
        {
            IEnumerable<Contact> result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);

            result.Count().ShouldEqual(1);
        }

        [Test]
        public void TryFindResult_First_Call_Should_Return_False()
        {
            Contact result;
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = null;

            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(false);
            result.ShouldEqual(null);
        }

        [Test]
        public void SaveFindResult_Should_Set_Cache()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = null;

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);

            queryOptions = new SortingOptions<Contact>("Name");
            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindResult_With_Different_QueryOptions_Should_Return_False()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, null, null, out result).ShouldEqual(false);


            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, new SortingOptions<Contact>("Name", true), null, out result).ShouldEqual(false);
        }

        [Test]
        public void TryFindResult_With_Same_QueryOptions_Should_Return_True()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User" };
            var specification = new Specification<Contact>(x => x.ContactId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        // Now Parition specific ones
        //  Paritions only effect the Find and FindAll
        [Test]
        public void TryFindResult_After_Add_To_Partition_Should_Return_False()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1};
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);

            var contact2 = new Contact() {ContactId = 2, Name = "Test User 2", ContactTypeId = 1};
            CachingStrategy.Add(2, contact2);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(false);
            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);

            // after saving the new results in the next generation then it should find it
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindResult_After_Add_To_Different_Partition_Should_Return_True()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);

            var contact2 = new Contact() { ContactId = 2, Name = "Test User 2", ContactTypeId = 2 };
            CachingStrategy.Add(2, contact2);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindResult_After_Update_To_Partition_Should_Return_False()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);

            contact.Name = "Test User - EDITED";
            CachingStrategy.Update(1, contact);

            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(false);
            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);

            // after saving the new results in the next generation then it should find it
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindResult_After_Update_To_Different_Partition_Should_Return_True()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);

            var contact2 = new Contact() { ContactId = 2, Name = "Test User 2", ContactTypeId = 2 };
            CachingStrategy.Update(2, contact2);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindResult_After_Delete_To_Partition_Should_Return_False()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);

            CachingStrategy.Delete(1, contact);

            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(false);
            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);

            // after saving the new results in the next generation then it should find it
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindResult_After_Delete_To_Different_Partition_Should_Return_True()
        {
            Contact result;
            var contact = new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);

            var contact2 = new Contact() { ContactId = 2, Name = "Test User 2", ContactTypeId = 2 };
            CachingStrategy.Delete(2, contact2);
            CachingStrategy.TryFindResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }


        [Test]
        public void TryFindAllResult_After_Add_To_Partition_Should_Return_False()
        {
            IEnumerable<Contact> result;
            var contacts = new[] { new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 } };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);

            var contact2 = new Contact() { ContactId = 2, Name = "Test User 2", ContactTypeId = 1 };
            CachingStrategy.Add(2, contact2);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(false);
            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);

            // after saving the new results in the next generation then it should find it
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindAllResult_After_Add_To_Different_Partition_Should_Return_True()
        {
            IEnumerable<Contact> result;
            var contacts = new[] { new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 } };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);

            var contact2 = new Contact() { ContactId = 2, Name = "Test User 2", ContactTypeId = 2 };
            CachingStrategy.Add(2, contact2);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindAllResult_After_Update_To_Partition_Should_Return_False()
        {
            IEnumerable<Contact> result;
            var contacts = new[] { new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 } };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);

            contacts[0].Name = "Test User - EDITED";
            CachingStrategy.Update(1, contacts[0]);

            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(false);
            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);

            // after saving the new results in the next generation then it should find it
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindAllResult_After_Update_To_Different_Partition_Should_Return_True()
        {
            IEnumerable<Contact> result;
            var contacts = new [] { new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 } };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);

            var contact2 = new Contact() { ContactId = 2, Name = "Test User 2", ContactTypeId = 2 };
            CachingStrategy.Update(2, contact2);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindAllResult_After_Delete_To_Partition_Should_Return_False()
        {
            IEnumerable<Contact> result;
            var contacts = new[] { new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 } };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);

            CachingStrategy.Delete(1, contacts[0]);

            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(false);
            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);

            // after saving the new results in the next generation then it should find it
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }

        [Test]
        public void TryFindAllResult_After_Delete_To_Different_Partition_Should_Return_True()
        {
            IEnumerable<Contact> result;
            var contacts = new[] { new Contact() { ContactId = 1, Name = "Test User", ContactTypeId = 1 } };
            var specification = new Specification<Contact>(x => x.ContactTypeId == 1);
            IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name", true);

            CachingStrategy.SaveFindAllResult(specification, queryOptions, null, contacts);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);

            var contact2 = new Contact() { ContactId = 2, Name = "Test User 2", ContactTypeId = 2 };
            CachingStrategy.Delete(2, contact2);
            CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).ShouldEqual(true);
        }
    }
}
