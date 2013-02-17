using System.Data.Entity;
using System.Reflection;
using NUnit.Framework;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects.PrimaryKeys;
using Should;

namespace SharpRepository.Tests.PrimaryKey
{
    [TestFixture]
    public class Ef5PrimaryKeyTests
    {
        [Test]
        public void Should_Return_KeyInt2_Property()
        {
            var repos = new TestEf5Repository<ObjectKeys, int>(new DbContext("test"));
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldEqual(typeof(int));
            propInfo.Name.ShouldEqual("KeyInt2");
        }
    }

    internal class TestEf5Repository<T, TKey> : Ef5Repository.Ef5Repository<T, TKey> where T : class, new()
    {
        public TestEf5Repository(DbContext dbContext, ICachingStrategy<T, TKey> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }

        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }
}