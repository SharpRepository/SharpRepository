using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryDeleteTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void Delete_Should_Remove_Item(IRepository<Contact, int> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId);
            result.ShouldNotBeNull();

            repository.Delete(contact);
            result = repository.Get(contact.ContactId);
            result.ShouldBeNull();
        }

        [ExecuteForAllRepositories]
        public void Delete_Should_Remove_Item_By_Key(IRepository<Contact, int> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId);
            result.ShouldNotBeNull();

            repository.Delete(contact.ContactId);
            result = repository.Get(contact.ContactId);
            result.ShouldBeNull();
        }

        //[Test]
        //[ExpectedException(typeof(Exception))]
        //public void Delete_Should_Throw_Exception_If_Item_Does_Not_Exist()
        //{
        //    Repository.Delete(new Contact());
        //}

        [ExecuteForAllRepositories]
        public void Delete_Should_Wait_To_Remove_Item_If_Item_Exists_In_BatchMode(IRepository<Contact, int> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);
            
            var result = repository.Get(contact.ContactId);
            result.ShouldNotBeNull();
            
            using (var batch = repository.BeginBatch())
            {
                batch.Delete(contact); // not really delete until call Save, because in BatchMode

                result = repository.Get(contact.ContactId);
                result.ShouldNotBeNull();

                batch.Commit(); // actually do the delete
            }

            result = repository.Get(contact.ContactId);
            result.ShouldBeNull();
        }

        [ExecuteForAllRepositories]
        public void Delete_Should_Remove_Multiple_Items(IRepository<Contact, int> repository)
        {
            IList<Contact> contacts = new List<Contact>
                                        {
                                            new Contact {Name = "Contact 1"},
                                            new Contact {Name = "Contact 2"},
                                            new Contact {Name = "Contact 3"},
                                        };

            repository.Add(contacts);
            var items = repository.GetAll().ToList();
            items.Count().ShouldEqual(3);

            repository.Delete(contacts.Take(2));
            items = repository.GetAll().ToList();
            items.Count().ShouldEqual(1);
            items.First().Name.ShouldEqual("Contact 3");
        }
    }
}