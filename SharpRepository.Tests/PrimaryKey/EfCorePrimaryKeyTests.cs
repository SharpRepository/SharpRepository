using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Shouldly;
using System.Reflection;

namespace SharpRepository.Tests.PrimaryKey
{
    [TestFixture]
    public class EfCorePrimaryKeyTests
    {
        protected DbContext context;

        [SetUp]
        public void Setup()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            
            var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                .UseSqlite(connection)
                .Options;

            // Create the schema in the database
            using (var context = new TestObjectContextCore(options))
            {
                context.Database.EnsureCreated();
            }

            // Run the test against one instance of the context
            context = new TestObjectContextCore(options);

        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
            context = null;
        }

        [Test]
        public void Should_Return_ContactId_Property()
        {
            var repos = new TestEfCoreRepository<Contact, int>(context);
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldBe(typeof(int));
            propInfo.Name.ShouldBe("ContactId");
        }

        [Test]
        public void Should_Return_Some_Another_Last_Id_Property()
        {
            var repos = new TestTripleKeyEfCoreRepository<TripleCompoundKeyItemInts, int, int, int>(context);
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo[0].PropertyType.ShouldBe(typeof(int));
            propInfo[0].Name.ShouldBe("SomeId");
            propInfo[1].PropertyType.ShouldBe(typeof(int));
            propInfo[1].Name.ShouldBe("AnotherId");
            propInfo[2].PropertyType.ShouldBe(typeof(int));
            propInfo[2].Name.ShouldBe("LastId");
        }
    }

    internal class TestEfCoreRepository<T, TKey> : EfCoreRepository<T, TKey> where T : class, new()
    {
        public TestEfCoreRepository(DbContext dbContext, ICachingStrategy<T, TKey> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }

        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }

    internal class TestTripleKeyEfCoreRepository<T, TKey, TKey2, TKey3> : EfCoreRepository<T, TKey, TKey2, TKey3> where T : class, new()
    {
        public TestTripleKeyEfCoreRepository(DbContext dbContext, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }

        public PropertyInfo[] TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }
}