using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryGetAllTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void GetAll_Should_Return_Every_Item(IRepository<Contact, int> repository)
        {
            for (int i = 1; i <= 5; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            IEnumerable<Contact> result = repository.GetAll().ToList();
            result.Count().ShouldEqual(5);
        }

        [ExecuteForAllRepositories]
        public void GetAll_Should_Return_Every_Items_With_Paging(IRepository<Contact, int> repository)
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
            result.Count().ShouldEqual(pageSize);
            queryOptions.TotalItems.ShouldEqual(totalItems);
            result.First().Name.ShouldEqual("Test User 3");
        }

        [ExecuteForAllRepositories]
        public void GetAll_With_Selector_Should_Return_Every_Item(IRepository<Contact, int> repository)
        {
            for (int i = 1; i <= 5; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            var result = repository.GetAll(c => c.Name);
            result.Count().ShouldEqual(5);
        }

        [ExecuteForAllRepositories]
        public void GetAll_With_Selector_Should_Return_Every_Items_With_Paging(IRepository<Contact, int> repository)
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
            result.Count().ShouldEqual(pageSize);
            queryOptions.TotalItems.ShouldEqual(totalItems);
            result.First().ShouldEqual("Test User 3");
        }
    }
}