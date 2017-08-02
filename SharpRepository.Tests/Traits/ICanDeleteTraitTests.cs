using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Traits;
using SharpRepository.Tests.TestObjects;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Tests.Traits
{
    [TestFixture]
    public class ICanDeleteTraitTests : TestBase
    {
        [Test]
        public void ICanDelete_Exposes_Delete_Entity()
        {
            var repo = new ContactRepository();
            var contact = new Contact { Name = "Test User" };
            repo.Add(contact);
            repo.Delete(contact);
        }

        [Test]
        public void ICanDelete_Exposes_Delete_Multiple_Entities()
        {
            var repo = new ContactRepository();
            var contact = new Contact { Name = "Test User" };
            repo.Add(contact);
            repo.Add(contact);
            repo.Delete(new List<Contact> { contact, contact });
        }

        [Test]
        public void ICanDelete_Exposes_Delete_By_Id()
        {
            var repo = new ContactRepository();
            var contact = new Contact { Name = "Test User" };
            repo.Add(contact);
            repo.Delete(1);
        }

        private class ContactRepository : InMemoryRepository<Contact, int>, IContactRepository
        {
        }

        private interface IContactRepository : ICanDelete<Contact, Int32>
        {
        }
    }
}