using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using Raven.Client.Document;
using Raven.Client.Embedded;
using SharpRepository.Db4oRepository;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.XmlRepository;
using SharpRepository.EfRepository;
using SharpRepository.RavenDbRepository;
using SharpRepository.MongoDbRepository;

namespace SharpRepository.Tests.Integration.Data
{
    public class RepositoryTestCaseDataFactory
    {
        public static IEnumerable<TestCaseData> Build(RepositoryTypes[] includeTypes)
        {
            //if (includeTypes.Contains(RepositoryTypes.All) || includeTypes.Contains(RepositoryTypes.InMemory))
            //{
            //    yield return new TestCaseData(new InMemoryRepository<Contact, int>()).SetName("InMemoryRepository Test");
            //}

            //if (includeTypes.Contains(RepositoryTypes.All) || includeTypes.Contains(RepositoryTypes.Xml))
            //{
            //    var xmlDataDirectoryPath = XmlDataDirectoryFactory.Build("Contact");
            //    yield return
            //        new TestCaseData(new XmlRepository<Contact, int>(xmlDataDirectoryPath)).SetName("XmlRepository Test");
            //}

            //if (includeTypes.Contains(RepositoryTypes.All) || includeTypes.Contains(RepositoryTypes.Ef))
            //{
            //    var dbPath = EfDataDirectoryFactory.Build();
            //    Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            //    yield return
            //        new TestCaseData(new EfRepository<Contact, int>(new TestObjectEntities("Data Source=" + dbPath))).SetName("EfRepository Test");
            //}

            //if (includeTypes.Contains(RepositoryTypes.All) || includeTypes.Contains(RepositoryTypes.Dbo4))
            //{
            //    var dbPath = Db4oDataDirectoryFactory.Build("Contact");
            //    yield return new TestCaseData(new Db4oRepository<Contact, int>(dbPath)).SetName("Db4oRepository Test");
            //}

            if (includeTypes.Contains(RepositoryTypes.All) || includeTypes.Contains(RepositoryTypes.MongoDb))
            {
                const string connectionString = "mongodb://localhost/?safe=true";
                yield return new TestCaseData(new MongoDbRepository<Contact, int>(connectionString)).SetName("MongoDb Test");
            }

            //if (includeTypes.Contains(RepositoryTypes.All) || includeTypes.Contains(RepositoryTypes.RavenDb))
            //{
            //    var documentStore = new EmbeddableDocumentStore
            //                            {
            //                                RunInMemory = true,
            //                                Conventions = { DefaultQueryingConsistency = ConsistencyOptions.QueryYourWrites }
            //                            };
            //    yield return new TestCaseData(new RavenDbRepository<Contact, int>(documentStore)).SetName("RavenDbRepository Test");
            //}
        }
    }
}