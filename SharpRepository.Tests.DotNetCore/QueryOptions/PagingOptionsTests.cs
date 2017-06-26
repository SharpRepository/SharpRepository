using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository.Queries;
using SharpRepository.Tests.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.QueryOptions
{
    [TestFixture]
    public class PagingOptionsTests : TestBase
    {
        [Test]
        public void PagingOptions_PageNumber_Will_Be_Set_In_Constructor()
        {
            new PagingOptions<Contact>(1, 10, "Name").PageNumber.ShouldBe(1);
            new PagingOptions<Contact, string>(1, 10, m => m.Name).PageNumber.ShouldBe(1);
        }

        [Test]
        public void PagingOptions_PageSize_Will_Be_Set_In_Constructor()
        {
            new PagingOptions<Contact>(1, 10, "Name").PageSize.ShouldBe(10);
            new PagingOptions<Contact, string>(1, 10, m => m.Name).PageSize.ShouldBe(10);
        }

        [Test]
        public void PagingOptions_Apply_Will_Set_TotalItems()
        {
            var contacts = new List<Contact>();
            for (int i = 1; i <= 5; i++)
            {
                contacts.Add(new Contact { Name = "Test User " + i });
            }

            const int resultingPage = 2;
            const int pageSize = 2;
            var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", isDescending: true);
            qo.Apply(contacts.AsQueryable());
            qo.TotalItems.ShouldBe(5);

            var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, isDescending: true);
            qo2.Apply(contacts.AsQueryable());
            qo2.TotalItems.ShouldBe(5);
        }

        [Test]
        public void PagingOptions_Apply_Return_Requested_Page()
        {
            var contacts = new List<Contact>();
            for (int i = 1; i <= 5; i++)
            {
                contacts.Add(new Contact { Name = "Test User " + i });
            }

            const int resultingPage = 2;
            const int pageSize = 2;

            var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", isDescending: true);
            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldBe(2);
            queryable.First().Name.ShouldBe("Test User 3");

            var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, isDescending: true);
            queryable = qo2.Apply(contacts.AsQueryable());
            queryable.Count().ShouldBe(2);
            queryable.First().Name.ShouldBe("Test User 3");
        }

        [Test]
        public void PagingOptions_Apply_Will_Set_TotalItems_With_Multiple_Sort()
        {
            var contacts = new List<Contact>();
            for (int i = 1; i <= 5; i++)
            {
                contacts.Add(new Contact { Name = "Test User " + i });
            }

            const int resultingPage = 2;
            const int pageSize = 2;
            var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", isDescending: true);
            qo.ThenSortBy("ContactTypeId");
            qo.Apply(contacts.AsQueryable());
            qo.TotalItems.ShouldBe(5);

            var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, isDescending: true);
            qo2.ThenSortBy(x => x.ContactTypeId);
            qo2.Apply(contacts.AsQueryable());
            qo2.TotalItems.ShouldBe(5);
        }

        [Test]
        public void PagingOptions_Apply_Return_Requested_Page_With_Multiple_Sort()
        {
            var contacts = new List<Contact>();
            for (int i = 1; i <= 5; i++)
            {
                contacts.Add(new Contact { Name = "Test User " + (i % 2), ContactTypeId = i});
            }

            const int resultingPage = 2;
            const int pageSize = 2;

            var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", isDescending: true);
            qo.ThenSortBy("ContactTypeId", isDescending: true);
            
            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldBe(2);

            var contact = queryable.First();
            contact.Name.ShouldBe("Test User 1");
            contact.ContactTypeId.ShouldBe(1);

            var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, isDescending: true);
            qo2.ThenSortBy(x => x.ContactTypeId, isDescending: true);

            queryable = qo2.Apply(contacts.AsQueryable());
            queryable.Count().ShouldBe(2);

            contact = queryable.First();
            contact.Name.ShouldBe("Test User 1");
            contact.ContactTypeId.ShouldBe(1);
        }
    }
}