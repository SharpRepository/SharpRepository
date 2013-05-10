using System;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Conventions
{
    [TestFixture]
    public class RepositoryConventionTests
    {
        [Test]
        public void Default_PrimaryKeySuffix_Is_Id()
        {
            DefaultRepositoryConventions.PrimaryKeySuffix.ShouldEqual("Id");
        }

        [Test]
        public void RepositoryConventions_Uses_Default_PrimaryKeySuffix()
        {
            DefaultRepositoryConventions.PrimaryKeySuffix = "Key";
            DefaultRepositoryConventions.GetPrimaryKeyName(typeof (Contact)).ShouldBeNull();

            // change back to default for the rest of the tests
            DefaultRepositoryConventions.PrimaryKeySuffix = "Id";
        }

        [Test]
        public void Default_PrimaryKeyName()
        {
            DefaultRepositoryConventions.GetPrimaryKeyName(typeof (Contact)).ShouldEqual("ContactId");
        }

        [Test]
        public void Change_PrimaryKeyName()
        {
            var orig = DefaultRepositoryConventions.GetPrimaryKeyName;

            DefaultRepositoryConventions.GetPrimaryKeyName = type => "PK_" + type.Name + "_Id";

            DefaultRepositoryConventions.GetPrimaryKeyName(typeof(TestConventionObject)).ShouldEqual("PK_TestConventionObject_Id");

            DefaultRepositoryConventions.GetPrimaryKeyName = orig;
        }

        internal class TestConventionObject
        {
            public int PK_TestConventionObject_Id { get; set; }
            public string Name { get; set; }
        }
    }

    
}
