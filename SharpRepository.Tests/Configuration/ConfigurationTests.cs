using System;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using SharpRepository.Tests.TestObjects;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;
using Microsoft.Extensions.Configuration;
using System.IO;
using Shouldly;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace SharpRepository.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationTests
    {
        RepositoryFactory factory;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();

            var sectionName = "sharpRepository";

            IConfigurationSection sharpRepoConfig = config.GetSection(sectionName);
            
            if (sharpRepoConfig == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            factory = new RepositoryFactory(sharpRepoConfig);
        }

        [Test]
        public void DefaultInMemoryConfiguration()
        {
            var repos = factory.GetInstance<Contact>();

            if (!(repos is InMemoryRepository<Contact, int>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void InMemoryLoadConfigurationRepositoryByName()
        {
            var sectionName = "sharpRepository2";
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json")
              .Build();
            var sharpRepoConfig2 = config.GetSection(sectionName);
            if (sharpRepoConfig2 == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            var sharpConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(sharpRepoConfig2);
            var repos = RepositoryFactory.GetInstance<Contact, string>(sharpConfig, "inMem");

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not EfRepository");
            }
        }

        [Test]
        public void StandardCacheNeedsIocConfiguration()
        {
            try
            {
                var repos = factory.GetInstance<Contact, string>("inMemory");
                Assert.False(true, "Repo must throw exception");
            }
            catch (Exception e)
            {
                e.InnerException.Message.ShouldBe("RepositoryDependencyResolver.Current must be configured with the instance of IMemoryCache");
            }
        }
        
        [Test]
        public void EfCoreRepositoryNeedsIocConfiguration()
        {
            try
            {
                var repos = factory.GetInstance<Contact, string>("efCoreRepos");
                Assert.False(true, "Repo must throw exception");
            }
            catch (ConfigurationErrorsException e)
            {
                e.Message.ShouldBe("The EfCore repository Factory gets DbContext or DbContextOptionBuilder from RepositoryDependencyResolver containing the Ioc container passing directly DbContextOptions");
            }
        }


        [Test]
        public void LoadInMemoryRepositoryFromConfigurationObject()
        {
            var config = new SharpRepositoryConfiguration();
            config.AddRepository(new InMemoryRepositoryConfiguration("default"));
            var repos = RepositoryFactory.GetInstance<Contact, string>(config);

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }

            if (!(repos.CachingStrategy is NoCachingStrategy<Contact, string>))
            {
                throw new Exception("not NoCachingStrategy");
            }
        }

        [Test]
        public void EfCoreRepositoryFromConfigurationObject()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new TestObjectContextCore(options);

            var config = new SharpRepositoryConfiguration();
            var coreRepoconfig = new EfCoreRepositoryConfiguration("default", dbContext);
            coreRepoconfig.Attributes.Add("dbContextType", "SharpRepository.Tests.TestObjects.TestObjectContextCore, SharpRepository.Tests");

            config.AddRepository(coreRepoconfig);
            
            var repos = RepositoryFactory.GetInstance<Contact, string>(config);

            if (!(repos is EfCoreRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }

            if (!(repos.CachingStrategy is NoCachingStrategy<Contact, string>))
            {
                throw new Exception("not NoCachingStrategy");
            }
        }

        [Test]
        public void EfCoreRepositoryNeedsDbContextType()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new TestObjectContextCore(options);

            var config = new SharpRepositoryConfiguration();
            var coreRepoconfig = new EfCoreRepositoryConfiguration("default", dbContext);

            config.AddRepository(coreRepoconfig);

            try
            {
                var repos = RepositoryFactory.GetInstance<Contact, string>(config);
            } catch (Exception e)
            {
                e.Message.ShouldBe("The DbContextOptions passed to the DbContext constructor must be a DbContextOptions<DbContext>. When registering multiple DbContext types make sure that the constructor for each context type has a DbContextOptions<TContext> parameter rather than a non-generic DbContextOptions parameter.");
            }
        }
        
        [Test]
        public void LoadEfRepositoryAndCachingFromConfigurationObject()
        {
            var config = new SharpRepositoryConfiguration();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new TestObjectContextCore(options);

            config.AddRepository(new InMemoryRepositoryConfiguration("inMemory", "timeout"));

            var coreRepoconfig = new EfCoreRepositoryConfiguration("efCore", dbContext, "standard", "inMemoryProvider");
            coreRepoconfig.Attributes.Add("dbContextType", "SharpRepository.Tests.TestObjects.TestObjectContextCore, SharpRepository.Tests");

            config.AddRepository(coreRepoconfig);
            config.DefaultRepository = "efCore";

            config.AddCachingStrategy(new StandardCachingStrategyConfiguration("standard"));
            config.AddCachingStrategy(new TimeoutCachingStrategyConfiguration("timeout", 200));
            config.AddCachingStrategy(new NoCachingStrategyConfiguration("none"));
            
            config.AddCachingProvider(new InMemoryCachingProviderConfiguration("inMemoryProvider", new MemoryCache(new MemoryCacheOptions())));

            var repos = RepositoryFactory.GetInstance<Contact, string>(config);

            if (!(repos is EfCoreRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }

            if (!(repos.CachingStrategy is StandardCachingStrategy<Contact, string>))
            {
                throw new Exception("not StandardCachingStrategy");
            }
        }

        [Test]
        public void TestFactoryOverloadMethod()
        {
            var repos = factory.GetInstance(typeof(Contact), typeof(string));

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void TestFactoryOverloadMethodForCompoundKey()
        {
            var repos = factory.GetInstance(typeof (Contact), typeof (string), typeof(string));

            if (!(repos is InMemoryRepository<Contact, string, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void TestFactoryOverloadMethodForTripleCompoundKey()
        {
            var repos = factory.GetInstance(typeof(Contact), typeof(string), typeof(string), typeof(string));

            if (!(repos is InMemoryRepository<Contact, string, string, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void TestFactoryOverloadMethodForNoGenericsCompoundKey()
        {
            var repos = factory.GetCompoundKeyInstance(typeof(Contact));

            if (!(repos is InMemoryCompoundKeyRepository<Contact>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }
    }
}
