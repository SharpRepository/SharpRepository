using System;
using System.Configuration;
using NUnit.Framework;
using SharpRepository.Repository.Configuration;
using SharpRepository.Tests.TestObjects;
using SharpRepository.InMemoryRepository;
using SharpRepository.EfRepository;
using SharpRepository.Repository;

namespace SharpRepository.Tests.Configuration
{
    

    [TestFixture]
    public class ConfigurationTests
    {
        private System.Configuration.Configuration _config;

        [SetUp]
        public void SetUp()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        [Test]
        public void LoadConfiguration()
        {          
            var section = (SharpRepositorySection)_config.GetSection("sharpRepository");
            if (section == null)
                throw new ConfigurationErrorsException("Section  is not found.");
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
    }
}
