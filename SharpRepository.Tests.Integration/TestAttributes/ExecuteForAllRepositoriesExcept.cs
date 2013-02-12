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
                var repositoryTypes = new[]
                                          {
                                                                     RepositoryTypes.Dbo4, 
                                                                     RepositoryTypes.RavenDb, 
                                                                     RepositoryTypes.Xml,
                                                                     RepositoryTypes.MongoDb, 
                                                                     RepositoryTypes.InMemory, 
                                                                     RepositoryTypes.Ef5, 
                                                                     RepositoryTypes.Cache, 
                                          };

                return RepositoryTestCaseDataFactory.Build(RemoveExceptions(repositoryTypes));
            }
        }

        private static RepositoryTypes[] RemoveExceptions(RepositoryTypes[] repositoryTypes)
        {
            if (_exceptions == null || _exceptions.Length == 0)
                return repositoryTypes;

            var list = new List<RepositoryTypes>();
            list.AddRange(repositoryTypes);
            foreach (var exception in _exceptions.Where(list.Contains))
            {
                list.Remove(exception);
            }

            return list.ToArray();
        }

        private static RepositoryTypes[] _exceptions;
        private static string _reason;

        public ExecuteForAllRepositoriesExceptAttribute(string reason, params RepositoryTypes[] exceptions) : this()
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
