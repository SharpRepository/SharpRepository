using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using SharpRepository.InMemoryRepository;
using SharpRepository.EfCoreRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.MongoDbRepository;
using System;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class CompoundKeyRepositoryJoinTests : TestBase
    {

        [Test]
        public void Join_GetAll_Should_Return_All_Items_InMemory()
        {
            var inMemoryRepository = new InMemoryRepository<User, string, int>();
            Join_GetAll_Should_Return_All_Items(inMemoryRepository);
        }
        
        [Test]
        public void Join_GetAll_Should_Return_All_Items_EfCore()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                    .UseSqlite(connection)
                    .Options;

            // Create the schema in the database
            var context = new TestObjectContextCore(options);
            context.Database.EnsureCreated();
            var efCoreRepository = new EfCoreRepository<User, string, int>(context);
            Join_GetAll_Should_Return_All_Items(efCoreRepository);
        }

        public void Join_GetAll_Should_Return_All_Items(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                var item = new User { Username = "Test User " + i.ToString(), Age = (i % 2) + 1, ContactTypeId = i == 1 ? 1 : 2 };
                repository.Add(item);
            }

            var contactTypeRepository = new InMemoryRepository<ContactType, int>();
            contactTypeRepository.Add(new ContactType() { ContactTypeId = 1, Abbreviation = "T1" });
            contactTypeRepository.Add(new ContactType() { ContactTypeId = 2, Abbreviation = "T2" });

            var compositeRepos = repository.Join(contactTypeRepository, c => c.ContactTypeId, ct => ct.ContactTypeId,
                            (c, ct) => new { Name = c.Username, TypeAbbrev = ct.Abbreviation });

            var all = compositeRepos.GetAll().ToList();

            all.Count.ShouldBe(5);

            all.Single(c => c.Name == "Test User 1").TypeAbbrev.ShouldBe("T1");
            all.Single(c => c.Name == "Test User 2").TypeAbbrev.ShouldBe("T2");
            all.Single(c => c.Name == "Test User 3").TypeAbbrev.ShouldBe("T2");
            all.Single(c => c.Name == "Test User 4").TypeAbbrev.ShouldBe("T2");
            all.Single(c => c.Name == "Test User 5").TypeAbbrev.ShouldBe("T2");            
        }
    }
}
