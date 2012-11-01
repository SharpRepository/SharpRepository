using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryFindAllTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Specification(IRepository<Contact, string> repository)
        {
            for (int i = 1; i <= 3; i++)
            {
                var contact = new Contact {Name = "Test User " + i};
                repository.Add(contact);
            }

            var result = repository.FindAll(new Specification<Contact>(p => p.Name == "Test User 1")); // Note: Raven doesn't like p.Name.Equals("...");
            result.Count().ShouldEqual(1);
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Specification_With_Paging(IRepository<Contact, string> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            const int totalItems = 10;

            var queryOptions = new PagingOptions<Contact, string>(resultingPage, pageSize, m => m.Name);

            for (int i = 1; i <= totalItems; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i};
                repository.Add(contact);
            }

            // this fails for RavenDb because the ContactId is an int but is being used as the key, so the check on ContactId <= 5 is doing a string comparison and including ContactId = 10 as well
            //  need to look into why this happens and how to get around it
            var result = repository.FindAll(new Specification<Contact>(p => p.ContactTypeId <= totalItems / 2), queryOptions);
            result.Count().ShouldEqual(pageSize);
            queryOptions.TotalItems.ShouldEqual(totalItems / 2);
            result.First().Name.ShouldEqual("Test User 3");
        }

        //[ExecuteForRepositories(RepositoryTypes.Xml,RepositoryTypes.InMemory, RepositoryTypes.Ef)]
        [ExecuteForAllRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Specification_With_Paging_MagicString(IRepository<Contact, string> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            const int totalItems = 10;

            var queryOptions = new PagingOptions<Contact>(resultingPage, pageSize, "Name");

            for (int i = 1; i <= totalItems; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i };
                repository.Add(contact);
            }

            // this fails for RavenDb because the ContactId is an int but is being used as the key, so the check on ContactId <= 5 is doing a string comparison and including ContactId = 10 as well
            //  need to look into why this happens and how to get around it
            var result = repository.FindAll(new Specification<Contact>(p => p.ContactTypeId <= totalItems / 2), queryOptions);
            result.Count().ShouldEqual(pageSize);
            queryOptions.TotalItems.ShouldEqual(totalItems / 2);
            result.First().Name.ShouldEqual("Test User 3");
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification(IRepository<Contact, string> repository)
        {
            for (int i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            var result = repository.FindAll(new Specification<Contact>(p => p.Name == "Test User 1").OrElse(new Specification<Contact>(p => p.Name == "Test User 2")));
            result.Count().ShouldEqual(2);
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification_With_Paging(IRepository<Contact, string> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            var queryOptions = new PagingOptions<Contact>(resultingPage, pageSize, "Name");

            for (int i = 1; i <= 10; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            IEnumerable<Contact> result = repository
                .FindAll(new Specification<Contact>(p => p.Name == "Test User 1")
                                .OrElse(new Specification<Contact>(p => p.Name == "Test User 5")
                                        .OrElse(new Specification<Contact>(p => p.Name == "Test User 8"))),
                            queryOptions);

            result.Count().ShouldEqual(1);
            queryOptions.TotalItems.ShouldEqual(3);
            result.First().Name.ShouldEqual("Test User 8");
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification_With_Paging_And_Sort_Descending(IRepository<Contact, string> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            var queryOptions = new PagingOptions<Contact>(resultingPage, pageSize, "Name", true);

            for (int i = 1; i <= 10; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            IEnumerable<Contact> result = repository
                .FindAll(new Specification<Contact>(p => p.Name == "Test User 1")
                                .OrElse(new Specification<Contact>(p => p.Name == "Test User 5")
                                        .OrElse(new Specification<Contact>(p => p.Name == "Test User 8"))),
                            queryOptions);

            result.Count().ShouldEqual(1);
            queryOptions.TotalItems.ShouldEqual(3);
            result.First().Name.ShouldEqual("Test User 1");
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification_With_Paging_MagicString(IRepository<Contact, string> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            var queryOptions = new PagingOptions<Contact>(resultingPage, pageSize, "Name");

            for (int i = 1; i <= 10; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            IEnumerable<Contact> result = repository
                .FindAll(new Specification<Contact>(p => p.Name == "Test User 1")
                                .OrElse(new Specification<Contact>(p => p.Name == "Test User 5")
                                        .OrElse(new Specification<Contact>(p => p.Name == "Test User 8"))),
                            queryOptions);

            result.Count().ShouldEqual(1);
            queryOptions.TotalItems.ShouldEqual(3);
            result.First().Name.ShouldEqual("Test User 8");
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Return_All_Items_Which_Satisfy_Composite_Specification_With_Paging_MagicString_And_Sort_Descending(IRepository<Contact, string> repository)
        {
            const int resultingPage = 2;
            const int pageSize = 2;
            var queryOptions = new PagingOptions<Contact>(resultingPage, pageSize, "Name", true);

            for (int i = 1; i <= 10; i++)
            {
                var contact = new Contact { Name = "Test User " + i };
                repository.Add(contact);
            }

            IEnumerable<Contact> result = repository
                .FindAll(new Specification<Contact>(p => p.Name == "Test User 1")
                                .OrElse(new Specification<Contact>(p => p.Name == "Test User 5")
                                        .OrElse(new Specification<Contact>(p => p.Name == "Test User 8"))),
                            queryOptions);

            result.Count().ShouldEqual(1);
            queryOptions.TotalItems.ShouldEqual(3);
            result.First().Name.ShouldEqual("Test User 1");
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Match_On_Boolean(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 10; i++)
            {
                var contact = new Contact { Name = "Test User " + i , IsActive = i % 2 == 0};
                repository.Add(contact);
            }

            var results = repository.FindAll(x => x.IsActive);
            results.Count().ShouldEqual(5);
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Match_On_DateTime(IRepository<Contact, string> repository)
        {
            var startDate = new DateTime(2012, 1, 1);
            for (var i = 1; i <= 10; i++)
            {
                var contact = new Contact { Name = "Test User " + i , CreatedDate = startDate.AddMonths(i-1)};
                repository.Add(contact);
            }

            var results = repository.FindAll(x => x.CreatedDate < startDate.AddMonths(5));
            results.Count().ShouldEqual(5);
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Match_On_Any(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 10; i++)
            {
                var contact = new Contact { Name = "Test User " + i, EmailAddresses = new List<EmailAddress>() { new EmailAddress {Label = i % 2 == 0 ? "Home" : "Work"}}};
                repository.Add(contact);
            }

            // need to implement VisitSubQueryMethod
            var results = repository.FindAll(x => x.EmailAddresses.Any(m => m.Label == "Home"));
            results.Count().ShouldEqual(5);
        }

        [ExecuteForAllRepositories]
        public void FindAll_Should_Match_On_StartsWith(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 10; i++)
            {
                var contact = new Contact { Name = "Test User " + i, Title = (i % 2 == 0 ? "One " : "Two ") + i };
                repository.Add(contact);
            }

            var results = repository.FindAll(x => x.Title.StartsWith("One"));
            results.Count().ShouldEqual(5);
        }
    }
}