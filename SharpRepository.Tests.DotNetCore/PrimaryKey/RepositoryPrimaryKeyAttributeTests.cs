using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Aspects;
using SharpRepository.Tests.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.PrimaryKey
{
    [TestFixture]
    public class RepositoryPrimaryKeyAttributeTests : TestBase
    {
        [Test]
        public void No_Primary_Key_Should_Return_Null()
        {
            var repos = new TestRepository<NoPrimaryKeyObject, int>();
            repos.TestGetPrimaryKeyPropertyInfo().ShouldBeNull();
        }

        [Test]
        public void Id_Primary_Key_Should_Return_Id_Property()
        {
            var repos = new TestRepository<IdPrimaryKeyObject, int>();
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldBe(typeof(int));
            propInfo.Name.ShouldBe("Id");
        }

        [Test]
        public void Id_Primary_Key_With_Wrong_Type_Should_Return_Null()
        {
            var repos = new TestRepository<IdPrimaryKeyObject, string>();
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.ShouldBeNull();
        }

        [Test]
        public void ClassId_Primary_Key_Should_Return_Id_Property()
        {
            var repos = new TestRepository<ClassNameIdPrimaryKeyObject, int>();
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldBe(typeof(int));
            propInfo.Name.ShouldBe("ClassNameIdPrimaryKeyObjectId");
        }

        [Test]
        public void ClassId_Primary_Key_With_Wrong_Type_Should_Return_Null()
        {
            var repos = new TestRepository<ClassNameIdPrimaryKeyObject, string>();
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.ShouldBeNull();
        }

        [Test]
        public void Attribute_Primary_Key_Should_Return_Id_Property()
        {
            var repos = new TestRepository<UseAttributePrimaryKeyObject, int>();
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldBe(typeof(int));
            propInfo.Name.ShouldBe("SomeRandomName");
        }

        [Test]
        public void Attribute_Primary_Key_With_Wrong_Type_Should_Return_Null()
        {
            var repos = new TestRepository<UseAttributePrimaryKeyObject, string>();
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.ShouldBeNull();
        }
    }

    internal class NoPrimaryKeyObject
    {
        public NoPrimaryKeyObject()
        {

        }
        public string Value { get; set; }
    }

    internal class IdPrimaryKeyObject
    {
        public IdPrimaryKeyObject()
        {

        }
        public int Id { get; set; }
        public string Value { get; set; }
    }

    internal class ClassNameIdPrimaryKeyObject
    {
        public ClassNameIdPrimaryKeyObject()
        {

        }
        public int ClassNameIdPrimaryKeyObjectId { get; set; }
        public string Value { get; set; }
    }

    internal class UseAttributePrimaryKeyObject
    {
        public UseAttributePrimaryKeyObject()
        {

        }
        [RepositoryPrimaryKey]
        public int SomeRandomName { get; set; }
        public string Value { get; set; }
    }

    internal class TestRepository<T, TKey> : InMemoryRepository.InMemoryRepository<T, TKey> where T : class, new()
    {
        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }

        public void SuppressAudit()
        {
            DisableAspect(typeof(AuditAttributeMock));
        }

        public void RestoreAudit()
        {
            EnableAspect(typeof(AuditAttributeMock));
        }

        public IEnumerable<RepositoryActionBaseAttribute> GetAspects()
        {
            return Aspects;
        }
    }
}
