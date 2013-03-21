using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForAllRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static IEnumerable<TestCaseData> ForAllRepositoriesTestCaseData
        {
            get { return RepositoryTestCaseDataFactory.Build(new[]
                                                                 {
//                                                                     RepositoryTypes.Dbo4, 
                                                                     RepositoryTypes.RavenDb, 
//                                                                     RepositoryTypes.Xml,
//                                                                     RepositoryTypes.MongoDb, 
//                                                                     RepositoryTypes.InMemory, 
//                                                                     RepositoryTypes.Ef5, 
//                                                                     RepositoryTypes.Cache, 
                                                                     RepositoryTypes.CouchDb
                                                                 }); }
        }

        public ExecuteForAllRepositoriesAttribute() : base(typeof(ExecuteForAllRepositoriesAttribute), "ForAllRepositoriesTestCaseData")
        {
        }
    }
}