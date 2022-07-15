using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForCompoundKeyRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static string _testName;

        private static RepositoryType[] _includeType;

        private static IEnumerable<TestCaseData> ForCompoundKeyRepositoriesTestCaseData
        {
            get
            {
                return CompoundKeyRepositoryTestCaseDataFactory.Build(_includeType, _testName);
            }
        }
                        
        public ExecuteForCompoundKeyRepositoriesAttribute(string testName, params RepositoryType[] repositoryType) : base(typeof(ExecuteForCompoundKeyRepositoriesAttribute), "ForCompoundKeyRepositoriesTestCaseData")
        {
            _testName = testName;
            _includeType = repositoryType;
        }
    }
}