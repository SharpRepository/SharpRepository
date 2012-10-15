using MongoDB.Driver;
using NUnit.Framework;
using Rhino.Mocks;

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

        /// <summary>
        /// Create a mock
        /// </summary>
        /// <typeparam name="T">Type to be mocked</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>T</returns>
        protected static T M<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateMock<T>(argumentsForConstructor);
        }

        /// <summary>
        /// Create a partial mock
        /// </summary>
        /// <typeparam name="T">Type to be partially mocked</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>T</returns>
        protected static T Pm<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GeneratePartialMock<T>(argumentsForConstructor);
        }

        /// <summary>
        /// Create a stub
        /// </summary>
        /// <typeparam name="T">Type to be stubbed</typeparam>
        /// <param name="argumentsForConstructor">Constructor arguments</param>
        /// <returns>T</returns>
        protected static T S<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateStub<T>(argumentsForConstructor);
        }

        protected static class AssertIgnores
        {
            public static void MongoServerIsNotRunning()
            {
                Assert.Ignore("MongoServer is NOT running. MongoDbRepository integration tests are excluded from the test suite until MongoDb installed on this machine. Get MongoDb installed and running on Windows: http://docs.mongodb.org/manual/tutorial/install-mongodb-on-windows/");
            }

            public static void CouchDbServerIsNotRunning()
            {
                Assert.Ignore("CouchDb  is NOT running. CouchDbRepository integration tests are excluded from the test suite until CouchDb installed on this machine. Get CouchDb installed and running on Windows: http://wiki.apache.org/couchdb/Installing_on_Windows");
            }  
        }
        
    }
}