using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static IEnumerable<TestCaseData> ForRepositoriesTestCaseData
        {
            get
            {
                return RepositoryTestCaseDataFactory.Build(_includeTypes);
            }
        }

        private static RepositoryTypes[] _includeTypes;
                
        public ExecuteForRepositoriesAttribute(params RepositoryTypes[] repositoryTypes) : this()
        {
            _includeTypes = repositoryTypes;
        }

        public ExecuteForRepositoriesAttribute() : base(typeof(ExecuteForRepositoriesAttribute), "ForRepositoriesTestCaseData")
        {
        }
    }
}