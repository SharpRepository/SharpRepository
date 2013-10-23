using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.EfRepository;
using SharpRepository.InMemoryRepository;
using SharpRepository.CacheRepository;


namespace SharpRepository.Tests.Integration.Data
{
    public class CompoundKeyRepositoryTestCaseDataFactory
    {
        public static IEnumerable<TestCaseData> Build(RepositoryType[] includeType)
        {
            if (includeType.Contains(RepositoryType.InMemory))
            {
                yield return new TestCaseData(new InMemoryRepository<User, string, int>()).SetName("InMemoryRepository Test");
            }

            if (includeType.Contains(RepositoryType.Ef5))
            {
                var dbPath = EfDataDirectoryFactory.Build();
                Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
                yield return new TestCaseData(new EfRepository<User, string, int>(new TestObjectEntities("Data Source=" + dbPath))).SetName("EfRepository Test");
            }

            if (includeType.Contains(RepositoryType.Cache))
            {
                yield return new TestCaseData(new CacheRepository<User, string, int>(CachePrefixFactory.Build())).SetName("CacheRepository Test");
            }
        }
    }
}