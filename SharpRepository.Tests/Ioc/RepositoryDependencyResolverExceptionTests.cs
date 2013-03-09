using System;
using NUnit.Framework;
using SharpRepository.Repository.Ioc;
using Should;

namespace SharpRepository.Tests.Ioc
{
    [TestFixture]
    public class RepositoryDependencyResolverExceptionTests
    {
        [Test]
        public void DependencyType_Should_Get_Set()
        {
            var ex = new RepositoryDependencyResolverException(typeof (Int32));
            ex.DependencyType.ShouldEqual(typeof(Int32));
        }

        [Test]
        public void InnerException_Should_Get_Set()
        {
            var ex = new RepositoryDependencyResolverException(typeof(Int32), new ArgumentNullException());
            ex.InnerException.ShouldBeType<ArgumentNullException>();
        }
    }
}
