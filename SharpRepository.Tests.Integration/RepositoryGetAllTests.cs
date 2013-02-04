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
        public void GetAll_Should_Return_Every_Item(IRepository<Contact, string> repository)
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
            result.Count().ShouldEqual(pageSize);
            queryOptions.TotalItems.ShouldEqual(totalItems);
            result.First().Name.ShouldEqual("Test User 3");
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
            result.Count().ShouldEqual(5);
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

            total.ShouldEqual(5);
        }

        // For some unkown reason ReSharper (at least I believe it is ReSharper) is turning the string below red and saying it is an unrecognized symbol  when it is a single string, but offers to Split string and that makes the red highlight and error go away
        //  even though it will compile and run when it is marked as a red error
        [ExecuteForAllRepositoriesExcept("NHibnernate " + "Linq does not support Pagination with selector.  It throws an Unsupported exception when calling Select() on the IQueryable returned by calling query.Take().Skip()", 
            RepositoryTypes.NHibernate)]
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
            result.Count().ShouldEqual(pageSize);
            queryOptions.TotalItems.ShouldEqual(totalItems);
            result.First().ShouldEqual("Test User 3");
        }
    }
}