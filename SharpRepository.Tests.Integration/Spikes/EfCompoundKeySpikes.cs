using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using SharpRepository.EfRepository;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class EfCompoundKeySpikes
    {
         

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void CompoundKeyRepository_Should_Work()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            ICompoundKeyRepository<User, string, int> repository = new EfRepository<User, string, int>(new TestObjectEntities("Data Source=" + dbPath));

            repository.Add(new User { Username = "jeff", Age = 21, FullName = "Jeff - 21" });
            repository.Add(new User { Username = "jeff", Age = 31, FullName = "Jeff - 31" });
            repository.Add(new User { Username = "jeff", Age = 41, FullName = "Jeff - 41" });

            repository.Add(new User { Username = "ben", Age = 31, FullName = "Ben - 31" });
            repository.Add(new User { Username = "ben", Age = 41, FullName = "Ben - 41" });
            repository.Add(new User { Username = "ben", Age = 51, FullName = "Ben - 51" });

            repository.Get("jeff", 31).FullName.ShouldEqual("Jeff - 31");
            repository.Get("ben", 31).FullName.ShouldEqual("Ben - 31");
            repository.Get("jeff", 41).FullName.ShouldEqual("Jeff - 41");

            repository.FindAll(x => x.Age == 31).Count().ShouldEqual(2);
        }

        [Test]
        public void CompoundKeyRepositoryNoGenerics_Should_Work()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            ICompoundKeyRepository<User> repository = new EfCompoundKeyRepository<User>(new TestObjectEntities("Data Source=" + dbPath));

            repository.Add(new User { Username = "jeff", Age = 21, FullName = "Jeff - 21" });
            repository.Add(new User { Username = "jeff", Age = 31, FullName = "Jeff - 31" });
            repository.Add(new User { Username = "jeff", Age = 41, FullName = "Jeff - 41" });

            repository.Add(new User { Username = "ben", Age = 31, FullName = "Ben - 31" });
            repository.Add(new User { Username = "ben", Age = 41, FullName = "Ben - 41" });
            repository.Add(new User { Username = "ben", Age = 51, FullName = "Ben - 51" });

            repository.Get("jeff", 31).FullName.ShouldEqual("Jeff - 31");
            repository.Get("ben", 31).FullName.ShouldEqual("Ben - 31");
            repository.Get("jeff", 41).FullName.ShouldEqual("Jeff - 41");

            repository.FindAll(x => x.Age == 31).Count().ShouldEqual(2);
        }
    }
}
