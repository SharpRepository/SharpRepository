using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using SharpRepository.EfRepository;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using Microsoft.Extensions.Caching.Memory;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class EfNullableForeignKeyCacheSpikes
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Deleted_Entity_With_Nullable_PartitionKey_Should_Not_Be_Returned_From_Cache()
        {
            var dbPath = EfDataDirectoryFactory.Build();

            // Standard partition cache and expression.
            var strategy = new StandardCachingStrategyWithPartition<Node, int, int?>(new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions())),
                n => n.ParentId);
            
            // Standard EF repo.
            IRepository<Node, int> repository =
                new EfRepository<Node, int>(new TestObjectContext("Data Source=" + dbPath), strategy);
            
            // Create a root node (ParentId = null).
            var rootNode = new Node() {ParentId = null, Name = "Root"};
            repository.Add(rootNode);

            // Note: If we dont assign an int? here then TryPartitionValue in StandardCachingStrategyBase cannot find the partition key in the expression
            // because there would be a cast made in the expression which breaks partition key discovery.
            int? rootNodeId = rootNode.Id;
            
            // Create a child node referencing the root node as it's parent.
            var childNode = new Node() {ParentId = rootNodeId, Name = "Child"};
            repository.Add(childNode);

            // Find the child nodes via the parentId, this will cause the cache to store the child node.
            repository.FindAll(n => n.ParentId == rootNodeId).Count().ShouldBe(1);

            // Deleting the child node should also increment the partition generation.
            repository.Delete(childNode);

            // Subsequent query should not return the child node.
            repository.FindAll(n => n.ParentId == rootNodeId).Count().ShouldBe(0, "Deleted entity returned from cache.");
        }
    }
}
