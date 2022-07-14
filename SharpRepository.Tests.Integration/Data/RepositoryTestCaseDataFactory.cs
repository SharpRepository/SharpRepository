using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.EfRepository;
using SharpRepository.EfCoreRepository;
using SharpRepository.MongoDbRepository;
using SharpRepository.InMemoryRepository;
using System;
using Microsoft.EntityFrameworkCore;

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

            if (includeType.Contains(RepositoryType.Ef))
            {
                var dbPath = EfDataDirectoryFactory.Build();
                yield return
                    new TestCaseData(new EfRepository<Contact, string>(new TestObjectContext("Data Source=" + dbPath))).SetName("EfRepository" + testName);
            }

            if (includeType.Contains(RepositoryType.EfCore))
            {
                var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                     .UseInMemoryDatabase("integration test")
                     .Options;

                // Create the schema in the database
                var context = new TestObjectContextCore(options);
                context.Database.EnsureCreated();
                yield return new TestCaseData(new EfCoreRepository<Contact, string>(context)).SetName("EfCoreRepository " + testName);
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

        }
    }
}