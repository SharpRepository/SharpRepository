using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryGetTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void Get_Should_Return_Item_If_Item_Exists(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User", ContactTypeId = 1 };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId);
            result.Name.ShouldBe(contact.Name);
            result.ContactTypeId.ShouldBe(contact.ContactTypeId);
        }

        [ExecuteForAllRepositories]
        public void Get_Should_Return_Null_If_Item_Does_Not_Exists(IRepository<Contact, string> repository)
        {
            var result = repository.Get(string.Empty);
            result.ShouldBeNull();
        }

        [ExecuteForAllRepositories]
        public void Get_With_String_Selector_Should_Return_Item_If_Item_Exists(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId, c => c.Name);
            result.ShouldBe("Test User");
        }

        [ExecuteForAllRepositories]
        public void Get_With_Int_Selector_Should_Return_Item_If_Item_Exists(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User", ContactTypeId = 2 };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId, c => c.ContactTypeId);
            result.ShouldBe(2);
        }

        [ExecuteForAllRepositories]
        public void Get_With_Anonymous_Class_Selector_Should_Return_Item_If_Item_Exists(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User", ContactTypeId = 2 };
            repository.Add(contact);

            var result = repository.Get(contact.ContactId, c => new { c.ContactTypeId, c.Name });
            result.ContactTypeId.ShouldBe(2);
            result.Name.ShouldBe("Test User");
        }

        [ExecuteForAllRepositories]
        public void Get_With_String_Selector_Should_Return_Default_If_Item_Does_Not_Exists(IRepository<Contact, string> repository)
        {
            var result = repository.Get(string.Empty, c => c.Name);
            result.ShouldBe(default(string));
        }

        [ExecuteForAllRepositories]
        public void Get_With_Int_Selector_Should_Return_Default_If_Item_Does_Not_Exists(IRepository<Contact, string> repository)
        {
            var result = repository.Get(string.Empty, c => c.ContactTypeId);
            result.ShouldBe(default(int));
        }

        [ExecuteForAllRepositories]
        public void Get_With_Anonymouse_Class_Selector_Should_Return_Null_If_Item_Does_Not_Exists(IRepository<Contact, string> repository)
        {
            var result = repository.Get(string.Empty, c => new { c.ContactTypeId, c.Name });
            result.ShouldBeNull();
        }

        [ExecuteForAllRepositories]
        public void TryGet_Should_Return_True_And_Item_If_Item_Exists(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User", ContactTypeId = 1 };
            repository.Add(contact);

            repository.TryGet(contact.ContactId, out Contact result).ShouldBeTrue();
            result.Name.ShouldBe(contact.Name);
            result.ContactTypeId.ShouldBe(contact.ContactTypeId);
        }

        [ExecuteForAllRepositories]
        public void TryGet_Should_Return_False_And_Null_If_Item_Does_Not_Exists(IRepository<Contact, string> repository)
        {
            repository.TryGet(string.Empty, out Contact result).ShouldBeFalse();
            result.ShouldBeNull();
        }

        [ExecuteForAllRepositories]
        public void TryGet_Should_Return_True_If_Item_Exists(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User", ContactTypeId = 1 };
            repository.Add(contact);

            repository.Exists(contact.ContactId).ShouldBeTrue();
        }

        [ExecuteForAllRepositories]
        public void TryGet_Should_Return_False_If_Item_Does_Not_Exists(IRepository<Contact, string> repository)
        {
            repository.Exists(string.Empty).ShouldBeFalse();
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.MongoDb, Reason = "ContactId is the ObjectId, must be a 24 hex string")]
        public void GetMany_Params_Should_Return_Multiple_Items(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                var contact = new Contact { ContactId = i.ToString(), Name = "Test User " + i};
                repository.Add(contact);
            }

            var items = repository.GetMany("1", "3", "4", "5");
            items.Count().ShouldBe(4);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.MongoDb, Reason = "ContactId is the ObjectId, must be a 24 hex string")]
        public void GetMany_List_Should_Return_Multiple_Items(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                var contact = new Contact { ContactId = i.ToString(), Name = "Test User " + i};
                repository.Add(contact);
            }

            var items = repository.GetMany(new [] {"1", "3", "4", "5" }.ToList());
            items.Count().ShouldBe(4);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.MongoDb, Reason = "ContactId is the ObjectId, must be a 24 hex string")]
        public void GetManyAsDictionary_Params_Should_Return_Multiple_Items(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                var contact = new Contact { ContactId = i.ToString(), Name = "Test User " + i};
                repository.Add(contact);
            }

            var items = repository.GetManyAsDictionary("1", "3", "4", "5");
            items.Count().ShouldBe(4);
            items.ContainsKey("1").ShouldBeTrue();
            items.ContainsKey("2").ShouldBeFalse();
            items.ContainsKey("3").ShouldBeTrue();
            items.ContainsKey("4").ShouldBeTrue();
            items.ContainsKey("5").ShouldBeTrue();
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.MongoDb, Reason = "ContactId is the ObjectId, must be a 24 hex string")]
        public void GetManyAsDictionary_List_Should_Return_Multiple_Items(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                var contact = new Contact { ContactId = i.ToString(), Name = "Test User " + i};
                repository.Add(contact);
            }

            var items = repository.GetManyAsDictionary(new [] {"1", "3", "4", "5" }.ToList());
            items.ContainsKey("1").ShouldBeTrue();
            items.ContainsKey("2").ShouldBeFalse();
            items.ContainsKey("3").ShouldBeTrue();
            items.ContainsKey("4").ShouldBeTrue();
            items.ContainsKey("5").ShouldBeTrue();
        }
    }
}