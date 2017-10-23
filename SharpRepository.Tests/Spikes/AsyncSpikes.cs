using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Tests.TestObjects;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    public class AsyncSpikes
    {
        private TestObjectContextCore context;

        [SetUp]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseSqlite(connection)
                .Options;

            // Run the test against one instance of the context
            context = new TestObjectContextCore(options);
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            context = null;
        }

        [Test]
        public void TestAsyncRequest()
        {
            var repo = new EfCoreRepository<Contact>(context);

            var contacts = repo.GetAll().ToAsyncEnumerable().ToList();

            contacts.Result.Count.ShouldBe(0);
        }
    }
}
