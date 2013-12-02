using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using SharpRepository.CouchDbRepository;
using SharpRepository.Db4oRepository;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.XmlRepository;
using SharpRepository.EfRepository;
using SharpRepository.RavenDbRepository;
using SharpRepository.MongoDbRepository;
using SharpRepository.InMemoryRepository;
using SharpRepository.CacheRepository;


namespace SharpRepository.Tests.Integration.Data
{
    public class RepositoryTestCaseDataFactory
    {
        public static IEnumerable<TestCaseData> Build(RepositoryType[] includeType)
        {
            if (includeType.Contains(RepositoryType.InMemory))
            {
                yield return new TestCaseData(new InMemoryRepository<Contact, string>()).SetName("InMemoryRepository Test");
            }

            if (includeType.Contains(RepositoryType.Xml))
            {
                var xmlDataDirectoryPath = XmlDataDirectoryFactory.Build("Contact");
                yield return
                    new TestCaseData(new XmlRepository<Contact, string>(xmlDataDirectoryPath)).SetName("XmlRepository Test");
            }

            if (includeType.Contains(RepositoryType.Ef5))
            {
                var dbPath = EfDataDirectoryFactory.Build();
                Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
                yield return
                    new TestCaseData(new EfRepository<Contact, string>(new TestObjectEntities("Data Source=" + dbPath))).SetName("EfRepository Test");
            }

            if (includeType.Contains(RepositoryType.Dbo4))
            {
                var dbPath = Db4oDataDirectoryFactory.Build("Contact");
                yield return new TestCaseData(new Db4oRepository<Contact, string>(dbPath)).SetName("Db4oRepository Test");
            }

            if (includeType.Contains(RepositoryType.MongoDb))
            {
                string connectionString = MongoDbConnectionStringFactory.Build("Contact");
           
                if (MongoDbRepositoryManager.ServerIsRunning(connectionString))
                {
                    MongoDbRepositoryManager.DropDatabase(connectionString); // Pre-test cleanup
                    yield return new TestCaseData(new MongoDbRepository<Contact, string>(connectionString)).SetName("MongoDb Test");
                }
            }

            if (includeType.Contains(RepositoryType.RavenDb))
            {
                var documentStore = new EmbeddableDocumentStore
                                        {
                                            RunInMemory = true,
                                            Conventions = { DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites }
                                        };
                yield return new TestCaseData(new RavenDbRepository<Contact, string>(documentStore)).SetName("RavenDbRepository Test");
            }

            if (includeType.Contains(RepositoryType.Cache))
            {
                yield return new TestCaseData(new CacheRepository<Contact, string>(CachePrefixFactory.Build())).SetName("CacheRepository Test");
            }

            if (includeType.Contains(RepositoryType.CouchDb))
            {
                if (CouchDbRepositoryManager.ServerIsRunning(CouchDbUrl.Host, CouchDbUrl.Port))
                {
                    var databaseName = CouchDbDatabaseNameFactory.Build("Contact");
                    CouchDbRepositoryManager.DropDatabase(CouchDbUrl.Host, CouchDbUrl.Port, databaseName);
                    CouchDbRepositoryManager.CreateDatabase(CouchDbUrl.Host, CouchDbUrl.Port, databaseName);

                    yield return new TestCaseData(new CouchDbRepository<Contact>(CouchDbUrl.Host, CouchDbUrl.Port, databaseName)).SetName("CouchDbRepository Test");
                }

            }
        }
    }
}