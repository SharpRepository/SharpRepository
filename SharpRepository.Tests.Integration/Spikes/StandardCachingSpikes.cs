using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.EfRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class StandardCachingSpikes
    {
        // Tests validate fix for Issue #40 - Find/FindAll results are cached without consideration for predicate values
        // https://github.com/SharpRepository/SharpRepository/issues/40
        [Test]
        public void FindShouldBeCachedWithSinglePredicateValueHash()
        {
            var repository = new InMemoryRepository<Contact, string>(new StandardCachingStrategy<Contact, string>()); // by default uses InMemoryCache

            for (var i = 1; i < 5; i++)
            {
                repository.Add(new Contact { ContactId = i.ToString(), Name = "Contact " + i, ContactTypeId = 1 });
            }

            var contactId = "1";
            var contact = repository.Find(x => x.ContactId == contactId);
            contact.ContactId.ShouldEqual(contactId);

            contactId = "2";
            contact = repository.Find(x => x.ContactId == contactId);
            contact.ContactId.ShouldEqual(contactId);
        }
        
        [Test]
        public void FindShouldBeCachedWithMultiplePredicateValueHash()
        {
            var repository = new InMemoryRepository<Contact, string>(new StandardCachingStrategy<Contact, string>()); // by default uses InMemoryCache

            for (var i = 1; i < 5; i++)
            {
                repository.Add(new Contact { ContactId = i.ToString(), Name = "Contact " + i, ContactTypeId = 1});
            }

            var contactId = "1";
            repository.Find(x => x.ContactTypeId == 1 && x.ContactId == contactId)
                .ContactId.ShouldEqual(contactId);

            contactId = "2";
            repository.Find(x => x.ContactTypeId == 1 && x.ContactId == contactId)
                .ContactId.ShouldEqual(contactId);
        }

        [Test]
        public void FindAllShouldBeCachedWithSinglePredicateValueHash()
        {
            var repository = new InMemoryRepository<Contact, string>(new StandardCachingStrategy<Contact, string>()); // by default uses InMemoryCache

            for (var i = 1; i < 5; i++)
            {
                repository.Add(new Contact { ContactId = i.ToString(), Name = "Contact " + i, ContactTypeId = 1 });
            }

            var contactId = "1";
            repository.FindAll(x => x.ContactId == contactId)
                .First().ContactId.ShouldEqual(contactId);

            contactId = "2";
            repository.FindAll(x => x.ContactId == contactId)
                .First().ContactId.ShouldEqual(contactId);
        }

        [Test]
        public void FindAllShouldBeCachedWithMultiplePredicateValueHash()
        {
            var repository = new InMemoryRepository<Contact, string>(new StandardCachingStrategy<Contact, string>()); // by default uses InMemoryCache

            for (var i = 1; i < 5; i++)
            {
                repository.Add(new Contact { ContactId = i.ToString(), Name = "Contact " + i, ContactTypeId = 1 });
            }

            var contactId = "1";
            repository.FindAll(x => x.ContactTypeId == 1 && x.ContactId == contactId)
                .First().ContactId.ShouldEqual(contactId);

            contactId = "2";
            repository.FindAll(x => x.ContactTypeId == 1 && x.ContactId == contactId)
                .First().ContactId.ShouldEqual(contactId);
        }

        [Test]
        public void Get_With_Selector_Should_Not_Use_Cache_If_Entity_Updated()
        {
            var repository = new InMemoryRepository<Contact, string>(new StandardCachingStrategy<Contact, string>()); // by default uses InMemoryCache

            for (var i = 1; i < 3; i++)
            {
                repository.Add(new Contact { ContactId = i.ToString(), Name = "Contact " + i, ContactTypeId = 1 });
            }

            const string contactId = "1";

            var contactName = repository.Get(contactId, c => c.Name);
            contactName.ShouldEqual("Contact 1");

            var contact = repository.Get(contactId);
            contact.Name = "Contact 1 - EDITED";

            contactName = repository.Get(contactId, c => c.Name);
            contactName.ShouldEqual("Contact 1 - EDITED");
        }

        [Test]
        public void Delete_With_Cache_And_Ef()
        {
            var cachingStrategy = new StandardCachingStrategy<Contact, string>();
            var dbPath = EfDataDirectoryFactory.Build();
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");

            var repository = new EfRepository<Contact, string>(new TestObjectEntities("Data Source=" + dbPath), cachingStrategy);

            repository.Add(new Contact() { ContactId = "1", Name = "Contact1"});

            repository = new EfRepository<Contact, string>(new TestObjectEntities("Data Source=" + dbPath), cachingStrategy);
            repository.Get("1");
            repository.CacheUsed.ShouldBeTrue();
            repository.Delete("1");
        }

        [Test]
        public void Delete_Loop_With_Cache_And_Ef()
        {
            var cachingStrategy = new StandardCachingStrategy<Contact, string>();
            var dbPath = EfDataDirectoryFactory.Build();
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");

            var repository = new EfRepository<Contact, string>(new TestObjectEntities("Data Source=" + dbPath), cachingStrategy);

            repository.Add(new Contact() { ContactId = "1", Name = "Contact1", ContactTypeId = 1});
            repository.Add(new Contact() { ContactId = "2", Name = "Contact2", ContactTypeId = 2});
            repository.Add(new Contact() { ContactId = "3", Name = "Contact3", ContactTypeId = 2});
            repository.FindAll(x => x.ContactTypeId == 2);

            repository = new EfRepository<Contact, string>(new TestObjectEntities("Data Source=" + dbPath), cachingStrategy);
            
            repository.Delete(x => x.ContactTypeId == 2);
        }
    }
}
