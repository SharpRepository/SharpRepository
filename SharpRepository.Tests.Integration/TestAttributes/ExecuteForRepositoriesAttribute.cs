using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static string _testName;

        private static IEnumerable<TestCaseData> ForRepositoriesTestCaseData
        {
            get
            {
                return _testName == "ContactTypeTest" 
                    ? RepositoryContactTypeTestCaseDataFactory.Build(_includeType, _testName) 
                    : RepositoryTestCaseDataFactory.Build(_includeType, _testName);
            }
        }

        private static RepositoryType[] _includeType;
                
        public ExecuteForRepositoriesAttribute(params RepositoryType[] repositoryType) : this()
        {
            _includeType = repositoryType;
        }

        public ExecuteForRepositoriesAttribute(string testName = "Test", params RepositoryType[] repositoryType ) : this()
        {
            _includeType = repositoryType;
        }

        public ExecuteForRepositoriesAttribute(string testName = "Test") : base(typeof(ExecuteForRepositoriesAttribute), "ForRepositoriesTestCaseData")
        {
        }
    }
}