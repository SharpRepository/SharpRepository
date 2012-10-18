using NUnit.Framework;
using SharpRepository.MongoDbRepository;
using SharpRepository.CouchDbRepository;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryRunningTest : TestBase
    {
        [Test]
        public void MongoServer_Is_Running()
        {
            var connectionString = MongoDbConnectionStringFactory.Build("Test");

            if (MongoDbRepositoryManager.ServerIsRunning(connectionString))
            {
                MongoDbRepositoryManager.DropDatabase(connectionString);
                Assert.Pass("MongoServer is running");
            }
            else
            {
                AssertIgnores.MongoServerIsNotRunning();
            }
        }

        [Test]
        public void CouchDb_Is_Running()
        {
            if (CouchDbRepositoryManager.ServerIsRunning(CouchDbUrl.Host, CouchDbUrl.Port))
            {
                Assert.Pass("CouchDb is running");
            }
            else
            {
                AssertIgnores.CouchDbServerIsNotRunning();
            }

        }
    }
}