using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForAllCompoundKeyRepositoriesExceptAttribute : TestCaseSourceAttribute
    {
        private static string _testName;
        private static RepositoryType[] _exceptions;
        public static string Reason;

        private static IEnumerable<TestCaseData> ForAllCompoundKeyRepositoriesExceptTestCaseData
        {
            get
            {
                return CompoundKeyRepositoryTestCaseDataFactory.Build(RemoveExceptions(RepositoryTypes.CompoundKey), _testName);
            }
        }

        private static RepositoryType[] RemoveExceptions(RepositoryType[] repositoryType)
        {
            if (_exceptions == null || _exceptions.Length == 0)
                return repositoryType;

            var list = new List<RepositoryType>();
            list.AddRange(repositoryType);
            foreach (var exception in _exceptions.Where(list.Contains))
            {
                list.Remove(exception);
            }

            return list.ToArray();
        }

        public ExecuteForAllCompoundKeyRepositoriesExceptAttribute(string testName, params RepositoryType[] exceptions) : base(typeof(ExecuteForAllCompoundKeyRepositoriesExceptAttribute), "ForAllCompoundKeyRepositoriesExceptTestCaseData")
        {
            _testName = testName;
            _exceptions = exceptions;
        }
    }
}
