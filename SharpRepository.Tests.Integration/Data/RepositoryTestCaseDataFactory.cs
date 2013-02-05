using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using SharpRepository.Db4oRepository;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.XmlRepository;
using SharpRepository.Ef5Repository;
using SharpRepository.RavenDbRepository;
using SharpRepository.MongoDbRepository;
using SharpRepository.InMemoryRepository;
using SharpRepository.CacheRepository;


namespace SharpRepository.Tests.Integration.Data
{
    public class RepositoryTestCaseDataFactory
    {
        public static IEnumerable<TestCaseData> Build(RepositoryTypes[] includeTypes)
        {
            if (includeTypes.Contains(RepositoryTypes.InMemory))
            {
                yield return new TestCaseData(new InMemoryRepository<Contact, string>()).SetName("InMemoryRepository Test");
            }

            if (includeTypes.Contains(RepositoryTypes.Xml))
            {
                var xmlDataDirectoryPath = XmlDataDirectoryFactory.Build("Contact");
                yield return
                    new TestCaseData(new XmlRepository<Contact, string>(xmlDataDirectoryPath)).SetName("XmlRepository Test");
            }

            if (includeTypes.Contains(RepositoryTypes.Ef5))
            {
                var dbPath = EfDataDirectoryFactory.Build();
                Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
                yield return
                    new TestCaseData(new Ef5Repository<Contact, string>(new TestObjectEntities("Data Source=" + dbPath))).SetName("EfRepository Test");
            }

            if (includeTypes.Contains(RepositoryTypes.Dbo4))
            {
                var dbPath = Db4oDataDirectoryFactory.Build("Contact");
                yield return new TestCaseData(new Db4oRepository<Contact, string>(dbPath)).SetName("Db4oRepository Test");
            }

            if (includeTypes.Contains(RepositoryTypes.MongoDb))
            {
                string connectionString = MongoDbConnectionStringFactory.Build("Contact");
           
                if (MongoDbRepositoryManager.ServerIsRunning(connectionString))
                {
                    MongoDbRepositoryManager.DropDatabase(connectionString); // Pre-test cleanup
                    yield return new TestCaseData(new MongoDbRepository<Contact, string>(connectionString)).SetName("MongoDb Test");
                }
            }

            if (includeTypes.Contains(RepositoryTypes.RavenDb))
            {
                var documentStore = new EmbeddableDocumentStore
                                        {
                                            RunInMemory = true,
                                            Conventions = { DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites }
                                        };
                yield return new TestCaseData(new RavenDbRepository<Contact, string>(documentStore)).SetName("RavenDbRepository Test");
            }

            if (includeTypes.Contains(RepositoryTypes.Cache))
            {
                yield return new TestCaseData(new CacheRepository<Contact, string>(CachePrefixFactory.Build())).SetName("CacheRepository Test");
            }
        }
    }
}