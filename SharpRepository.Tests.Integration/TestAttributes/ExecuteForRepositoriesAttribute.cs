using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static string _testName;
        private static RepositoryType[] _includeType;

        private static IEnumerable<TestCaseData> ForRepositoriesTestCaseData
        {
            get
            {
                return RepositoryTestCaseDataFactory.Build(_includeType, _testName);
            }
        }

        public ExecuteForRepositoriesAttribute(string testName, params RepositoryType[] repositoryType ) : base(typeof(ExecuteForRepositoriesAttribute), "ForRepositoriesTestCaseData")
        {
            _testName = testName;
            _includeType = repositoryType;
        }
    }
}