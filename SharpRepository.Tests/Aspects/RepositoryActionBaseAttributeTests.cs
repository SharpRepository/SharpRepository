using System.Linq;
using NUnit.Framework;
// using ServiceStack.Common.Extensions;
using SharpRepository.Repository.Aspects;
using SharpRepository.Tests.PrimaryKey;
using SharpRepository.Tests.TestObjects;

namespace SharpRepository.Tests.Aspects
{
    [TestFixture]
    public class RepositoryActionBaseAttributeTests
    {
        [Test]
        public void Aspect_IsNotCalled_WhenDisabled()
        {
            //Arrange
            var repository = new TestRepository<Product, int>();
            var aspect = repository
                .GetAspects()
                .OfType<AuditAttributeMock>()
                .First();

            //Act
            repository.SuppressAudit();
            var product = repository.Get(1);


            //Assert
            Assert.IsTrue(aspect.OnInitializedCalled);
            Assert.IsFalse(aspect.Enabled);
            Assert.IsFalse(aspect.OnGetExecutingCalled);
            Assert.IsFalse(aspect.OnGetExecutedCalled);
        }

        [Test]
        public void Aspect_IsCalled_WhenReEnabled()
        {
            //Arrange
            var repository = new TestRepository<Product, int>();
            var aspect = repository
                .GetAspects()
                .OfType<AuditAttributeMock>()
                .First();

            //Act
            repository.SuppressAudit();
            repository.RestoreAudit();
            var product = repository.Get(1);


            //Assert
            Assert.IsTrue(aspect.OnInitializedCalled);
            Assert.IsTrue(aspect.Enabled);
            Assert.IsTrue(aspect.OnGetExecutingCalled);
            Assert.IsTrue(aspect.OnGetExecutedCalled);
        }

        [Test]
        public void Aspect_WhenMultipleApplied_ExecutedInOrder()
        {
            //Arrange
            var repository = new TestRepository<Product, int>();
            var aspects = repository
                .GetAspects()
                .ToArray();
            var audit = (AuditAttributeMock)aspects.First(a => a is AuditAttributeMock);
            var specificAudit = (SpecificAuditAttribute)aspects.First(a => a is SpecificAuditAttribute);
            
            //Act
            var product = repository.Get(1);
            
            //Assert
            Assert.Greater(specificAudit.ExecutedOn, audit.ExecutedOn);
        }
    }
}