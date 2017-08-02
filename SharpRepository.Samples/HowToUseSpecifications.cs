using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Specifications;
using Shouldly;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Samples
{
    [TestFixture]
    public class HowToSpecificationsTraits
    {
        /*
         * What Are Specifications?
         *
         * A specification defines an expression (or predicate) for which entities are evaluated
         * by asking the question "does this match?" In simpiest terms, specifications are passed 
         * into the repository as filtering criteria. All objects which satisify the spec are 
         * included in the result and those which don't match are excluded. Specifications may be 
         * used to evaluate a single object or a collection of objects.
         *                   
         * Why Should I Use Specifications?
         * 
         * The specification pattern is great for adhering to the Single Responsibility Principle (SRP). 
         * 
         * With specifications we isolate our predicate logic into a very readable, reusable and maintainable 
         * code structure. 
         * 
         * Testability is arguably the greatest benefit. Linq queries can become complexity and 
         * are traditionally only integration tested against an available and configured database. 
         * There's a lot of overhead to integration test setup, but with specifications the query logic is 
         * isolated into a single unit which can be tested in isolation.
         * 
         * How Do I Use Specifications?
         * 
         * Let's work through a few examples using Orders.         
         */
        public class Order
        {
            public int OrderId{ get; set; }
            public string Name { get; set; }
        }
        
        /*
         * Here's a very simple specification which asks the question: "Does this Order has OrderId equal to 1?"         
         */
        [Test]
        public void Specifications_Can_Be_Defined_Inline()
        {
            var spec = new Specification<Order>(p => p.OrderId == 1);
            var order = new Order { OrderId = 1 };
            spec.IsSatisfiedBy(order).ShouldBeTrue();
        }

        /* 
         * Though specifications may be defined inline, as we did above, there's little value in this approach
         * because their's no code isolation, reuse or testability. It's just like you are defining your
         * standard Linq query inline
         * 
         * The better approach is to define a specification which implements one of the specification base types.
         * Here's the same specification compiled into it's own reusable object.
         */
        public class OrderByIdSpecification : Specification<Order>
        {
            public OrderByIdSpecification(int id) : base(p => p.OrderId == id)
            {

            }
        }

        /*
         * Now we are really testing our isolated business rules.          
         */
        [Test]
        public void Specifications_Can_Be_Composed()
        {
            var spec = new OrderByIdSpecification(1);
            var order = new Order { OrderId = 1 };
            spec.IsSatisfiedBy(order).ShouldBeTrue();

            order = new Order { OrderId = 2 };
            spec.IsSatisfiedBy(order).ShouldBeFalse();
        }

        /*
         * Specifications can be combined using conditional expressions such as And, AndAlso, AndNot, Or, OrElse, OrNot and Not.          
         */
        public class OrderByNameSpecification : Specification<Order>
        {
            public OrderByNameSpecification(string name)
                : base(p => p.Name == name)
            {

            }
        }
        
        [Test]
        public void Specifications_Can_Be_Chained()
        {
            var spec = new OrderByIdSpecification(1)
                .And(new OrderByNameSpecification("test")); 
            
            var order = new Order { OrderId = 1, Name = "test"};
            spec.IsSatisfiedBy(order).ShouldBeTrue();
        }


        /* 
         * Finally, specifications may be used to filter Find() and FindAll() repository results. 
         * Though one can't really unit test the repository calls themselves (they would need to be integration tested)
         * the specifications -- the business rules offered to the repository -- can be.
         */
        [Test]
        public void SharpRepository_Supports_Find_Filtering_With_Specifications()
        {
            var repo = new InMemoryRepository<Order, int>();
            
            for (int i = 1; i <= 5; i++)
            {
                repo.Add(new Order{ Name = "Order" + i });
            }

            var order1 = repo.Find(new OrderByIdSpecification(1));
            order1.OrderId.ShouldBe(1);

            var order2 = repo.Find(new OrderByNameSpecification("Order2"));
            order2.Name.ShouldBe("Order2");
            
            var spec = new OrderByIdSpecification(3).And(new OrderByNameSpecification("Order3"));

            var order3 = repo.Find(spec);
            order3.OrderId.ShouldBe(3);
            order3.Name.ShouldBe("Order3");
        }

        [Test]
        public void SharpRepository_Supports_FindAll_Filtering_With_Specifications()
        {
            var repo = new InMemoryRepository<Order, int>();

            for (int i = 1; i <= 5; i++)
            {
                repo.Add(new Order { Name = "Order" + i });
            }

            var order1 = repo.FindAll(new OrderByIdSpecification(1));
            order1.First().OrderId.ShouldBe(1);

            var order2 = repo.FindAll(new OrderByNameSpecification("Order2"));
            order2.First().Name.ShouldBe("Order2");

            var spec = new OrderByIdSpecification(3).And(new OrderByNameSpecification("Order3"));

            var order3 = repo.FindAll(spec);
            order3.First().OrderId.ShouldBe(3);
            order3.First().Name.ShouldBe("Order3");
        }
    }
}