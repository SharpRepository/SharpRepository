using System.Data.Entity;
using System.Reflection;
using NUnit.Framework;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects.PrimaryKeys;
using Shouldly;

namespace SharpRepository.Tests.PrimaryKey
{
    [TestFixture]
    public class EfPrimaryKeyTests
    {
        [Test]
        public void Should_Return_KeyInt2_Property()
        {
            var repos = new TestEfRepository<ObjectKeys, int>(new DbContext("test"));
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldBe(typeof(int));
            propInfo.Name.ShouldBe("KeyInt2");
        }

        [Test]
        public void Should_Return_KeyInt1_2_3_Property()
        {
            var repos = new TestTripleKeyEfRepository<TripleObjectKeys, int, int, int>(new DbContext("test"));
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo[0].PropertyType.ShouldBe(typeof(int));
            propInfo[0].Name.ShouldBe("KeyInt1");
            propInfo[1].PropertyType.ShouldBe(typeof(int));
            propInfo[1].Name.ShouldBe("KeyInt2");
            propInfo[2].PropertyType.ShouldBe(typeof(int));
            propInfo[2].Name.ShouldBe("KeyInt3");
        }
    }

    internal class TestEfRepository<T, TKey> : EfRepository.EfRepository<T, TKey> where T : class, new()
    {
        public TestEfRepository(DbContext dbContext, ICachingStrategy<T, TKey> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }

        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }

    internal class TestTripleKeyEfRepository<T, TKey, TKey2, TKey3> : EfRepository.EfRepository<T, TKey, TKey2, TKey3> where T : class, new()
    {
        public TestTripleKeyEfRepository(DbContext dbContext, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }

        public PropertyInfo[] TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }
}