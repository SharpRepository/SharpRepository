using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.Ef5Repository;
using SharpRepository.InMemoryRepository;
using SharpRepository.CacheRepository;


namespace SharpRepository.Tests.Integration.Data
{
    public class CompoundKeyRepositoryTestCaseDataFactory
    {
        public static IEnumerable<TestCaseData> Build(RepositoryTypes[] includeTypes)
        {
            if (includeTypes.Contains(RepositoryTypes.InMemory))
            {
                yield return new TestCaseData(new InMemoryRepository<User, string, int>()).SetName("InMemoryRepository Test");
            }

            if (includeTypes.Contains(RepositoryTypes.Ef5))
            {
                var dbPath = EfDataDirectoryFactory.Build();
                Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
                yield return new TestCaseData(new Ef5Repository<User, string, int>(new TestObjectEntities("Data Source=" + dbPath))).SetName("EfRepository Test");
            }

            if (includeTypes.Contains(RepositoryTypes.Cache))
            {
                yield return new TestCaseData(new CacheRepository<User, string, int>(CachePrefixFactory.Build())).SetName("CacheRepository Test");
            }
        }
    }
}