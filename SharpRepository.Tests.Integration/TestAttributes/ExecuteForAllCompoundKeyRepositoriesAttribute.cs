using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForAllCompoundKeyRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static string _testName;

        private static IEnumerable<TestCaseData> ForAllCompoundKeyRepositoriesTestCaseData
        {
            get
            {
                return CompoundKeyRepositoryTestCaseDataFactory.Build(RepositoryTypes.CompoundKey, _testName);
            }
        }

        public ExecuteForAllCompoundKeyRepositoriesAttribute(string testName)
            : base(typeof(ExecuteForAllCompoundKeyRepositoriesAttribute), "ForAllCompoundKeyRepositoriesTestCaseData")
        {
            _testName = testName;
        }
    }
}