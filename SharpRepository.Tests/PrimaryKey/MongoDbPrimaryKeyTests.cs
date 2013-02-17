using System.Reflection;
using NUnit.Framework;
using SharpRepository.Tests.TestObjects.PrimaryKeys;
using Should;

namespace SharpRepository.Tests.PrimaryKey
{
    [TestFixture]
    public class MongoDbPrimaryKeyTests
    {
        [Test]
        public void Should_Return_KeyInt1_Property()
        {
            var repos = new TestMongoDbRepository<ObjectKeys, int>();
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldEqual(typeof(int));
            propInfo.Name.ShouldEqual("KeyInt1");
        }
    }

    internal class TestMongoDbRepository<T, TKey> : MongoDbRepository.MongoDbRepository<T, TKey> where T : class, new()
    {
        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }
}
