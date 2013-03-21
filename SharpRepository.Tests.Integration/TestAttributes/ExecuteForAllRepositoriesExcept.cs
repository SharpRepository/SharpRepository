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
                return RepositoryTestCaseDataFactory.Build(RemoveExceptions(RepositoryTypes.All));
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
        private static string _reason;

        public ExecuteForAllRepositoriesExceptAttribute(string reason, params RepositoryType[] exceptions) : this()
        {
            _reason = reason; // TODO: it would be nice to find a way to display this in the Unit test results, but helpful in the unit test for human readable in the code
            _exceptions = exceptions;
        }

        public ExecuteForAllRepositoriesExceptAttribute()
            : base(typeof(ExecuteForAllRepositoriesExceptAttribute), "ForAllRepositoriesExceptTestCaseData")
        {
        }
    }
}
