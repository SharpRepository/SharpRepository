using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Traits;
using SharpRepository.Tests.TestObjects;
using System.Collections.Generic;

namespace SharpRepository.Tests.Traits
{
    [TestFixture]
    public class ICanAddTraitTests : TestBase
    {
        [Test]
        public void ICanAdd_Exposes_Add_Entity()
        {
            var repo = new ContactRepository();
            repo.Add(new Contact());
        }

        [Test]
        public void ICanAdd_Exposes_Add_Multiple_Entities()
        {
            var repo = new ContactRepository();
            repo.Add(new List<Contact> { new Contact(), new Contact() });
        }

        private class ContactRepository : InMemoryRepository<Contact, int>, IContactRepository
        {
        }

        private interface IContactRepository : ICanAdd<Contact>
        {
        }
    }
}