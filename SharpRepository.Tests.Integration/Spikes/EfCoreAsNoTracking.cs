using System.Linq;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using System.Collections.Generic;
using Shouldly;
using SharpRepository.Repository.FetchStrategies;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class EfCoreAsNoTracking
    {
        private TestObjectContextCore dbContext;

        [SetUp]
        public void SetupRepository()
        {
            var dbPath = EfDataDirectoryFactory.Build();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                 .UseSqlite(connection)
                 .Options;

            using (dbContext = new TestObjectContextCore(options))
            {
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

            // reistantiate in order to lose caches
            dbContext = new TestObjectContextCore(options);
        }

        [Test]
        public void EfCoreGetAllSateShouldBeUnchanged()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var firstContact = repository.GetAll().First();

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Unchanged);
        }

        [Test]
        public void EfCoreGetAllWithStrategySateShouldBeUnchanged()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var strat = new GenericFetchStrategy<Contact>();

            var firstContact = repository.GetAll(strat).First();

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Unchanged);
        }

        [Test]
        public void EfCoreGetAllWithStrategyAsNoTrackingSateShouldBeUnchanged()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var strat = new GenericFetchStrategy<Contact>()
                .AsNoTracking();

            var firstContact = repository.GetAll(strat).First();

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Detached);
        }

        [Test]
        public void EfCoreGetSateShouldBeUnchanged()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var firstContact = repository.Get("1");

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Unchanged);
        }

        [Test]
        public void EfCoreGetWithStrategySateShouldBeUnchanged()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var strat = new GenericFetchStrategy<Contact>();

            var firstContact = repository.Get("1", strat);

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Unchanged);
        }

        [Test]
        public void EfCoreGetWithStrategyAsNoTrackingSateShouldBeUnchanged()
        {
            var repository = new EfCoreRepository<Contact, string>(dbContext);

            var strat = new GenericFetchStrategy<Contact>()
                .AsNoTracking();

            var firstContact = repository.Get("1", strat);

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Detached);
        }
    }
}