using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForAllRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static string _testName;

        private static IEnumerable<TestCaseData> ForAllRepositoriesTestCaseData
        {
            get { return _testName == "ContactTypeTest" 
                    ? RepositoryContactTypeTestCaseDataFactory.Build(RepositoryTypes.All, _testName)
                    : RepositoryTestCaseDataFactory.Build(RepositoryTypes.All, _testName); }
        }

        public ExecuteForAllRepositoriesAttribute(string testName = "Test") : base(typeof(ExecuteForAllRepositoriesAttribute), "ForAllRepositoriesTestCaseData")
        {
            _testName = testName;
        }
    }
}