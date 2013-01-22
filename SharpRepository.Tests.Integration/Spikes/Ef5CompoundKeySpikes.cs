using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Ef5Repository;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class Ef5CompoundKeySpikes
    {
         ICompoundKeyRepository<User, string, int> _repository;

        [SetUp]
        public void Setup()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            _repository = new Ef5Repository<User, string, int>(new TestObjectEntities("Data Source=" + dbPath));
        }

        [Test]
        public void CompoundKeyRepository_Should_Work()
        {
            _repository.Add(new User { Username = "jeff", Age = 21, FullName = "Jeff - 21" });
            _repository.Add(new User { Username = "jeff", Age = 31, FullName = "Jeff - 31" });
            _repository.Add(new User { Username = "jeff", Age = 41, FullName = "Jeff - 41" });

            _repository.Add(new User { Username = "ben", Age = 31, FullName = "Ben - 31" });
            _repository.Add(new User { Username = "ben", Age = 41, FullName = "Ben - 41" });
            _repository.Add(new User { Username = "ben", Age = 51, FullName = "Ben - 51" });

            _repository.Get("jeff", 31).FullName.ShouldEqual("Jeff - 31");
            _repository.Get("ben", 31).FullName.ShouldEqual("Ben - 31");
            _repository.Get("jeff", 41).FullName.ShouldEqual("Jeff - 41");

            _repository.FindAll(x => x.Age == 31).Count().ShouldEqual(2);
        }
    }
}
