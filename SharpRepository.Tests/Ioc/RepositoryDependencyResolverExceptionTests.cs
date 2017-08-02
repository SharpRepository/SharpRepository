using System;
using NUnit.Framework;
using SharpRepository.Repository.Ioc;
using Shouldly;

namespace SharpRepository.Tests.Ioc
{
    [TestFixture]
    public class RepositoryDependencyResolverExceptionTests
    {
        [Test]
        public void DependencyType_Should_Get_Set()
        {
            var ex = new RepositoryDependencyResolverException(typeof (Int32));
            ex.DependencyType.ShouldBe(typeof(Int32));
        }

        [Test]
        public void InnerException_Should_Get_Set()
        {
            var ex = new RepositoryDependencyResolverException(typeof(Int32), new ArgumentNullException());
            ex.InnerException.ShouldBeOfType<ArgumentNullException>();
        }
    }
}
