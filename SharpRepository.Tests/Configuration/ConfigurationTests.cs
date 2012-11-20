using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpRepository.Repository.Configuration;
using SharpRepository.Tests.TestObjects;

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
            var section = (SharpRepositorySectionGroup)_config.GetSectionGroup("sharpRepository");
            if (section == null)
                throw new ConfigurationErrorsException("Section  is not found.");
        }

        [Test]
        public void InMemoryConfigurationNoParameters()
        {
            var repos = Repository.Repository.GetInstance<Contact, int>();

            if (!(repos is InMemoryRepository.InMemoryRepository<Contact, int>))
            {
                throw new Exception("Not InMemoryRepository");
            }

        }

    }
}
