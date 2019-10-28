using System.Data.Entity;
using System.Linq;
using NUnit.Framework;
using SharpRepository.EfRepository;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using System.Collections.Generic;
using Shouldly;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class EfAsNoTracking
    {
        private TestObjectContext dbContext;

        [SetUp]
        public void SetupRepository()
        {
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
        public void GetAllSateShouldBeUnchanged()
        {
            var repository = new EfRepository<Contact, string>(dbContext);

            var firstContact = repository.GetAll().First();

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Unchanged);
        }

        [Test]
        public void GetAllWithStrategySateShouldBeUnchanged()
        {
            var repository = new EfRepository<Contact, string>(dbContext);

            var strat = new GenericFetchStrategy<Contact>();

            var firstContact = repository.GetAll(strat).First();

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Unchanged);
        }

        [Test]
        public void GetAllWithStrategyAsNoTrackingSateShouldBeUnchanged()
        {
            var repository = new EfRepository<Contact, string>(dbContext);

            var strat = new GenericFetchStrategy<Contact>()
                .AsNoTracking();

            var firstContact = repository.GetAll(strat).First();

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Detached);
        }

        [Test]
        public void GetSateShouldBeUnchanged()
        {
            var repository = new EfRepository<Contact, string>(dbContext);

            var firstContact = repository.Get("1");

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Unchanged);
        }

        [Test]
        public void GetWithStrategySateShouldBeUnchanged()
        {
            var repository = new EfRepository<Contact, string>(dbContext);

            var strat = new GenericFetchStrategy<Contact>();

            var firstContact = repository.Get("1", strat);

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Unchanged);
        }

        [Test]
        public void GetWithStrategyAsNoTrackingSateShouldBeUnchanged()
        {
            var repository = new EfRepository<Contact, string>(dbContext);

            var strat = new GenericFetchStrategy<Contact>()
                .AsNoTracking();

            var firstContact = repository.Get("1", strat);

            dbContext.Entry(firstContact).State.ShouldBe(EntityState.Detached);
        }
    }
}