using System.Linq;
using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class StandardCachingSpike
    {
        [Test]
        public void GeneralTest()
        {
            var repository = new InMemoryRepository<Contact, string>(new StandardCachingStrategy<Contact, string>()); // by default uses InMemoryCache

            var contact = repository.Get("3");
            contact.ShouldBeNull();

            for (var i = 1; i < 10; i++)
                repository.Add(new Contact { ContactId = i.ToString(), Name = "Contact " + i });

            contact = repository.Get("3");
            contact.Name.ShouldEqual("Contact 3");

            var contacts = repository.FindAll(x => x.Name.Contains("tact"));

            contacts.Count().ShouldEqual(9);

            contacts = repository.FindAll(x => x.Name.EndsWith("tact 3"));
            contacts.Count().ShouldEqual(1);

            contacts = repository.FindAll(x => x.Name.EndsWith("tact 8"));
            contacts.Count().ShouldEqual(1);
            contacts.First().Name.ShouldEqual("Contact 8");

        }
    }
}
