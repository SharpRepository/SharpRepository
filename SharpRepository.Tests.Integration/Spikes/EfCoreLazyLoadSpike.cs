
using System.Linq;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using System.Collections.Generic;
using System.Diagnostics;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;

namespace SharpRepository.Tests.Integration.Spikes
{        
    [TestFixture]
    public class EfCoreLazyLoadSpike
    {
        private TestObjectContextCore dbContext;

        private Func<string, bool> filterSelects = q => q.StartsWith("Executing DbCommand") && q.Contains("SELECT") && !q.Contains("sqlite_master");

        [SetUp]
        public void SetupRepository()
        {
            var dbPath = EfDataDirectoryFactory.Build();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseSqlite(connection)
                .Options;
            
            // Create the schema in the database
            dbContext = new TestObjectContextCore(options);
            dbContext.Database.EnsureCreated();
            const int totalItems = 5;

            for (int i = 1; i <= totalItems; i++)
            {
                dbContext.Contacts.Add(
                    new Contact
                    {
                        ContactId = i.ToString(),
                        Name = "Test User " + i,
                        EmailAddresses = new List<EmailAddress> {
                            new EmailAddress {
                                ContactId = i.ToString(),
                                EmailAddressId = i,
                                Email = "omar.piani." + i.ToString() + "@email.com",
                                Label = "omar.piani." + i.ToString()
                            }
                        }
                    });
            }

            dbContext.SaveChanges();
        }
        
        [Test]
        public void EfCore_GetAll_Without_Includes_LazyLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var contact = repository.GetAll().First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            // dbContext.QueryLog.Count(filterSelects).ShouldBe(2); may be that dbcontext is disposed and the successive queries are not logged, quieries does not contains email so query was made in a lazy way but after.
        }

        [Test]
        public void EfCore_GetAll_With_Includes_In_Strategy_LazyLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);
            var strategy = new GenericFetchStrategy<Contact>();
            strategy.Include(x => x.EmailAddresses);

            var contact = repository.GetAll(strategy).First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);
        }

        [Test]
        public void EfCore_GetAll_With_Includes_In_Strategy_String_LazyLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);
            
            var strategy = new GenericFetchStrategy<Contact>();
            strategy.Include(x => x.EmailAddresses);

            var contact = repository.GetAll(strategy).First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);
        }

        [Test]
        public void EfCore_GetAll_With_Text_Include_LazyLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var contact = repository.GetAll("EmailAddresses").First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);
        }

        [Test]
        public void EfCore_GetAll_With_Text_Include_And_Pagination_LazyLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var pagination = new PagingOptions<Contact>(1, 4, "ContactId");

            var contact = repository.GetAll(pagination, "EmailAddresses").First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(3); // first query is count for total records
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(3);
        }

        [Test]
        public void EfCore_FindAll_With_Include_And_Predicate_In_Specs_LazyLoads_Email()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var findAllBySpec = new Specification<Contact>(obj => obj.ContactId == "1")
                    .And(obj => obj.EmailAddresses.Any(m => m.Email == "omar.piani.1@email.com"));

            var specification = new Specification<Contact>(obj => obj.Name == "Test User 1");

            findAllBySpec.FetchStrategy = new GenericFetchStrategy<Contact>();
            findAllBySpec.FetchStrategy
                .Include(obj => obj.EmailAddresses);

            // NOTE: This line will erase my FetchStrategy from above
            if (null != specification)
            {
                findAllBySpec = findAllBySpec.And(specification);
            }

            var contact = repository.FindAll(findAllBySpec).First();
            contact.Name.ShouldBe("Test User 1");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            dbContext.QueryLog.Count(filterSelects).ShouldBe(2);

            repository.FindAll(findAllBySpec).Count().ShouldBe(1);
        }
    }
}
