using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using SharpRepository.EfRepository;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class EfLazyLoadSpike
    {
        [Test]
        public void LazyLoad_Set_To_False()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");

            var dbContext = new TestObjectEntities("Data Source=" + dbPath);
            dbContext.Configuration.LazyLoadingEnabled = false;

            var repos = new MyEfRepository(dbContext);

            repos.LazyLoadValue.ShouldBeFalse();
        }
    }

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
}
