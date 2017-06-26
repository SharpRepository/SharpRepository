using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class CompoundKeyRepositoryGetAllTests : TestBase
    {
        [ExecuteForAllCompoundKeyRepositories]
        public void GetAll_Should_Return_Every_Item(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.GetAll().ToList();
            result.Count().ShouldBe(5);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void GetAll_Should_Return_Every_Items_With_Paging(ICompoundKeyRepository<User, string, int> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            const int totalItems = 5;

            var queryOptions = new PagingOptions<User>(resultingPage, pageSize, "Username");

            for (var i = 1; i <= totalItems; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.GetAll(queryOptions).ToList();
            result.Count().ShouldBe(pageSize);
            queryOptions.TotalItems.ShouldBe(totalItems);
            result.First().Username.ShouldBe("Test User 3");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void GetAll_With_Selector_Should_Return_Every_Item(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.GetAll(c => c.Username);
            result.Count().ShouldBe(5);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void GetAll_With_Anonymous_Selector_Should_Return_Every_Item(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var results = repository.GetAll(c => new {c.Username, c.Age});

            var total = 0;
            foreach (var result in results)
            {
                result.Username.ShouldStartWith("Test User");
                total++;
            }

            total.ShouldBe(5);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void GetAll_With_Selector_Should_Return_Every_Items_With_Paging(ICompoundKeyRepository<User, string, int> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            const int totalItems = 5;

            var queryOptions = new PagingOptions<User>(resultingPage, pageSize, "Username");

            for (var i = 1; i <= totalItems; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.GetAll(c => c.Username, queryOptions).ToList();
            result.Count().ShouldBe(pageSize);
            queryOptions.TotalItems.ShouldBe(totalItems);
            result.First().ShouldBe("Test User 3");
        }
    }
}