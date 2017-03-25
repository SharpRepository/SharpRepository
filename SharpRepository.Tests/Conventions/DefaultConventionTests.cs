using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Should;
using System;

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

        [Test]
        public void Default_CachePrefix()
        {
            var repos = new InMemoryRepository.InMemoryRepository<Contact>(new StandardCachingStrategy<Contact, int>());
            repos.CachingStrategy.FullCachePrefix.ShouldStartWith(DefaultRepositoryConventions.CachePrefix);
        }

        [Test]
        public void Change_CachePrefix()
        {
            const string newPrefix = "TestPrefix123";
            DefaultRepositoryConventions.CachePrefix = newPrefix;

            var repos = new InMemoryRepository.InMemoryRepository<Contact>(new StandardCachingStrategy<Contact, int>());
            repos.CachingStrategy.FullCachePrefix.ShouldStartWith(newPrefix);
        }

        [Test]
        public void RavenDb_Throws_NotSupportedException_When_GenerateKeyOnAdd_Is_Set_False()
        {
            var ravenDbRepo = new RavenDbRepository.RavenDbRepository<Contact>();
            Exception actualException = null;

            try
            {
                ravenDbRepo.GenerateKeyOnAdd = false;
            }
            catch(Exception ex)
            {
                actualException = ex;
            }

            actualException.ShouldNotBeNull();
            actualException.ShouldBeType<NotSupportedException>();
        }

        internal class TestConventionObject
        {
            public int PK_TestConventionObject_Id { get; set; }
            public string Name { get; set; }
        }
    }

    
}
