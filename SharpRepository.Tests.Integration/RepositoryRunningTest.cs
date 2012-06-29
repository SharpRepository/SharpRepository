using NUnit.Framework;
using SharpRepository.MongoDbRepository;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryRunningTest : TestBase
    {
        [Test]
        public void MongoServer_Is_Running()
        {
            string connectionString = MongoDbConnectionStringFactory.Build("Test");

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
    }
}