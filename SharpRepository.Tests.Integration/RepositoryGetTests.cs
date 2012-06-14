using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryGetTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void Get_Should_Return_Item_If_Item_Exists(IRepository<Contact, int> repository)
        {
            var contact = new Contact { Name = "Test User", ContactTypeId = 1 };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId);
            result.Name.ShouldEqual(contact.Name);
            result.ContactTypeId.ShouldEqual(contact.ContactTypeId);
        }

        [ExecuteForAllRepositories]
        public void Get_Should_Return_Null_If_Item_Does_Not_Exists(IRepository<Contact, int> repository)
        {
            var result = repository.Get(1);
            result.ShouldBeNull();
        }

        [ExecuteForAllRepositories]
        public void Get_With_String_Selector_Should_Return_Item_If_Item_Exists(IRepository<Contact, int> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId, c => c.Name);
            result.ShouldEqual("Test User");
        }

        [ExecuteForAllRepositories]
        public void Get_With_Int_Selector_Should_Return_Item_If_Item_Exists(IRepository<Contact, int> repository)
        {
            var contact = new Contact { Name = "Test User", ContactTypeId = 2 };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId, c => c.ContactTypeId);
            result.ShouldEqual(2);
        }

        [ExecuteForAllRepositories]
        public void Get_With_Anonymous_Class_Selector_Should_Return_Item_If_Item_Exists(IRepository<Contact, int> repository)
        {
            var contact = new Contact { Name = "Test User", ContactTypeId = 2 };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId, c => new { c.ContactTypeId, c.Name });
            result.ContactTypeId.ShouldEqual(2);
            result.Name.ShouldEqual("Test User");
        }

        [ExecuteForAllRepositories]
        public void Get_With_String_Selector_Should_Return_Default_If_Item_Does_Not_Exists(IRepository<Contact, int> repository)
        {
            var result = repository.Get(1, c => c.Name);
            result.ShouldEqual(default(string));
        }

        [ExecuteForAllRepositories]
        public void Get_With_Int_Selector_Should_Return_Default_If_Item_Does_Not_Exists(IRepository<Contact, int> repository)
        {
            var result = repository.Get(1, c => c.ContactTypeId);
            result.ShouldEqual(default(int));
        }

        [ExecuteForAllRepositories]
        public void Get_With_Anonymouse_Class_Selector_Should_Return_Null_If_Item_Does_Not_Exists(IRepository<Contact, int> repository)
        {
            var result = repository.Get(1, c => new { c.ContactTypeId, c.Name});
            result.ShouldBeNull();
        }
    }
}