using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForCompoundKeyRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static IEnumerable<TestCaseData> ForCompoundKeyRepositoriesTestCaseData
        {
            get
            {
                return CompoundKeyRepositoryTestCaseDataFactory.Build(_includeTypes);
            }
        }

        private static RepositoryTypes[] _includeTypes;
                
        public ExecuteForCompoundKeyRepositoriesAttribute(params RepositoryTypes[] repositoryTypes) : this()
        {
            _includeTypes = repositoryTypes;
        }

        public ExecuteForCompoundKeyRepositoriesAttribute()
            : base(typeof(ExecuteForCompoundKeyRepositoriesAttribute), "ForCompoundKeyRepositoriesTestCaseData")
        {
        }
    }
}