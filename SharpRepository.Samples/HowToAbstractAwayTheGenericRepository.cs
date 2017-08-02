using NUnit.Framework;
using SharpRepository.Repository;
using Shouldly;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Samples
{
    [TestFixture]
    public class HowToAbstractAwayTheGenericRepository
    {
        /*
         * Why add a layer of abstraction?
         *
         * You might find that instantiating a repository which is based on 
         * a generic base is a bit verbose since the entity and key types
         * are requirements of "newing up" a repository.
         * 
         * Let's say we have a simple Order.
         */
        public class Order
        {
            public int OrderId { get; set; }
            public string Name { get; set; }
        }

        /* 
         * Here's one way to create an InMemoryRepository of type Order where the 
         * key is an Int32.
         */
        [Test]
        public void InMemoryRepository_Requires_Generic_Type_And_KeyType()
        {
            var repo = new InMemoryRepository<Order, int>();
            repo.ShouldNotBeNull();
        }

        /* 
         * Alternatively, you can tighten things up a bit through simple 
         * inheritance. 
         */
        private class OrderRepository : InMemoryRepository<Order, int>{}
        
        /*
         * Now we can "new up" the OrderRepository which has the generic
         * types defined in the base. Cleaner, no?
         */
        [Test]
        public void OrderRepository_Can_Abstract_Away_Generic_Messiness()
        {
            var repo = new OrderRepository();
            repo.ShouldNotBeNull();
        }

        /* 
         * Though using custom repository wrappers is encouraged, 95% of the
         * time keys will integers. To accommodate, the repository is 
         * overloaded and the key type is defaulted to Int32 if one isn't 
         * provided.
         */
        [Test]
        public void InMemoryRepository_Defaults_Key_To_Int()
        {
            var repo = new InMemoryRepository<Order>();
            repo.ShouldNotBeNull();
        }
        
        /* 
         * NOTE: Perhaps you're more interested taking these basic abstraction
         * technique to another level? Check out HowToUseTraits which helps with
         * repository API customization.
         */
    }
}