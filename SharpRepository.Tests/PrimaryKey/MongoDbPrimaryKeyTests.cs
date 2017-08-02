using System.Reflection;
using NUnit.Framework;
using SharpRepository.Tests.TestObjects.PrimaryKeys;
using Shouldly;
using SharpRepository.MongoDbRepository;
using MongoDB.Driver;

namespace SharpRepository.Tests.PrimaryKey
{
    [TestFixture]
    public class MongoDbPrimaryKeyTests
    {
        [Test]
        public void Should_Return_KeyInt1_Property()
        {
            // TO DEL
            var _databaseName = MongoUrl.Create("mongodb://localhost/test").DatabaseName;
            var cli = new MongoClient("mongodb://localhost/test");

            var repos = new TestMongoDbRepository<ObjectKeys, int>();
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldBe(typeof(int));
            propInfo.Name.ShouldBe("KeyInt1");
        }
    }

    internal class TestMongoDbRepository<T, TKey> : MongoDbRepository<T, TKey> where T : class, new()
    {
        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }
}
