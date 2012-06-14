using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.QueryOptions
{
    [TestFixture]
    public class PagingOptionsTests : TestBase
    {
        [Test]
        public void PagingOptions_PageNumber_Will_Be_Set_In_Constructor()
        {
            new PagingOptions<Contact>(1, 10, "Name").PageNumber.ShouldEqual(1);
            new PagingOptions<Contact, string>(1, 10, m => m.Name).PageNumber.ShouldEqual(1);
        }

        [Test]
        public void PagingOptions_PageSize_Will_Be_Set_In_Constructor()
        {
            new PagingOptions<Contact>(1, 10, "Name").PageSize.ShouldEqual(10);
            new PagingOptions<Contact, string>(1, 10, m => m.Name).PageSize.ShouldEqual(10);
        }

        [Test]
        public void PagingOptions_SortProperty_Will_Be_Set_In_Constructor()
        {
            new PagingOptions<Contact>(1, 10, "Name").SortProperty.ShouldEqual("Name");
            new PagingOptions<Contact, string>(1, 10, m => m.Name).SortExpression.GetPropertyName().ShouldEqual("Name");
        }

        [Test]
        public void PagingOptions_SortDirection_Will_Be_Set_In_Constructor()
        {
            new PagingOptions<Contact>(1, 10, "Name").IsDescending.ShouldBeFalse();
            new PagingOptions<Contact, string>(1, 10, m => m.Name).IsDescending.ShouldBeFalse();

            new PagingOptions<Contact>(1, 10, "Name", true).IsDescending.ShouldBeTrue();
            new PagingOptions<Contact, string>(1, 10, m => m.Name, true).IsDescending.ShouldBeTrue();
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
            var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", true);
            qo.Apply(contacts.AsQueryable());
            qo.TotalItems.ShouldEqual(5);

            var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, true);
            qo2.Apply(contacts.AsQueryable());
            qo2.TotalItems.ShouldEqual(5);
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

            var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", true);
            IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
            queryable.Count().ShouldEqual(2);
            queryable.First().Name.ShouldEqual("Test User 3");

            var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, true);
            queryable = qo2.Apply(contacts.AsQueryable());
            queryable.Count().ShouldEqual(2);
            queryable.First().Name.ShouldEqual("Test User 3");
        }
    }
}