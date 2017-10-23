using System;
using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using SharpRepository.EfRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using System.Collections.Generic;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using Shouldly;

namespace SharpRepository.Tests.Integration.Spikes
{
    public class MyEfRepository : EfRepository<Contact, string>
    {
        public MyEfRepository(DbContext dbContext, ICachingStrategy<Contact, string> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }

        public bool LazyLoadValue
        {
            get { return Context.Configuration.LazyLoadingEnabled; }
        }
    }

    [TestFixture]
    public class EfLazyLoadSpike
    {
        private TestObjectContext dbContext;
        private List<string> queries;

        [SetUp]
        public void SetupRepository()
        {
            queries = new List<string>();
            var dbPath = EfDataDirectoryFactory.Build();
            dbContext = new TestObjectContext("Data Source=" + dbPath);

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

            // reistantiate in order to lose caches
            dbContext = new TestObjectContext("Data Source=" + dbPath);
        }

        [Test]
        public void LazyLoad_Set_To_False()
        {
            dbContext.Configuration.LazyLoadingEnabled = false;
            var repository = new MyEfRepository(dbContext);
            repository.LazyLoadValue.ShouldBeFalse();
        }

        [Test]
        public void GetAll_Without_Includes_LazyLoads_Email()
        {
            dbContext.Configuration.LazyLoadingEnabled = true;
            dbContext.Database.Log = sql =>
            {
                if (sql.Contains("SELECT"))
                {
                    queries.Add(sql);
                }
            };
            var repository = new MyEfRepository(dbContext);

            var contact = repository.GetAll().First();
            contact.Name.ShouldBe("Test User 1");
            queries.Count().ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            queries.Count().ShouldBe(2);
        }

        [Test]
        public void GetAll_With_Includes_In_Strategy_LazyLoads_Email()
        {
            dbContext.Configuration.LazyLoadingEnabled = true;
            dbContext.Database.Log = sql =>
            {
                if (sql.Contains("SELECT"))
                {
                    queries.Add(sql);
                }
            };
            var repository = new MyEfRepository(dbContext);


            var strategy = new GenericFetchStrategy<Contact>();
            strategy.Include(x => x.EmailAddresses);

            var contact = repository.GetAll(strategy).First();
            contact.Name.ShouldBe("Test User 1");
            queries.Count().ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            queries.Count().ShouldBe(1);
        }

        [Test]
        public void GetAll_With_Includes_In_Strategy_String_LazyLoads_Email()
        {
            dbContext.Configuration.LazyLoadingEnabled = true;
            dbContext.Database.Log = sql =>
            {
                if (sql.Contains("SELECT"))
                {
                    queries.Add(sql);
                }
            };
            var repository = new MyEfRepository(dbContext);


            var strategy = new GenericFetchStrategy<Contact>();
            strategy.Include(x => x.EmailAddresses);

            var contact = repository.GetAll(strategy).First();
            contact.Name.ShouldBe("Test User 1");
            queries.Count().ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            queries.Count().ShouldBe(1);
        }

        [Test]
        public void GetAll_With_Text_Include_LazyLoads_Email()
        {
            dbContext.Configuration.LazyLoadingEnabled = true;
            dbContext.Database.Log = sql =>
            {
                if (sql.Contains("SELECT"))
                {
                    queries.Add(sql);
                }
            };
            var repository = new MyEfRepository(dbContext);

            var contact = repository.GetAll("EmailAddresses").First();
            contact.Name.ShouldBe("Test User 1");
            queries.Count().ShouldBe(1);
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            queries.Count().ShouldBe(1);
        }

        [Test]
        public void GetAll_With_Text_Include_And_Pagination_LazyLoads_Email()
        {
            dbContext.Configuration.LazyLoadingEnabled = true;
            dbContext.Database.Log = sql =>
            {
                if (sql.Contains("SELECT"))
                {
                    queries.Add(sql);
                }
            };
            var repository = new MyEfRepository(dbContext);

            var pagination = new PagingOptions<Contact>(1, 4, "ContactId");

            var contact = repository.GetAll(pagination, "EmailAddresses").First();
            contact.Name.ShouldBe("Test User 1");
            queries.Count().ShouldBe(2); // first query is count for total records
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            queries.Count().ShouldBe(2);
        }

        [Test]
        public void FindAll_With_Include_And_Predicate_In_Specs_LazyLoads_Email()
        {
            dbContext.Configuration.LazyLoadingEnabled = true;
            dbContext.Database.Log = sql =>
            {
                if (sql.Contains("SELECT"))
                {
                    queries.Add(sql);
                }
            };
            var repository = new MyEfRepository(dbContext);

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
            queries.Count().ShouldBe(1); // first query is count for total records
            contact.EmailAddresses.First().Email.ShouldBe("omar.piani.1@email.com");
            queries.Count().ShouldBe(1);

            repository.FindAll(findAllBySpec).Count().ShouldBe(1);
        }
    }
}