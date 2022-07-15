using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using SharpRepository.EfCoreRepository;
using SharpRepository.EfRepository;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.TestObjects;
using System.Collections.Generic;
using System.Linq;


namespace SharpRepository.Tests.Integration.Data
{
    public class CompoundKeyRepositoryTestCaseDataFactory
    {
        private static int efCoreProgressive = 0;

        public static IEnumerable<TestCaseData> Build(RepositoryType[] includeType, string testName)
        {
            if (includeType.Contains(RepositoryType.InMemory))
            {
                yield return new TestCaseData(new InMemoryRepository<User, string, int>()).SetName("InMemoryRepository " + testName);
            }

            if (includeType.Contains(RepositoryType.Ef))
            {
                yield return new TestCaseData(new EfRepository<User, string, int>(new TestObjectContext(Effort.DbConnectionFactory.CreateTransient()))).SetName("EfRepository " + testName);
            }

            if (includeType.Contains(RepositoryType.EfCore))
            {
                efCoreProgressive++;

                var options = new DbContextOptionsBuilder<TestObjectContextCore>()
                     .UseInMemoryDatabase($"EfCore {testName} {efCoreProgressive}")
                     .Options;

                // Create the schema in the database
                var context = new TestObjectContextCore(options);
                context.Database.EnsureCreated();
                yield return new TestCaseData(new EfCoreRepository<User, string, int>(context)).SetName("EfCoreRepository " + testName);
            }
        }
    }
}