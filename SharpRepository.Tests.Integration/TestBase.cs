using NUnit.Framework;
using Moq;

namespace SharpRepository.Tests.Integration
{
    public abstract class TestBase
    {
        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Teardown()
        {
        }

        protected static T N<T>() where T : class
        {
            return default(T);
        }

        // <summary>
        /// Create a mock
        /// </summary>
        /// <typeparam name="T">Type to be mocked</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>T</returns>
        protected static T M<T>(params object[] argumentsForConstructor) where T : class
        {
            var mock = new MockRepository(MockBehavior.Default).Create<T>(argumentsForConstructor);
            return mock.Object;
        }

        /// <summary>
        /// Create a partial mock
        /// </summary>
        /// <typeparam name="T">Type to be partially mocked</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>T</returns>
        protected static T Pm<T>(params object[] argumentsForConstructor) where T : class
        {
            return M<T>(argumentsForConstructor);
        }

        /// <summary>
        /// Create a stub
        /// </summary>
        /// <typeparam name="T">Type to be stubbed</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>T</returns>
        protected static T S<T>(params object[] argumentsForConstructor) where T : class
        {
            return M<T>(argumentsForConstructor);
        }

        protected static class AssertIgnores
        {
            public static void MongoServerIsNotRunning()
            {
                Assert.Ignore("MongoServer is NOT running. MongoDbRepository integration tests are excluded from the test suite until MongoDb installed on this machine. Get MongoDb installed and running on Windows: http://docs.mongodb.org/manual/tutorial/install-mongodb-on-windows/");
            }    
        }
    }
}