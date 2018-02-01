using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using SharpRepository.CouchDbRepository;
using SharpRepository.Db4oRepository;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.XmlRepository;
using SharpRepository.EfRepository;
using SharpRepository.EfCoreRepository;
using SharpRepository.RavenDbRepository;
using SharpRepository.MongoDbRepository;
using SharpRepository.InMemoryRepository;
using SharpRepository.CacheRepository;
using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SharpRepository.Repository.Caching;
using Microsoft.Extensions.Caching.Memory;
using Raven.Client;

namespace SharpRepository.Tests.Integration.Data
{
    public class RepositoryTestCaseDataFactory
    {
        public static IEnumerable<TestCaseData> Build(RepositoryType[] includeType, string testName = "Test")
        {
            if (includeType.Contains(RepositoryType.InMemory))
            {
                yield return new TestCaseData(new InMemoryRepository<Contact, string>()).SetName("InMemoryRepository " + testName);
            }

            if (includeType.Contains(RepositoryType.Xml))
            {
                var xmlDataDirectoryPath = XmlDataDirectoryFactory.Build("Contact");
                yield return
                    new TestCaseData(new XmlRepository<Contact, string>(xmlDataDirectoryPath)).SetName("XmlRepository" + testName);
            }

            if (includeType.Contains(RepositoryType.Ef))
            {
                var dbPath = EfDataDirectoryFactory.Build();
                yield return
                    new TestCaseData(new EfRepository<Contact, string>(new TestObjectContext("Data Source=" + dbPath))).SetName("EfRepository" + testName);
            }

            if (includeType.Contains(RepositoryType.EfCore))
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                     .UseSqlite(connection)
                     .Options;

                // Create the schema in the database
                var context = new TestObjectContextCore(options);
                context.Database.EnsureCreated();
                yield return new TestCaseData(new EfCoreRepository<Contact, string>(context)).SetName("EfCoreRepository " + testName);
            }

            if (includeType.Contains(RepositoryType.Dbo4))
            {
                var dbPath = Db4oDataDirectoryFactory.Build("Contact");
                yield return new TestCaseData(new Db4oRepository<Contact, string>(dbPath)).SetName("Db4oRepository " + testName);
            }

            if (includeType.Contains(RepositoryType.MongoDb))
            {
                string connectionString = MongoDbConnectionStringFactory.Build("Contact");
           
                if (MongoDbRepositoryManager.ServerIsRunning(connectionString))
                {
                    MongoDbRepositoryManager.DropDatabase(connectionString); // Pre-test cleanup
                    yield return new TestCaseData(new MongoDbRepository<Contact, string>(connectionString)).SetName("MongoDb " + testName);
                }
            }

            if (includeType.Contains(RepositoryType.RavenDb))
            {
                var documentStore = new EmbeddableDocumentStore
                {
                        RunInMemory = true,
                        DataDirectory = "~\\Data\\RavenDb"
                };
                if (IntPtr.Size == 4)
                {
                    documentStore.Configuration.Storage.Voron.AllowOn32Bits = true;
                }

                IDocumentStore x = new EmbeddableDocumentStore();
                yield return new TestCaseData(new RavenDbRepository<Contact, string>(documentStore: documentStore)).SetName("RavenDbRepository " + testName);
            }

            if (includeType.Contains(RepositoryType.Cache))
            {
                var cachingProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
                yield return new TestCaseData(new CacheRepository<Contact, string>(CachePrefixFactory.Build(), cachingProvider)).SetName("CacheRepository " + testName);
            }

            if (includeType.Contains(RepositoryType.CouchDb))
            {
                if (CouchDbRepositoryManager.ServerIsRunning(CouchDbUrl.Host, CouchDbUrl.Port))
                {
                    var databaseName = CouchDbDatabaseNameFactory.Build("Contact");
                    CouchDbRepositoryManager.DropDatabase(CouchDbUrl.Host, CouchDbUrl.Port, databaseName);
                    CouchDbRepositoryManager.CreateDatabase(CouchDbUrl.Host, CouchDbUrl.Port, databaseName);

                    yield return new TestCaseData(new CouchDbRepository<Contact, string>(CouchDbUrl.Host, CouchDbUrl.Port, databaseName)).SetName("CouchDbRepository " + testName);
                }

            }
        }
    }
}