using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForAllRepositoriesExceptAttribute : TestCaseSourceAttribute
    {
        private static IEnumerable<TestCaseData> ForAllRepositoriesExceptTestCaseData
        {
            get
            {
                return RepositoryTestCaseDataFactory.Build(RemoveExceptions(RepositoryTypes.All), _testName);
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

        private static RepositoryType[] _exceptions;
        private static string _testName;
        public string Reason { get; set;  }


        public ExecuteForAllRepositoriesExceptAttribute(string testName, params RepositoryType[] exceptions) : this()
        {
            _exceptions = exceptions;
            _testName = testName;
        }

        public ExecuteForAllRepositoriesExceptAttribute(params RepositoryType[] exceptions) : this("Test", exceptions)
        {
        }

        public ExecuteForAllRepositoriesExceptAttribute()
            : base(typeof(ExecuteForAllRepositoriesExceptAttribute), "ForAllRepositoriesExceptTestCaseData")
        {
        }
    }
}
