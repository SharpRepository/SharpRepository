using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Traits;
using SharpRepository.Tests.TestObjects;
using System.Collections.Generic;

namespace SharpRepository.Tests.Traits
{
    [TestFixture]
    public class ICanUpdateTraitTests : TestBase
    {
        [Test]
        public void ICanUpdate_Exposes_Update_Entity()
        {
            var repo = new ContactRepository();

            var contact = new Contact { Name = "Name" };
            repo.Add(contact);

            contact.Name = "New Name";
            repo.Update(contact);
        }

        [Test]
        public void ICanUpdate_Exposes_Update_Multiple_Entities()
        {
            var repo = new ContactRepository();
            
            IList<Contact> contacts = new List<Contact>
                                        {
                                            new Contact {Name = "Contact 1"},
                                            new Contact {Name = "Contact 2"},
                                            new Contact {Name = "Contact 3"},
                                        };

            repo.Add(contacts);
            
            foreach (var contact in contacts)
            {
                contact.Name += " New Name";
            }

            repo.Update(contacts);
        }
       
        private class ContactRepository : InMemoryRepository<Contact, int>, IContactRepository
        {
        }

        private interface IContactRepository : ICanUpdate<Contact>
        {
        }
    }
}