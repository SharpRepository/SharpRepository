using System.Linq;
using NUnit.Framework;
// using ServiceStack.Common.Extensions;
using SharpRepository.Repository.Aspects;
using SharpRepository.Tests.PrimaryKey;
using SharpRepository.Tests.TestObjects;

namespace SharpRepository.Tests.Aspects
{
    [TestFixture]
    public class CompoundKeyRepositoryActionBaseAttributeTests
    {
        [Test]
        public void Aspect_IsNotCalled_WhenDisabled()
        {
            //Arrange
            var repository = new CompoundKeyTestRepository<CompoundKeyItemInts, int, int>();
            var aspect = repository
                .GetAspects()
                .OfType<AuditAttributeMock>()
                .First();

            //Act
            repository.SuppressAudit();
            var item = repository.Get(1,1);

            //Assert
            Assert.IsTrue(aspect.OnInitializedCalled, "OnInitializedCalled not called");
            Assert.IsFalse(aspect.Enabled, "unexpecly Enabled");
            Assert.IsFalse(aspect.OnGetExecutingCalled, "unexpecly OnGetExecutingCalled");
            Assert.IsFalse(aspect.OnGetExecutedCalled, "unexpecly OnGetExecutedCalled");
        }

        [Test]
        public void Aspect_IsCalled_WhenReEnabled()
        {
            //Arrange
            var repository = new CompoundKeyTestRepository<CompoundKeyItemInts, int, int>();
            var aspect = repository
                .GetAspects()
                .OfType<AuditAttributeMock>()
                .First();

            //Act
            repository.SuppressAudit();
            repository.RestoreAudit();
            var item = repository.Get(1, 1);

            //Assert
            Assert.IsTrue(aspect.OnInitializedCalled, "OnInitializedCalled not called");
            Assert.IsTrue(aspect.Enabled, "not Enabled");
            Assert.IsTrue(aspect.OnGetExecutingCalled, "OnGetExecutingCalled not called");
            Assert.IsTrue(aspect.OnGetExecutedCalled, "OnGetExecutedCalled not called");
        }

        [Test]
        public void Aspect_WhenMultipleApplied_ExecutedInOrder()
        {
            //Arrange
            var repository = new CompoundKeyTestRepository<CompoundKeyItemInts, int, int>();
            var aspects = repository
                .GetAspects()
                .ToArray();
            var audit = (AuditAttributeMock)aspects.First(a => a is AuditAttributeMock);
            var specificAudit = (SpecificAuditAttribute)aspects.First(a => a is SpecificAuditAttribute);
            
            //Act
            var item = repository.Get(1, 1);
            
            //Assert
            Assert.Greater(specificAudit.ExecutedOn, audit.ExecutedOn);
        }
    }
}