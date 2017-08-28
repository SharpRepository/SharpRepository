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
        public void InMemoryConfigurationNoParametersNoKeyTypes()
        {
            var repos = factory.GetInstance<Contact>();

            if (!(repos is InMemoryRepository<Contact, int>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void InMemoryConfigurationNoParameters()
        {
            var repos = factory.GetInstance<Contact, string>();

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void LoadConfigurationRepositoryByName()
        {
            var sectionName = "sharpRepository2";
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddXmlFile("app.config")
               .Build();
            var sharpRepoConfig2 = config.GetSection(sectionName) as ISharpRepositoryConfiguration;
            if (sharpRepoConfig2 == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            var repos = RepositoryFactory.GetInstance<Contact, string>(sharpRepoConfig2, "efRepos");

            if (!(repos is EfCoreRepository<Contact, string>))
            {
                throw new Exception("Not EfRepository");
            }
        }

        [Test]
        public void LoadConfigurationRepositoryBySectionName()
        {
            var sectionName = "sharpRepository2";
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddXmlFile("app.config")
               .Build();
            var sharpRepoConfig2 = config.GetSection(sectionName) as ISharpRepositoryConfiguration;
            if (sharpRepoConfig2 == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            var repos = RepositoryFactory.GetInstance<Contact, string>(sharpRepoConfig2, null);

            if (!(repos is EfCoreRepository<Contact, string>))
            {
                throw new Exception("Not EfRepository");
            }
        }

        [Test]
        public void LoadConfigurationRepositoryBySectionAndRepositoryName()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddXmlFile("app.config")
               .Build();

            var sectionName = "sharpRepository2";

            var sharpRepoConfig2 = config.GetSection(sectionName) as ISharpRepositoryConfiguration;
            if (sharpRepoConfig2 == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            var repos = RepositoryFactory.GetInstance<Contact, string>(sharpRepoConfig2, "inMem");

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void LoadRepositoryDefaultStrategyAndOverrideNone()
        {
            var repos = factory.GetInstance<Contact, string>();

            if (!(repos.CachingStrategy is StandardCachingStrategy<Contact, string>))
            {
                throw new Exception("Not standard caching default");
            }

            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddXmlFile("app.config")
               .Build();

            var sectionName = "inMemoryNoCaching";

            var sharpRepoConfig2 = config.GetSection(sectionName) as ISharpRepositoryConfiguration;
            if (sharpRepoConfig2 == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");


            repos = RepositoryFactory.GetInstance<Contact, string>(sharpRepoConfig2);

            if (!(repos.CachingStrategy is NoCachingStrategy<Contact, string>))
            {
                throw new Exception("Not the override of default for no caching");
            }
        }

        [Test]
        public void LoadInMemoryRepositoryFromConfigurationObject()
        {
            var config = new SharpRepositoryConfiguration();
//            config.AddRepository("default", typeof(InMemoryConfigRepositoryFactory));
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
        public void LoadEfRepositoryFromConfigurationObject()
        {
            var config = new SharpRepositoryConfiguration();
            config.AddRepository(new EfCoreRepositoryConfiguration("default", "DefaultConnection", typeof(TestObjectContext)));
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
        public void LoadEfRepositoryAndCachingFromConfigurationObject()
        {
            var config = new SharpRepositoryConfiguration();
            config.AddRepository(new InMemoryRepositoryConfiguration("inMemory", "timeout"));
            config.AddRepository(new EfCoreRepositoryConfiguration("ef5", "DefaultConnection", typeof(TestObjectContext), "standard", "inMemoryProvider"));
            config.DefaultRepository = "ef5";

            config.AddCachingStrategy(new StandardCachingStrategyConfiguration("standard"));
            config.AddCachingStrategy(new TimeoutCachingStrategyConfiguration("timeout", 200));
            config.AddCachingStrategy(new NoCachingStrategyConfiguration("none"));
            
            config.AddCachingProvider(new InMemoryCachingProviderConfiguration("inMemoryProvider"));

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
