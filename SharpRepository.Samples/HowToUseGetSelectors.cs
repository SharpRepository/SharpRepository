using System;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using Shouldly;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Samples
{
    [TestFixture]
    public class HowToUseGetSelectors
    {
        /*
         * What are Get Selectors?
         * 
         * There are overloaded versions of the Get, GetAll, Find and FindAll methods
         * that allow the use of a selector expression.  You can think of it as what you 
         * would pass to the LINQ .Select() method.  
         * 
         * This allows you to only bring back the columns you are looking for to decrease
         * network traffic, or map from your Entity to a ViewModel or other object structure
         * that you need it in.
         * 
         * What does this look like in practice?
         * 
         * Let's try it out with some Orders and a OrderRepository
         */

        public class Order
        {
            public int OrderId { get; set; }
            public string Name { get; set; }
            public double Total { get; set; }
            public DateTime OrderDate { get; set; }
        }

        public class OrderViewModel
        {
            public string Name { get; set; }
            public bool IsExpensiveOrder { get; set; }
        }

        [Test]
        public void Repository_Supports_Selectors()
        {
            var repo = new InMemoryRepository<Order>();

            // let's add a couple of orders to work with
            repo.Add(new Order()
                         {
                             Name = "Order 1",
                             Total = 120.00,
                             OrderDate = new DateTime(2013, 4, 26)
                         });

            repo.Add(new Order()
                         {
                             Name = "Order 2",
                             Total = 80.00,
                             OrderDate = new DateTime(2013, 4, 24)
                         });

            // normal Get method
            var order = repo.Get(1);
            order.OrderId.ShouldBe(1);

            // in this case we only need the order name
            var orderName = repo.Get(1, x => x.Name);
            orderName.ShouldBe("Order 1");

            // we can also bring back an anonymous type if needed
            var anonymousType = repo.Get(1, x => new { Name = x.Name, IsExpensiveOrder = x.Total > 100.0 });
            anonymousType.IsExpensiveOrder.ShouldBeTrue();

            // or we can map it to a specific type we have defined like a ViewModel
            var viewModel = repo.Get(1, x => new OrderViewModel() {Name = x.Name, IsExpensiveOrder = x.Total > 100.0});
            viewModel.IsExpensiveOrder.ShouldBeTrue();

            // We have the same options with the GetAll, Find and FindAll as well
            orderName = repo.Find(x => x.OrderId == 2, x => x.Name);
            orderName.ShouldBe("Order 2");

            // we can also bring back an anonymous type if needed
            var anonymousTypes = repo.GetAll(x => new { Name = x.Name, IsExpensiveOrder = x.Total > 100.0 }).ToList();
            anonymousTypes.Count.ShouldBe(2);
            anonymousTypes.First().Name.ShouldBe("Order 1");
            anonymousTypes.First().IsExpensiveOrder.ShouldBeTrue();
            
            anonymousTypes.Last().Name.ShouldBe("Order 2");
            anonymousTypes.Last().IsExpensiveOrder.ShouldBeFalse();
            
            // or we can map it to a specific type we have defined like a ViewModel
            var viewModels = repo.FindAll(x => x.OrderId < 5, x => new OrderViewModel() { Name = x.Name, IsExpensiveOrder = x.Total > 100.0 }).ToList();
            viewModels.Count.ShouldBe(2);
            viewModels.First().Name.ShouldBe("Order 1");
            viewModels.First().IsExpensiveOrder.ShouldBeTrue();

            viewModels.Last().Name.ShouldBe("Order 2");
            viewModels.Last().IsExpensiveOrder.ShouldBeFalse();
        }
    }
}
