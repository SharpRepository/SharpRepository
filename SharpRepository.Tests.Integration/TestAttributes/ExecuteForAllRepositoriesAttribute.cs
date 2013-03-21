using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForAllRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static IEnumerable<TestCaseData> ForAllRepositoriesTestCaseData
        {
            get { return RepositoryTestCaseDataFactory.Build(RepositoryTypes.All); }
        }

        public ExecuteForAllRepositoriesAttribute() : base(typeof(ExecuteForAllRepositoriesAttribute), "ForAllRepositoriesTestCaseData")
        {
        }
    }
}