using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using NUnit.Framework;
using SharpRepository.EfRepository;
using SharpRepository.Ioc.StructureMap;
using SharpRepository.Repository;
using SharpRepository.Repository.Ioc;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Should;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class RepositoryDependencySpikes
    {
        [SetUp]
        public void Setup()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");

            // structure map
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry(new StructureMapRegistry(dbPath));
                x.ForRepositoriesUseSharpRepository();
            });

            RepositoryDependencyResolver.SetDependencyResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }

        [Test]
        public void EfConfigRepositoryFactory_Using_Ioc_Should_Not_Require_ConnectionString()
        {
            var config = new EfRepositoryConfiguration("TestConfig", null, typeof (TestObjectEntities));
            var factory = new EfConfigRepositoryFactory(config);

            factory.GetInstance<Contact, string>();
        }

        [Test]
        public void EfConfigRepositoryFactory_Using_Ioc_Should_Return_TestObjectEntites_Without_DbContextType_Defined()
        {
            var config = new EfRepositoryConfiguration("TestConfig");
            var factory = new EfConfigRepositoryFactory(config);

            var repos = factory.GetInstance<Contact, string>();

            var propInfo = repos.GetType().GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var dbContext = (TestObjectEntities)propInfo.GetValue(repos, null);
            dbContext.ShouldBeType<TestObjectEntities>();
        }

        [Test]
        public void EfConfigRepositoryFactory_Using_Ioc_Should_Share_DbContext()
        {
            var config = new EfRepositoryConfiguration("TestConfig", "tmp", typeof (TestObjectEntities));
            var factory = new EfConfigRepositoryFactory(config);

            var repos1 = factory.GetInstance<Contact, string>();
            var repos2 = factory.GetInstance<Contact, string>();

            // use reflecton to get the protected Context property
            var propInfo1 = repos1.GetType().GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var dbContext1 = (TestObjectEntities)propInfo1.GetValue(repos1, null);
            var propInfo2 = repos2.GetType().GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var dbContext2 = (TestObjectEntities)propInfo2.GetValue(repos2, null);

            dbContext1.ShouldEqual(dbContext2);
        }

        [Test]
        public void Ioc_For_IRepository_T_Should_Not_Error()
        {
            var repos = ObjectFactory.GetInstance<IRepository<ContactType>>();
        }

        [Test]
        public void Ioc_For_IRepository_T_TKey_Should_Not_Error()
        {
            var repos = ObjectFactory.GetInstance<IRepository<ContactType, int>>();
        }
    }

    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry(string dbPath)
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
            });

            For<DbContext>()
                .HybridHttpOrThreadLocalScoped()
                .Use<TestObjectEntities>()
                .Ctor<string>("connectionString").Is("Data Source=" + dbPath);

            For<TestObjectEntities>()
                .HybridHttpOrThreadLocalScoped()
                .Use<TestObjectEntities>()
                .Ctor<string>("connectionString").Is("Data Source=" + dbPath);
        }
    }
}
