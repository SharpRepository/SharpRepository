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
                new TestDependencyResolver().GetService<ISomeFakeInterface>();

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
    
    public class TestDependencyResolver : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }

    public interface ISomeFakeInterface
    {
    }
}
