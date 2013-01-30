using System.Linq;
using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
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
    }
}
