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
        public void Default_PrimaryKeyNameChecks()
        {
            DefaultRepositoryConventions.GetPrimaryKeyName(typeof (Contact)).ShouldEqual("ContactId");
        }
    }
}
