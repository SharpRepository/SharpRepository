using System;
using NUnit.Framework;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Tests.Ioc
{
    [TestFixture]
    public class DependencyResolverTests
    {
        [Test]
        public void No_Ioc_Configuration_Should_Throw_Exception()
        {
            try
            {
                new TestDependencyResolver().Resolve<ISomeFakeInterface>();

                Assert.Fail(); // exception was not throws
            }
            catch (RepositoryDependencyResolverException)
            {
                // ignore
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            
        }
    }
    
    public class TestDependencyResolver : BaseRepositoryDependencyResolver
    {
        protected override T ResolveInstance<T>()
        {
            throw new NotImplementedException();
        }

        protected override object ResolveInstance(Type type)
        {
            throw new NotImplementedException();
        }
    }

    public interface ISomeFakeInterface
    {
    }
}
