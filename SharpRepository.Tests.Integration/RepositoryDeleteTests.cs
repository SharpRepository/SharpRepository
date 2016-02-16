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
        public void Delete_Should_Remove_Item(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);

            //var result = repository.Get(contact.ContactId);
            //result.ShouldNotBeNull();

            repository.Delete(contact);
            //result = repository.Get(contact.ContactId);
            //result.ShouldBeNull();
        }

        [ExecuteForAllRepositories]
        public void Delete_Should_Remove_Item_By_Key(IRepository<Contact, string> repository)
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
        public void Delete_Should_Wait_To_Remove_Item_If_Item_Exists_In_BatchMode(IRepository<Contact, string> repository)
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
        public void Delete_Should_Remove_Multiple_Items(IRepository<Contact, string> repository)
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

        [ExecuteForAllRepositories]
        public void Delete_Predicate_Should_Remove_Multiple_Items(IRepository<Contact, string> repository)
        {
            IList<Contact> contacts = new List<Contact>
                                        {
                                            new Contact {Name = "Contact 1", ContactTypeId = 1},
                                            new Contact {Name = "Contact 2", ContactTypeId = 1},
                                            new Contact {Name = "Contact 3", ContactTypeId = 3},
                                        };

            repository.Add(contacts);
            var items = repository.GetAll().ToList();
            items.Count().ShouldEqual(3);

            repository.Delete(x => x.ContactTypeId < 3);
            items = repository.GetAll().ToList();
            items.Count().ShouldEqual(1);
            items.First().Name.ShouldEqual("Contact 3");
        }

        [ExecuteForAllRepositories]
        public void Delete_By_Keys_Enum_Should_Remove_Multiple_Items(IRepository<Contact, string> repository)
        {
            var contact1 = new Contact {Name = "Contact 1", ContactTypeId = 1};
            var contact2 = new Contact {Name = "Contact 2", ContactTypeId = 1};
            var contact3 = new Contact {Name = "Contact 3", ContactTypeId = 3};

            repository.Add(contact1);
            repository.Add(contact2);
            repository.Add(contact3);

            var items = repository.GetAll().ToList();
            items.Count().ShouldEqual(3);

            var ids = new List<string>()
                      {
                          contact1.ContactId,
                          contact2.ContactId
                      };
            repository.Delete(ids);


            items = repository.GetAll().ToList();
            items.Count().ShouldEqual(1);
            items.First().Name.ShouldEqual("Contact 3");
        }

        [ExecuteForAllRepositories]
        public void Delete_By_Params_Should_Remove_Multiple_Items(IRepository<Contact, string> repository)
        {
            var contact1 = new Contact {Name = "Contact 1", ContactTypeId = 1};
            var contact2 = new Contact {Name = "Contact 2", ContactTypeId = 1};
            var contact3 = new Contact {Name = "Contact 3", ContactTypeId = 3};

            repository.Add(contact1);
            repository.Add(contact2);
            repository.Add(contact3);

            var items = repository.GetAll().ToList();
            items.Count().ShouldEqual(3);

            repository.Delete(contact1.ContactId, contact2.ContactId);

            items = repository.GetAll().ToList();
            items.Count().ShouldEqual(1);
            items.First().Name.ShouldEqual("Contact 3");
        }

    }
}