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
using Shouldly;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Web;
using StructureMap.Graph;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class RepositoryDependencySpikes
    {
        protected Container container;

        [SetUp]
        public void Setup()
        {
            var dbPath = EfDataDirectoryFactory.Build();

            // structure map
            container = new Container(x =>
            {
                x.AddRegistry(new StructureMapRegistry(dbPath));
                x.ForRepositoriesUseSharpRepository();
            });

            RepositoryDependencyResolver.SetDependencyResolver(new StructureMapRepositoryDependencyResolver(container));
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

            dbContext1.ShouldBe(dbContext2);
        }

        [Test]
        public void Ioc_For_IRepository_T_Should_Be_EfRepository_T()
        {
            var repos = container.GetInstance<IRepository<ContactType>>();
            repos.ShouldBeType<EfRepository<ContactType>>();
        }

        [Test]
        public void Ioc_For_IRepository_T_TKey_Should_Be_EfRepository_T_TKey()
        {
            var repos = container.GetInstance<IRepository<ContactType, int>>();
            repos.ShouldBeType<EfRepository<ContactType, int>>();
        }

        [Test]
        public void Ioc_For_ICompoundKeyRepository_T_TKey_TKey2_Should_Be_EfRepository_T_TKey_TKey2()
        {
            var repos = container.GetInstance<ICompoundKeyRepository<ContactType, int, string>>();
            repos.ShouldBeType<EfRepository<ContactType, int, string>>();
        }

        [Test]
        public void Ioc_For_ICompoundRepository_T_TKey_TKey2_TKey3_Should_Be_EfRepository_T_TKey_TKey2_TKey3()
        {
            var repos = container.GetInstance<ICompoundKeyRepository<ContactType, int, string, string>>();
            repos.ShouldBeType<EfRepository<ContactType, int,string,string>>();
        }

        [Test]
        public void Ioc_For_ICompoundRepository_T_Should_Be_EfCompoundRepository_T()
        {
            var repos = container.GetInstance<ICompoundKeyRepository<ContactType>>();
            repos.ShouldBeType<EfCompoundKeyRepository<ContactType>>();
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
