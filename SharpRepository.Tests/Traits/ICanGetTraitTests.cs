using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Traits;
using SharpRepository.Tests.TestObjects;
using Should;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Tests.Traits
{
    [TestFixture]
    public class ICanGetTraitTests : TestBase
    {
        [Test]
        public void ICanGet_Exposes_Get_By_Id()
        {
            var repo = new ContactRepository();

            var contact = new Contact { Name = "Test User", ContactTypeId = 1 };
            repo.Add(contact);

            var result = repo.Get(contact.ContactId);
            result.Name.ShouldEqual(contact.Name);
            result.ContactTypeId.ShouldEqual(contact.ContactTypeId);
        }

        [Test]
        public void ICanGet_Exposes_GetAll()
        {
            var repo = new ContactRepository();
            
            for (int i = 1; i <= 5; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repo.Add(contact);
            }

            IEnumerable<Contact> result = repo.GetAll().ToList();
            result.Count().ShouldEqual(5);
        }

        [Test]
        public void ICanGet_Exposes_Get_With_Result()
        {
            var repo = new ContactRepository();

            var contact = new Contact { Name = "Test User" };
            repo.Add(contact);

            var result = repo.Get(1, x => new { contact.ContactId, contact.Name });
            result.Name.ShouldEqual(contact.Name);
        }

      
        private class ContactRepository : InMemoryRepository<Contact, int>, IContactRepository
        {
         
        }

        private interface IContactRepository : ICanGet<Contact, Int32>
        
        {

        }
    }
}