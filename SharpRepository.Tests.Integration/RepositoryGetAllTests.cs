using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using SharpRepository.EfRepository;
using System.Collections.Generic;
using System.Linq;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryGetAllTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void GetAll_Should_Return_Every_Item(IRepository<Contact, string> repository)
        {
            for (int i = 1; i <= 5; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            IEnumerable<Contact> result = repository.GetAll().ToList();
            result.Count().ShouldBe(5);
        }

        [ExecuteForAllRepositories]
        public void GetAll_Should_Return_Every_Items_With_Paging(IRepository<Contact, string> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            const int totalItems = 5;

            var queryOptions = new PagingOptions<Contact>(resultingPage, pageSize, "Name");

            for (int i = 1; i <= totalItems; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            IEnumerable<Contact> result = repository.GetAll(queryOptions).ToList();
            result.Count().ShouldBe(pageSize);
            queryOptions.TotalItems.ShouldBe(totalItems);
            result.First().Name.ShouldBe("Test User 3");
        }

        [ExecuteForAllRepositories]
        public void GetAll_With_Selector_Should_Return_Every_Item(IRepository<Contact, string> repository)
        {
            for (int i = 1; i <= 5; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            var result = repository.GetAll(c => c.Name);
            result.Count().ShouldBe(5);
        }

        [ExecuteForAllRepositories]
        public void GetAll_With_Anonymous_Selector_Should_Return_Every_Item(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            var results = repository.GetAll(c => new {c.Name, c.Title});

            var total = 0;
            foreach (var result in results)
            {
                result.Name.ShouldStartWith("Test User");
                total++;
            }

            total.ShouldBe(5);
        }

        [ExecuteForAllRepositories]
        public void GetAll_With_Selector_Should_Return_Every_Items_With_Paging(IRepository<Contact, string> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            const int totalItems = 5;

            var queryOptions = new PagingOptions<Contact>(resultingPage, pageSize, "Name");

            for (int i = 1; i <= totalItems; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            var result = repository.GetAll(c => c.Name, queryOptions).ToList();
            result.Count().ShouldBe(pageSize);
            queryOptions.TotalItems.ShouldBe(totalItems);
            result.First().ShouldBe("Test User 3");
        }
    }
}