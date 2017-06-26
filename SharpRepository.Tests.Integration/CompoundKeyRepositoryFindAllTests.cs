using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class CompoundKeyRepositoryFindAllTests : TestBase
    {
        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Specification(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new User {Username = "Test User " + i, Age = i};
                repository.Add(contact);
            }

            var result = repository.FindAll(new Specification<User>(p => p.Username == "Test User 1")); 
            result.Count().ShouldBe(1);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Predicate(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new User { Username = "Test User " + i, Age = i };
                repository.Add(contact);
            }

            var result = repository.FindAll(p => p.Username == "Test User 1"); // Note: Raven doesn't like p.Name.Equals("...");
            result.Count().ShouldBe(1);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Specification_With_Paging(ICompoundKeyRepository<User, string, int> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            const int totalItems = 10;

            var queryOptions = new PagingOptions<User, string>(resultingPage, pageSize, m => m.Username);

            for (var i = 1; i <= totalItems; i++)
            {
                var contact = new User { Username = "Test User " + i, Age = i };
                repository.Add(contact);
            }

            var result = repository.FindAll(new Specification<User>(p => p.Age <= totalItems / 2), queryOptions);
            result.Count().ShouldBe(pageSize);
            queryOptions.TotalItems.ShouldBe(totalItems / 2);
            result.First().Username.ShouldBe("Test User 3");
        }

        //[ExecuteForRepositories(RepositoryType.Xml,RepositoryType.InMemory, RepositoryType.Ef)]
        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Specification_With_Paging_MagicString(ICompoundKeyRepository<User, string, int> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            const int totalItems = 10;

            var queryOptions = new PagingOptions<User>(resultingPage, pageSize, "Username");

            for (var i = 1; i <= totalItems; i++)
            {
                var contact = new User { Username = "Test User " + i, Age = i };
                repository.Add(contact);
            }

            // this fails for RavenDb because the ContactId is an int but is being used as the key, so the check on ContactId <= 5 is doing a string comparison and including ContactId = 10 as well
            //  need to look into why this happens and how to get around it
            var result = repository.FindAll(new Specification<User>(p => p.Age <= totalItems / 2), queryOptions);
            result.Count().ShouldBe(pageSize);
            queryOptions.TotalItems.ShouldBe(totalItems / 2);
            result.First().Username.ShouldBe("Test User 3");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new User { Username = "Test User " + i, Age = i };
                repository.Add(contact);
            }

            var result = repository.FindAll(new Specification<User>(p => p.Username == "Test User 1").OrElse(new Specification<User>(p => p.Username == "Test User 2")));
            result.Count().ShouldBe(2);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification_With_Paging(ICompoundKeyRepository<User, string, int> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            var queryOptions = new PagingOptions<User>(resultingPage, pageSize, "Username");

            for (var i = 1; i <= 10; i++)
            {
                var contact = new User { Username = "Test User " + i, Age = i };
                repository.Add(contact);
            }

            IEnumerable<User> result = repository
                .FindAll(new Specification<User>(p => p.Username == "Test User 1")
                                .OrElse(new Specification<User>(p => p.Username == "Test User 5")
                                        .OrElse(new Specification<User>(p => p.Username == "Test User 8"))),
                            queryOptions);

            result.Count().ShouldBe(1);
            queryOptions.TotalItems.ShouldBe(3);
            result.First().Username.ShouldBe("Test User 8");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification_With_Paging_And_Sort_Descending(ICompoundKeyRepository<User, string, int> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            var queryOptions = new PagingOptions<User>(resultingPage, pageSize, "Username", true);

            for (var i = 1; i <= 10; i++)
            {
                var contact = new User { Username = "Test User " + i, Age = i };
                repository.Add(contact);
            }

            IEnumerable<User> result = repository
                .FindAll(new Specification<User>(p => p.Username == "Test User 1")
                                .OrElse(new Specification<User>(p => p.Username == "Test User 5")
                                        .OrElse(new Specification<User>(p => p.Username == "Test User 8"))),
                            queryOptions);

            result.Count().ShouldBe(1);
            queryOptions.TotalItems.ShouldBe(3);
            result.First().Username.ShouldBe("Test User 1");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification_With_Paging_MagicString(ICompoundKeyRepository<User, string, int> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            var queryOptions = new PagingOptions<User>(resultingPage, pageSize, "Username");

            for (var i = 1; i <= 10; i++)
            {
                var contact = new User { Username = "Test User " + i, Age = i };
                repository.Add(contact);
            }

            IEnumerable<User> result = repository
                .FindAll(new Specification<User>(p => p.Username == "Test User 1")
                                .OrElse(new Specification<User>(p => p.Username == "Test User 5")
                                        .OrElse(new Specification<User>(p => p.Username == "Test User 8"))),
                            queryOptions);

            result.Count().ShouldBe(1);
            queryOptions.TotalItems.ShouldBe(3);
            result.First().Username.ShouldBe("Test User 8");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification_With_Paging_MagicString_And_Sort_Descending(ICompoundKeyRepository<User, string, int> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            var queryOptions = new PagingOptions<User>(resultingPage, pageSize, "Username", true);

            for (var i = 1; i <= 10; i++)
            {
                var contact = new User { Username = "Test User " + i, Age = i };
                repository.Add(contact);
            }

            IEnumerable<User> result = repository
                .FindAll(new Specification<User>(p => p.Username == "Test User 1")
                                .OrElse(new Specification<User>(p => p.Username == "Test User 5")
                                        .OrElse(new Specification<User>(p => p.Username == "Test User 8"))),
                            queryOptions);

            result.Count().ShouldBe(1);
            queryOptions.TotalItems.ShouldBe(3);
            result.First().Username.ShouldBe("Test User 1");
        }
    }
}