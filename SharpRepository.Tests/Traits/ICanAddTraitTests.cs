using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Traits;
using SharpRepository.Tests.TestObjects;
using Should;
using SharpRepository.InMemoryRepository;

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