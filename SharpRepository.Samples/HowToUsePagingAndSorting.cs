using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository.Queries;
using Should;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Samples
{
    [TestFixture]
    public class HowToUsePagingAndSorting
    {
        /*
         * Paging and Sorting are both very common needs and SharpRepository handles
         * them well.  Let me show you how
         * 
         * Both query methods (GetAll and FindAll) have an optional QueryOptions parameter
         * which you will use when needing to sort or do paging.
         */

    public class Order
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
    }

        [Test]
        public void Repository_Handles_Sorting()
        {
            var repo = new InMemoryRepository<Order>();
            repo.Add(OrdersToLoad());

            // there are 2 ways to handle sorting, there is an Expression based way
            //  and a "magic string" based approach.
            // Why the 2 approaches?
            //  For convenience really.  In a Web based applicaiton sometimes it is easier to 
            //  post back a string that represents the properrty that you want to sort on.

            // First, the Expression way
            var descendingOrders = repo.GetAll(new SortingOptions<Order, DateTime>(x => x.OrderDate, isDescending: true));
            descendingOrders.First().OrderId.ShouldEqual(1);

            var ascendingOrders = repo.GetAll(new SortingOptions<Order, DateTime>(x => x.OrderDate, isDescending: false));
            ascendingOrders.First().OrderId.ShouldEqual(2);

            // You can also combine sortings and selectors (See HowToUseGetSelectors for more info)
            var descendingNames = repo.GetAll(x => x.Name, new SortingOptions<Order, DateTime>(x => x.OrderDate, isDescending: true));
            descendingNames.First().ShouldEqual("Order 1");

            // The Magic String approach to sorting
            //  you can see that you don't need the second generic type (the property type to sort on), just the name of the property
            ascendingOrders = repo.GetAll(new SortingOptions<Order>("OrderDate", isDescending: false));
            ascendingOrders.First().OrderId.ShouldEqual(2);

            // using sorting with FindAll
            var minDate = DateTime.Now.AddDays(-7);
            var ordersWithinAWeek = repo.FindAll(x => x.OrderDate > minDate, new SortingOptions<Order, double>(x => x.Total, true));
            ordersWithinAWeek.Count().ShouldEqual(2);
        }

        [Test]
        public void Repository_Handles_Pagination()
        {
            var repo = new InMemoryRepository<Order, int>();
            repo.Add(OrdersToLoad());

            // with PagingOptions you give it the pageNumber, number per page, and the sorting options
            //      since the sorting options are the same as described above, I will just do the Expression based approach here
            var pagingOptions = new PagingOptions<Order, DateTime>(1, 2, x => x.OrderDate, isDescending: true);
            var pageOneOrders = repo.GetAll(pagingOptions).ToList();

            pageOneOrders.Count.ShouldEqual(2);
            pageOneOrders.First().OrderId.ShouldEqual(1);
            pagingOptions.TotalItems.ShouldEqual(3);

            // now we can get the second page of results
            pagingOptions.PageNumber = 2;

            var pageTwoOrders = repo.GetAll(pagingOptions).ToList();

            pageTwoOrders.Count.ShouldEqual(1);
            pageTwoOrders.First().OrderId.ShouldEqual(2);
            pagingOptions.TotalItems.ShouldEqual(3);
        }


        private IEnumerable<Order> OrdersToLoad()
        {
            return new List<Order>()
                       {
                           new Order()
                               {
                                   Name = "Order 1",
                                   Total = 120.00,
                                   OrderDate = DateTime.Now
                               },
                           new Order()
                               {
                                   Name = "Order 2",
                                   Total = 80.00,
                                   OrderDate = DateTime.Now.AddMonths(-1)
                               }
                               ,
                           new Order()
                               {
                                   Name = "Order 3",
                                   Total = 100.00,
                                   OrderDate = DateTime.Now.AddDays(-2)
                               }
                       };
        }
    }
}
