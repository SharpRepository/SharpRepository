using System;
using System.Linq;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class EfCoreCompoundKeySpikes
    {
        private DbContext context;

        [SetUp]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseSqlite(connection)
                .Options;
            
            // Run the test against one instance of the context
            context = new TestObjectContextCore(options);
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            context = null;
        }

        [Test]
        public void CompoundKeyRepository_Should_Work()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            ICompoundKeyRepository<User, string, int> repository = new EfCoreRepository<User, string, int>(context);

            repository.Add(new User { Username = "jeff", Age = 21, FullName = "Jeff - 21" });
            repository.Add(new User { Username = "jeff", Age = 31, FullName = "Jeff - 31" });
            repository.Add(new User { Username = "jeff", Age = 41, FullName = "Jeff - 41" });

            repository.Add(new User { Username = "ben", Age = 31, FullName = "Ben - 31" });
            repository.Add(new User { Username = "ben", Age = 41, FullName = "Ben - 41" });
            repository.Add(new User { Username = "ben", Age = 51, FullName = "Ben - 51" });

            repository.Get("jeff", 31).FullName.ShouldBe("Jeff - 31");
            repository.Get("ben", 31).FullName.ShouldBe("Ben - 31");
            repository.Get("jeff", 41).FullName.ShouldBe("Jeff - 41");

            repository.FindAll(x => x.Age == 31).Count().ShouldBe(2);
        }

        [Test]
        public void CompoundKeyRepositoryNoGenerics_Should_Work()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            ICompoundKeyRepository<User> repository = new EfCoreCompoundKeyRepository<User>(context);

            repository.Add(new User { Username = "jeff", Age = 21, FullName = "Jeff - 21" });
            repository.Add(new User { Username = "jeff", Age = 31, FullName = "Jeff - 31" });
            repository.Add(new User { Username = "jeff", Age = 41, FullName = "Jeff - 41" });

            repository.Add(new User { Username = "ben", Age = 31, FullName = "Ben - 31" });
            repository.Add(new User { Username = "ben", Age = 41, FullName = "Ben - 41" });
            repository.Add(new User { Username = "ben", Age = 51, FullName = "Ben - 51" });

            repository.Get("jeff", 31).FullName.ShouldBe("Jeff - 31");
            repository.Get("ben", 31).FullName.ShouldBe("Ben - 31");
            repository.Get("jeff", 41).FullName.ShouldBe("Jeff - 41");

            repository.FindAll(x => x.Age == 31).Count().ShouldBe(2);
        }
    }
}
