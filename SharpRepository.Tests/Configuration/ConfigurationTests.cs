using System;
using NUnit.Framework;
using SharpRepository.EfRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using SharpRepository.Tests.TestObjects;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;

namespace SharpRepository.Tests.Configuration
{
    

    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void InMemoryConfigurationNoParametersNoKeyTypes()
        {
            var repos = RepositoryFactory.GetInstance<Contact>();

            if (!(repos is InMemoryRepository<Contact, int>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void InMemoryConfigurationNoParameters()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>();

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void LoadConfigurationRepositoryByName()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>("efRepos");

            if (!(repos is EfRepository<Contact, string>))
            {
                throw new Exception("Not EfRepository");
            }

        }

        [Test]
        public void LoadConfigurationRepositoryBySectionName()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>("sharpRepository2", null);

            if (!(repos is EfRepository<Contact, string>))
            {
                throw new Exception("Not EfRepository");
            }
        }

        [Test]
        public void LoadConfigurationRepositoryBySectionAndRepositoryName()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>("sharpRepository2", "inMem");

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void LoadRepositoryDefaultStrategyAndOverrideNone()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>();

            if (!(repos.CachingStrategy is StandardCachingStrategy<Contact, string>))
            {
                throw new Exception("Not standard caching default");
            }

            repos = RepositoryFactory.GetInstance<Contact, string>("inMemoryNoCaching");

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
            config.AddRepository(new EfRepositoryConfiguration("default", "DefaultConnection", typeof(TestObjectEntities)));
            var repos = RepositoryFactory.GetInstance<Contact, string>(config);

            if (!(repos is EfRepository<Contact, string>))
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
            config.AddRepository(new EfRepositoryConfiguration("ef5", "DefaultConnection", typeof(TestObjectEntities), "standard", "inMemoryProvider"));
            config.DefaultRepository = "ef5";

            config.AddCachingStrategy(new StandardCachingStrategyConfiguration("standard"));
            config.AddCachingStrategy(new TimeoutCachingStrategyConfiguration("timeout", 200));
            config.AddCachingStrategy(new NoCachingStrategyConfiguration("none"));
            
            config.AddCachingProvider(new InMemoryCachingProviderConfiguration("inMemoryProvider"));

            var repos = RepositoryFactory.GetInstance<Contact, string>(config);

            if (!(repos is EfRepository<Contact, string>))
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
            var repos = RepositoryFactory.GetInstance(typeof (Contact), typeof (string));

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void TestFactoryOverloadMethodForCompoundKey()
        {
            var repos = RepositoryFactory.GetInstance(typeof (Contact), typeof (string), typeof(string));

            if (!(repos is InMemoryRepository<Contact, string, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }
    }
}
