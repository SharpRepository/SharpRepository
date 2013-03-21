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
                return CompoundKeyRepositoryTestCaseDataFactory.Build(_includeType);
            }
        }

        private static RepositoryType[] _includeType;
                
        public ExecuteForCompoundKeyRepositoriesAttribute(params RepositoryType[] repositoryType) : this()
        {
            _includeType = repositoryType;
        }

        public ExecuteForCompoundKeyRepositoriesAttribute()
            : base(typeof(ExecuteForCompoundKeyRepositoriesAttribute), "ForCompoundKeyRepositoriesTestCaseData")
        {
        }
    }
}