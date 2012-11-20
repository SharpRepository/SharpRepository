using System;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.RavenDbRepository;
using SharpRepository.MongoDbRepository;
using SharpRepository.EfRepository;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class ConfigurationSpikes
    {
        [Test]
        public void DefaultRaveDbConfiguration()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>();

            if (!(repos is RavenDbRepository<Contact, string>))
            {
                throw new Exception("Not RavenDbRepository");
            }
        }

        [Test]
        public void EfConfiguration()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>("efRepositoryTest");

            if (!(repos is EfRepository<Contact, string>))
            {
                throw new Exception("Not EfRepository");
            }
        }
    }
}
