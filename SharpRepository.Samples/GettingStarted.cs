using NUnit.Framework;
using SharpRepository.Repository;
using Shouldly;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Samples
{
    [TestFixture]
    public class GettingStarted
    {
        /*
        * Getting Started
        *
        * Let's keep things simple. The Repository pattern is a facade that abstracts 
        * away the technical concerns of your persistence implementation by offering a 
        * collection-like interface for managing domain objects.         
        * 
        * SharpRepository is a persistent ignorant seam which sits between your 
        * application and data access layer. SharpRepository provides the generic base
        * but also numerous persistence implementations (EF, RavenDb, etc) which can 
        * be swapped based on project needs. By offering the needed separation
        * of concerns, SharpRepository promotes the ability to unit test agains mocks
        * as opposed to intergration testing against the database itself.
        * 
        * For more information on the Repository Pattern:
        * 
        * http://domaindrivendesign.org/books/#DDD
        * http://martinfowler.com/eaaCatalog/repository.html
        * http://msdn.microsoft.com/en-us/library/ff649690.aspx
        * 
        * CRUD Operations
        * 
        * The most basic repository operations are CRUD (create, read, update, delete).
        * Here we define the entity type, Order, which will be persisted and we execute
        * the four CRUD operations. 
        */
        public class Order
        {
            public int OrderId{ get; set; }
            public string Name { get; set; }
        }
        
        [Test]
        public void SharpRepository_Supports_Basic_Crud_Operations()
        {
            // Declare your generic InMemory Repository.  
            // Check out HowToAbstractAwayTheGenericRepository.cs for cleaner ways to new up a repo.
            var repo = new InMemoryRepository<Order, int>();
            
            // Create 
            var create = new Order { Name = "Big sale" };
            repo.Add(create);

            const int expectedOrderId = 1;
            create.OrderId.ShouldBe(expectedOrderId);

            // Read 
            var read = repo.Get(expectedOrderId);
            read.Name.ShouldBe(create.Name);
            
            // Update
            read.Name = "Really big sale";
            repo.Update(read);
            
            var update = repo.Get(expectedOrderId);
            update.OrderId.ShouldBe(expectedOrderId);
            update.Name.ShouldBe(read.Name);

            // Delete
            repo.Delete(update);
            var delete = repo.Get(expectedOrderId);
            delete.ShouldBeNull();
        }
    }
}