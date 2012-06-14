using System;
using System.Linq.Expressions;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class CachingTests : TestBase
    {
        [Test]
        public void Same_Predicate_Will_Give_Same_ToString_Value()
        {
            Expression<Func<Contact, bool>> predicate = contact => (contact.Name == "test" && contact.ContactId == 1);
            Expression<Func<Contact, bool>> predicate2 = contact => (contact.Name == "test" && contact.ContactId == 1);

            var hash1 = predicate.ToString();
            var hash2 = predicate2.ToString();

            hash1.ShouldEqual(hash2);
        }

        [Test]
        public void Same_Specification_Will_Give_Same_ToString_Value()
        {
            var spec = new Specification<Contact>(p => p.ContactId == 1)
                .And(new Specification<Contact>(p => p.Name.Equals("test")));

            var spec2 = new Specification<Contact>(p => p.ContactId == 1)
                .And(new Specification<Contact>(p => p.Name.Equals("test")));

            var hash1 = spec.ToString();
            var hash2 = spec2.ToString();

            hash1.ShouldEqual(hash2);
        }

        [Test]
        public void Different_Specification_Param_Will_Give_Different_ToString_Value()
        {
            var spec = new Specification<Contact>(p => p.ContactId == 1)
                .And(new Specification<Contact>(p => p.Name.Equals("test")));

            var spec2 = new Specification<Contact>(p => p.ContactId == 2)
                .And(new Specification<Contact>(p => p.Name.Equals("test")));

            var hash1 = spec.ToString();
            var hash2 = spec2.ToString();

            hash1.ShouldNotEqual(hash2);
        }

        [Test]
        public void Different_Specification_Ordering_Will_Give_Different_ToString_Value()
        {
            var spec = new Specification<Contact>(p => p.ContactId == 1)
                .And(new Specification<Contact>(p => p.Name.Equals("test")));

            var spec2 = new Specification<Contact>(p => p.Name.Equals("test"))
                .And(new Specification<Contact>(p => p.ContactId == 1));

            var hash1 = spec.ToString();
            var hash2 = spec2.ToString();

            hash1.ShouldNotEqual(hash2);
        }

        //[Test]
        //public void Testing_Cache_Through_Debug_Breaks()
        //{
        //    var cachingStrategy = new StandardCachingStrategy<Contact, int>(new InMemoryCachingProvider());
        //    var repository = new InMemoryRepository<Contact, int>(cachingStrategy);

        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 1"});
        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 2" });
        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 3" });
        //    repository.Add(new Contact() { ContactTypeId = 2, Name = "Personal User 1" });
        //    repository.Add(new Contact() { ContactTypeId = 2, Name = "Personal User 2" });

        //    var workUsers = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers = repository.FindAll(x => x.ContactTypeId == 2);

        //    var workUsers2 = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers2 = repository.FindAll(x => x.ContactTypeId == 2);

        //    // this should increment generation for both queries above
        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 4" });

        //    var workUsers3 = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers3 = repository.FindAll(x => x.ContactTypeId == 2);
        //}

        //[Test]
        //public void Testing_Cache_Partition_Through_Debug_Breaks()
        //{
        //    var cachingStrategy = new StandardCachingWithPartitionStrategy<Contact, int, int>(new InMemoryCachingProvider());
        //    cachingStrategy.Partition = c => c.ContactTypeId;
        //    var repository = new InMemoryRepository<Contact, int>(cachingStrategy);

        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 1" });
        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 2" });
        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 3" });
        //    repository.Add(new Contact() { ContactTypeId = 2, Name = "Personal User 1" });
        //    repository.Add(new Contact() { ContactTypeId = 2, Name = "Personal User 2" });

        //    var workUsers = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers = repository.FindAll(x => x.ContactTypeId == 2);

        //    var workUsers2 = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers2 = repository.FindAll(x => x.ContactTypeId == 2);

        //    // this should increment generation for both queries above
        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 4" });

        //    var workUsers3 = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers3 = repository.FindAll(x => x.ContactTypeId == 2);
        //}

        //[Test]
        //public void Testing_Cache_Partition_With_BatchMode_Through_Trace()
        //{
        //    var cachingStrategy = new StandardCachingWithPartitionStrategy<Contact, int, int>(new InMemoryCachingProvider());
        //    cachingStrategy.Partition = c => c.ContactTypeId;
        //    var repository = new InMemoryRepository<Contact, int>(cachingStrategy);
            
        //    using (var batch = repository.BeginBatch())
        //    {
        //        batch.Add(new Contact() {ContactTypeId = 1, Name = "Work User 1"});
        //        batch.Add(new Contact() {ContactTypeId = 1, Name = "Work User 2"});
        //        batch.Add(new Contact() {ContactTypeId = 1, Name = "Work User 3"});
        //        batch.Add(new Contact() {ContactTypeId = 2, Name = "Personal User 1"});
        //        batch.Add(new Contact() {ContactTypeId = 2, Name = "Personal User 2"});
        //        batch.Commit();
        //    }

        //    var workUsers = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers = repository.FindAll(x => x.ContactTypeId == 2);

        //    var workUsers2 = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers2 = repository.FindAll(x => x.ContactTypeId.Equals(2));

        //    // this should increment generation for both queries above
        //    repository.Add(new Contact() { ContactTypeId = 1, Name = "Work User 4" });
        //    repository.Save();

        //    var workUsers3 = repository.FindAll(x => x.ContactTypeId == 1);
        //    var personalUsers3 = repository.FindAll(x => x.ContactTypeId == 2);
        //}
    }
}
