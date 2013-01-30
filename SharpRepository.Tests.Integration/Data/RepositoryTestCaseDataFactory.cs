using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using SharpRepository.Db4oRepository;
using SharpRepository.Tests.Integration.Data.NHibernate;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.XmlRepository;
using SharpRepository.Ef5Repository;
using SharpRepository.RavenDbRepository;
using SharpRepository.MongoDbRepository;
using SharpRepository.InMemoryRepository;
using SharpRepository.NHibernateRepository;

namespace SharpRepository.Tests.Integration.Data
{
    public class RepositoryTestCaseDataFactory
    {
        private static Mutex _sessionMutex = new Mutex();

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

//            if (includeTypes.Contains(RepositoryTypes.NHibernate))
//            {
//                // reference: http://dotnetslackers.com/articles/ado_net/Your-very-first-NHibernate-application-Part-1.aspx#implementing-and-mapping-the-first-object-of-the-domain-model
//                var cfg = Fluently.Configure().Database(SQLiteConfiguration.Standard.InMemory);
//                                  //.Mappings(m => m.FluentMappings.AddFromAssemblyOf<ContactMapping>());
//
//                _sessionMutex.WaitOne();
//
//
//                var sessionSource = new SessionSource(cfg.BuildConfiguration().Properties, new NHibernatePersistenceModel());
//                var session = sessionSource.CreateSession();
//                sessionSource.BuildSchema(session);
////                var sessionFactory = cfg.BuildSessionFactory();
////                var session = sessionFactory.OpenSession();
//
//                _sessionMutex.ReleaseMutex();
//                yield return new TestCaseData(new NHibernateRepository<Contact, string>(session)).SetName("NHibernateRepository Test");
//            }
        }
    }
}