using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class CompoundKeyRepositoryDeleteTests : TestBase
    {
        [ExecuteForAllCompoundKeyRepositories]
        public void Delete_Should_Remove_Item(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User {Username = "Test User", Age = 11, FullName = "Test User - 11"};
            repository.Add(item);

            var result = repository.Get("Test User", 11);
            result.ShouldNotBeNull();

            repository.Delete(item);
            result = repository.Get("Test User", 11);
            result.ShouldBeNull();
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Delete_Should_Remove_Item_By_Key(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User { Username = "Test User", Age = 11, FullName = "Test User - 11" };
            repository.Add(item);

            var result = repository.Get(item.Username, item.Age);
            result.ShouldNotBeNull();

            repository.Delete(item.Username, item.Age);

            result = repository.Get(item.Username, item.Age);
            result.ShouldBeNull();
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Delete_Should_Wait_To_Remove_Item_If_Item_Exists_In_BatchMode(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User { Username = "Test User", Age = 11, FullName = "Test User - 11" };
            repository.Add(item);

            var result = repository.Get(item.Username, item.Age);
            result.ShouldNotBeNull();
            
            using (var batch = repository.BeginBatch())
            {
                batch.Delete(item);// not really delete until call Save, because in BatchMode

                result = repository.Get(item.Username, item.Age);
                result.ShouldNotBeNull();

                batch.Commit(); // actually do the delete
            }

            result = repository.Get(item.Username, item.Age);
            result.ShouldBeNull();
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Delete_Should_Remove_Multiple_Items(ICompoundKeyRepository<User, string, int> repository)
        {
            IList<User> users = new List<User>
            {
                new User { Username = "Test User", Age = 11, FullName = "Test User - 11" },
                new User { Username = "Test User", Age = 21, FullName = "Test User - 21" },
                new User { Username = "Test User 2", Age = 11, FullName = "Test User  2- 11" },
            };

            repository.Add(users);
            var items = repository.GetAll().ToList();
            items.Count().ShouldBe(3);

            repository.Delete(users.Take(2));
            items = repository.GetAll().ToList();
            items.Count().ShouldBe(1);
            items.First().Username.ShouldBe("Test User 2");
        }
    }
}