using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Traits;
using System.Linq;
using Shouldly;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Samples
{
    [TestFixture]
    public class HowToUseTraits
    {
        /*
         * What Are Traits?
         *
         * Traits are based on the Interface Segregation Principle (ISP)         
         * which is the notion that 'many client specific interfaces are 
         * better than one general purpose interface.' As Uncle Bob puts
         * it, "Make fine grained interfaces that are client specific" 
         * or don't force clients to implement interfaces they don’t use. 
         *  
         * http://en.wikipedia.org/wiki/Interface_segregation_principle
         * http://en.wikipedia.org/wiki/Solid_(object-oriented_design)
         *  
         * Traits are those segregated interfaces which expose a subset
         * of what RepositoryBase and its inheritors offer. Traits map to
         * repository operations. For example, ICanAdd trait is an interface
         * with exposes the repository Add methods. Other examples include
         * ICanUpdate and ICanDelete.
         *  
         * Note, this implementation was inspired by Richard Dingwall's article,
         * "IRepository: one size does not fit all"
         * http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/
         * 
         * Why Should I Use Traits?
         *  
         * By default, the repository exposes all CRUD operations through 
         * public methods. That's convenient but it might not play nicely 
         * with your business rules and thus the repository API you wish to 
         * code against. Let's say your business spec states that Orders can
         * never be changed once issued. If you code directly against 
         * InMemoryRepository<Order, int> you'll have access to operations 
         * like Add(), Get(), Update() and Delete(). If you wish to reduce 
         * your OrderRepository API to a subset of methods -- just Add() and 
         * Get() -- you'll need to leverage traits and you need to start coding
         * against an IOrderRepository interface rather than the implementation
         * itself. 
         * 
         * How Do I Use Traits?
         * 
         * Let's work through an example using the OrderRepository called 
         * out above.  
         * 
         * First, define the Order. 
         */
        public class Order
        {
            public int OrderId{ get; set; }
            public string Name { get; set; }
        }

       /* 
        * Next, define a custom IOrderRepository which limits the Order 
        * repository to Add and Get operations.
        */
        private interface IOrderRepository : ICanAdd<Order>, ICanGet<Order, int> {}

       /* 
        * Finally, create a concrete implementation of the OrderRepository.        
        */
        private class OrderRepository : InMemoryRepository<Order, int>, IOrderRepository {}

       /*
        * That's it. Check out the examples below. 
        * 
        * In the first test, you'll see that IOrderRepository has been
        * reduced to only the Add and Get operations.
        */
        [Test]
        public void IOrderRepository_Can_Only_Add_And_Get()
        {
            IOrderRepository repo = new OrderRepository();
            repo.Add(new Order{Name = "Big Sale"});

            var result = repo.GetAll();
            result.Count().ShouldBe(1);
        }

        /*
         * But be sure to code against the IOrderRepository rather
         * than OrderRepository directly. Otherwise, additional operations
         * like Delete() are still availabe.
         */
        [Test]
        public void OrderRepository_Can_Do_More_Than_Add_And_Get()
        {
            var order = new Order {Name = "Big Sale"};

            var repo = new OrderRepository();
            repo.Add(order);
            order.OrderId.ShouldBe(1);
            
            repo.GetAll().Count().ShouldBe(1);

            repo.Delete(order);
            repo.GetAll().Count().ShouldBe(0);
        }
    }
}