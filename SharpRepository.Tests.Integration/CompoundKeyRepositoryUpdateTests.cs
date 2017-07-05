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
    public class CompoundKeyRepositoryUpdateTests : TestBase
    {
        [ExecuteForAllCompoundKeyRepositories]
        public void Update_Should_Save_Modified_Business_Name(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User { Username = "Test User", Age = 22, FullName = "Test User - 22" };
            repository.Add(item);

            var item2 = new User { Username = "Test User 2", Age = 22, FullName = "Test User 2 - 22" };
            repository.Add(item2);

            item.FullName = "Test User - 22 - Updated";
            repository.Update(item);

            var updated = repository.Get(item.Username, item.Age);
            var notUpdated = repository.Get(item2.Username, item2.Age);

            updated.FullName.ShouldBe("Test User - 22 - Updated");
            notUpdated.FullName.ShouldBe("Test User 2 - 22");
        }
        
        [ExecuteForAllCompoundKeyRepositories]
        public void Update_Should_Update_Multiple_Items(ICompoundKeyRepository<User, string, int> repository)
        {
            var users = new List<User>
            {
                new User { Username = "Test User", Age = 11, FullName = "Test User - 11" },
                new User { Username = "Test User", Age = 21, FullName = "Test User - 21" },
                new User { Username = "Test User 2", Age = 11, FullName = "Test User  2- 11" },
            };

            repository.Add(users);
            var items = repository.GetAll().ToList();
            items.Count().ShouldBe(3);

            foreach (var user in users.Take(2))
            {
                user.FullName += "UPDATED";
            }

            repository.Update(users);
            items = repository.GetAll().ToList();
            items.Count(x => x.FullName.EndsWith("UPDATED")).ShouldBe(2);
        }
    }
}